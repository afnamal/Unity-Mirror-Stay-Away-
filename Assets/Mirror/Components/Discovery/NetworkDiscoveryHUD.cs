using System.Collections.Generic;
using UnityEngine;

namespace Mirror.Discovery
{
    [DisallowMultipleComponent]
    [AddComponentMenu("Network/Network Discovery HUD")]
    [HelpURL("https://mirror-networking.gitbook.io/docs/components/network-discovery")]
    [RequireComponent(typeof(NetworkDiscovery))]
    public class NetworkDiscoveryHUD : MonoBehaviour
    {
        readonly Dictionary<long, ServerResponse> discoveredServers = new Dictionary<long, ServerResponse>();
        Vector2 scrollViewPos = Vector2.zero;

        public NetworkDiscovery networkDiscovery;

#if UNITY_EDITOR
        void OnValidate()
        {
            if (networkDiscovery == null)
            {
                networkDiscovery = GetComponent<NetworkDiscovery>();
                UnityEditor.Events.UnityEventTools.AddPersistentListener(networkDiscovery.OnServerFound, OnDiscoveredServer);
                UnityEditor.Undo.RecordObjects(new Object[] { this, networkDiscovery }, "Set NetworkDiscovery");
            }
        }
#endif

        void OnGUI()
        {
            if (NetworkManager.singleton == null)
                return;

            GUIStyle buttonStyle = new GUIStyle(GUI.skin.button)
            {
                fontSize = 20,
                fixedWidth = 200,
                fixedHeight = 60
            };

            GUIStyle labelStyle = new GUIStyle(GUI.skin.label)
            {
                fontSize = 24,
                alignment = TextAnchor.MiddleCenter
            };

            if (!NetworkClient.isConnected && !NetworkServer.active && !NetworkClient.active)
                DrawGUI(buttonStyle, labelStyle);

            if (NetworkServer.active || NetworkClient.active)
                StopButtons(buttonStyle);
        }

        void DrawGUI(GUIStyle buttonStyle, GUIStyle labelStyle)
        {
            GUILayout.BeginArea(new Rect(10, 10, 350, 600));
            GUILayout.BeginVertical();

            if (GUILayout.Button("Oyuncuları Bul", buttonStyle))
            {
                discoveredServers.Clear();
                networkDiscovery.StartDiscovery();
            }

            // LAN Host
            if (GUILayout.Button("Oyun Başlat", buttonStyle))
            {
                discoveredServers.Clear();
                NetworkManager.singleton.StartHost();
                networkDiscovery.AdvertiseServer();
            }

            // Dedicated server
            

            GUILayout.Label($"Discovered Servers [{discoveredServers.Count}]:", labelStyle);

            // servers
            scrollViewPos = GUILayout.BeginScrollView(scrollViewPos);

            foreach (ServerResponse info in discoveredServers.Values)
            {
                if (GUILayout.Button(info.EndPoint.Address.ToString(), buttonStyle))
                {
                    Connect(info);
                }
            }

            GUILayout.EndScrollView();
            GUILayout.EndVertical();
            GUILayout.EndArea();
        }

        void StopButtons(GUIStyle buttonStyle)
        {
            GUILayout.BeginArea(new Rect(10, 10, 350, 600));
            GUILayout.BeginVertical();

            // stop host if host mode
            if (NetworkServer.active && NetworkClient.isConnected)
            {
                if (GUILayout.Button("Oturumu Sonlandır", buttonStyle))
                {
                    NetworkManager.singleton.StopHost();
                    networkDiscovery.StopDiscovery();
                }
            }
            // stop client if client-only
            else if (NetworkClient.isConnected)
            {
                if (GUILayout.Button("Oturumu Sonlandır", buttonStyle))
                {
                    NetworkManager.singleton.StopClient();
                    networkDiscovery.StopDiscovery();
                }
            }
            // stop server if server-only
            else if (NetworkServer.active)
            {
                if (GUILayout.Button("Oturumu Sonlandır", buttonStyle))
                {
                    NetworkManager.singleton.StopServer();
                    networkDiscovery.StopDiscovery();
                }
            }

            GUILayout.EndVertical();
            GUILayout.EndArea();
        }

        void Connect(ServerResponse info)
        {
            networkDiscovery.StopDiscovery();
            NetworkManager.singleton.StartClient(info.uri);
        }

        public void OnDiscoveredServer(ServerResponse info)
        {
            // Note that you can check the versioning to decide if you can connect to the server or not using this method
            discoveredServers[info.serverId] = info;
        }
    }
}
