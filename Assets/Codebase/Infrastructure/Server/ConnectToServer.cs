using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;

namespace Codebase.Infrastructure.Server
{
    /// <summary>
    /// boot класс для подключения к серверу
    /// </summary>
    public class ConnectToServer : MonoBehaviourPunCallbacks
    {
        private void Start()
        {
            PhotonNetwork.ConnectUsingSettings();
        }

        public override void OnConnectedToMaster()
        {
            PhotonNetwork.JoinLobby(TypedLobby.Default);
        }

        public override void OnJoinedLobby()
        {
            SceneManager.LoadScene("Lobby_server");
        }
    }
}