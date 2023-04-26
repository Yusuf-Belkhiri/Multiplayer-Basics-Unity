using Mirror;
using UnityEngine;

public class MyNetworkManager : NetworkManager
{
    // public override void OnClientConnect()
    // {
    //     base.OnClientConnect();
    //     Debug.Log("I (client) connected to the server!");
    // }

    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    {
        base.OnServerAddPlayer(conn);
        
        // Set the client's name & color when he joins
        MyNetworkPlayer player = conn.identity.GetComponent<MyNetworkPlayer>();
        player.SetDisplayName($"Player {numPlayers}");
        player.SetColor(new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f)));

        //print("A player is added to this server, number of players: " + numPlayers);
    }
}
