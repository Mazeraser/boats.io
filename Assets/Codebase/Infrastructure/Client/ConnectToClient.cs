using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;

namespace Codebase.Infrastructure.Client
{
    /// <summary>
    /// boot класс для подключения к серверу
    /// </summary>
    public class ConnectToClient : MonoBehaviourPunCallbacks
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
            SceneManager.LoadScene("Lobby_client");
        }
    }
}