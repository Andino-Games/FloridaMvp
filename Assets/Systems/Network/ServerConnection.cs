using Photon.Pun;
using UnityEngine;

public class ServerConnection : MonoBehaviourPunCallbacks
{
    void Start()
    {
        Debug.Log(" Conectando a Photon...");
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log(" Conectado al Master Server.");
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        Debug.Log(" Has entrado al lobby.");
    }

    public override void OnDisconnected(Photon.Realtime.DisconnectCause cause)
    {
        Debug.LogError($" Desconectado de Photon. Razón: {cause}");
    }
}