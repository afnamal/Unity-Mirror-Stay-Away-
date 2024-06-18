using Mirror;
using UnityEngine;

public class CustomNetworkManager : NetworkManager
{
    public GameObject playerPrefab2;

    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    {
        GameObject player;

        // İlk oyuncu ise varsayılan prefab'ı kullan
        if (numPlayers == 0)
        {
            player = Instantiate(playerPrefab);
        }
        // İkinci oyuncu ise playerPrefab2'yi kullan
        else
        {
            player = Instantiate(playerPrefab2);
        }

        NetworkServer.AddPlayerForConnection(conn, player);
    }
}
