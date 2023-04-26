using System;
using System.Threading.Tasks;
using Mirror;
using TMPro;
using UnityEngine;

public class MyNetworkPlayer : NetworkBehaviour
{
    [SyncVar(hook = nameof(HandleDisplayNameUpdated))]          // call HandleDisplayColorUpdated whenever the display name gets updated on a client
    [SerializeField] private string _displayName = "Missing Name";        // Synced var
    
    [SyncVar(hook = nameof(HandleDisplayColorUpdated))]
    [SerializeField] private Color _displayColor = Color.red;
    
    // Appearance
    [SerializeField] private TMP_Text _displayNameText = null;
    [SerializeField] private Renderer _displayColorRenderer = null;


    // Executed in the Server, but not obliged to be called from the server
    #region SERVER (Exectution in Server)

    // Those are being used (called) from the server
    [Server]
    public void SetDisplayName(string newDisplayName)       // Called only from the server
    {
        // SERVER AUTHORITY (We can check here if the name is valid)
        // Here, it will be checked in both cases if the server OR the client wants to set the name
        //if (newDisplayName.Length < 2 || newDisplayName.Length > 20) return;
        
        _displayName = newDisplayName;
    }

    [Server]
    public void SetColor(Color newColor)
    {
        _displayColor = newColor;
    }

    
    // Those are being called from the client (but executed in the server)
    // REMOTE ACTION
    [Command]
    private void CmdSetDisplayName(string newDisplayName)           // The client calls this method on the server (execution on the server)
    {
        // SERVER AUTHORITY (We can check here if the name is valid), OR WE CAN CHECK IT INSIDE  SetDisplayName()
        // Here, it will be checked only when the client wants to set his name
        if (newDisplayName.Length is < 2 or > 20) return;
        
        RpcLogNewName(newDisplayName);      // called from the server & executed on the client
        
        SetDisplayName(newDisplayName);
    }
    #endregion
    

    // Executed in the Client, but not obliged to be called from the client
    #region CLIENT (Exectution in Client)

    // HOOK METHODS:    they must take 2 parameters
    private void HandleDisplayNameUpdated(string oldName, string newName)        // Update _displayNameText whenever _displayName is updated (from the server)
    {
        _displayNameText.SetText(newName);
    }
    
    private void HandleDisplayColorUpdated(Color oldColor, Color newColor)
    {
        _displayColorRenderer.material.SetColor("_BaseColor", newColor);
    }


    [ContextMenu("Set My Name")]        // to be called from the inspector
    private void SetMyName()
    {
        CmdSetDisplayName("M");
    }
    
    
    // REMOTE ACTIONS
    [ClientRpc]
    private void RpcLogNewName(string newDisplayName)
    {
        Debug.Log($"Client got a new name: {newDisplayName}");
    }
    #endregion


    // private async void Start()
    // {
    //     await Task.Delay(5);
    //     SetDisplayName("TestName");
    // }
}
