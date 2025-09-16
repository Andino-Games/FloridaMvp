using UnityEngine;
using Photon.Pun;
using TMPro;
using System.Collections;
using Systems.Player;

public class GameManager : MonoBehaviourPunCallbacks
{
    public static GameManager Instance;
    public TMP_Text countdownText;
    public GameObject resultPanel;
    public TMP_Text resultText;
    public GameObject restartButton, menuButton;

    private bool _raceStarted = false;
    private int _playersFinished = 0;
    private string _winner = "";

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        photonView.RPC("StartCountdown", RpcTarget.All);
    }

    [PunRPC]
    private void StartCountdown()
    {
        StartCoroutine(CountdownCoroutine());
    }

    private IEnumerator CountdownCoroutine()
    {
        int countdown = 3;
        while (countdown >= 0)
        {
            countdownText.text = countdown > 0 ? countdown.ToString() : "GO!";
            yield return new WaitForSeconds(1f);
            countdown--;
        }
        countdownText.gameObject.SetActive(false);
        _raceStarted = true;

        EnablePlayerMovement();
    }

    private void EnablePlayerMovement()
    {
        Movement[] players = FindObjectsOfType<Movement>();
        foreach (var player in players)
        {
            player.EnableMovement();
        }
    }

    [PunRPC]
    public void PlayerFinished(string playerName)
    {
        if (!_raceStarted) return;

        if (PhotonNetwork.IsMasterClient)
        {
            _playersFinished++;
            

            if (_playersFinished == 1)
            {
                _winner = playerName;
                Debug.Log($" Primer lugar: {_winner}");
            }

            if (_playersFinished == 2) // Cuando ambos terminen
            {
                photonView.RPC("ShowResults", RpcTarget.All, _winner);
            }
        }
    }

    [PunRPC]
    private void ShowResults(string winner)
    {
        if (PhotonNetwork.NickName == winner)
        {
            photonView.RPC("DisplayResult", RpcTarget.All, winner, "¡Ganaste!");
        }
        else
        {
            photonView.RPC("DisplayResult", RpcTarget.All, winner, "Perdiste.");
        }
    }

    [PunRPC]
    private void DisplayResult(string winner, string message)
    {
        resultText.text = message;
        resultPanel.SetActive(true);
    }

    public void RestartGame()
    {
        PhotonNetwork.LoadLevel("Game");
    }

    public void GoToMenu()
    {
        PhotonNetwork.LoadLevel("Menu");
    }
}
