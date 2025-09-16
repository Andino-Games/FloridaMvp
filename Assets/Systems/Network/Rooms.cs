using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Rooms : MonoBehaviourPunCallbacks
{
    [SerializeField] private TMP_InputField joinInputfield, createInputfield;
    [SerializeField] private GameObject loadingScreen; // Asigna el Panel de la UI en el Inspector

    private string joinRoomName = "";
    private string createRoomName = "";

    private void Start()
    {
        PhotonNetwork.AutomaticallySyncScene = true; // Sincroniza la escena para todos
        joinInputfield.onValueChanged.AddListener(text => joinRoomName = text);
        createInputfield.onValueChanged.AddListener(text => createRoomName = text);
    }


    [ContextMenu("Create")]
    public void CreateRoom()
    {
        if (string.IsNullOrEmpty(createRoomName))
        {
            Debug.LogError("⚠ No puedes crear una sala sin nombre.");
            return;
        }

        RoomOptions roomOptions = new RoomOptions
        {
            MaxPlayers = 2, // Solo dos jugadores para la carrera
            EmptyRoomTtl = 5000
        };

        PhotonNetwork.CreateRoom(createRoomName, roomOptions);
    }

    [ContextMenu("Join")]
    public void JoinRoom()
    {
        if (!PhotonNetwork.InLobby)
        {
            Debug.LogWarning(" No estás en el lobby. Intentando unirse...");
            PhotonNetwork.JoinLobby();
            return;
        }

        if (string.IsNullOrEmpty(joinRoomName))
        {
            Debug.LogWarning("⚠ Nombre de la sala vacío. Intentando unirse a una aleatoria...");
            PhotonNetwork.JoinRandomRoom();
            return;
        }

        PhotonNetwork.JoinOrCreateRoom(joinRoomName, new RoomOptions { MaxPlayers = 2 }, TypedLobby.Default);
    }


    public override void OnJoinedRoom()
    {
        Debug.Log($" Te has unido a la sala {PhotonNetwork.CurrentRoom.Name}");

        
        if (PhotonNetwork.CurrentRoom.PlayerCount == 1)
        {
            loadingScreen.SetActive(true);
        }
        else
        {
           
            StartRace();
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log($"🔹 {newPlayer.NickName} ha entrado a la sala.");

        
        if (PhotonNetwork.CurrentRoom.PlayerCount == 2)
        {
            StartRace();
        }
    }

    private void StartRace()
    {
        Debug.Log(" ¡La carrera va a empezar!");
        loadingScreen.SetActive(false); 
        PhotonNetwork.LoadLevel("Game"); 
    }
}
