using UnityEngine;
using Photon.Pun;

public class FinishLine : MonoBehaviourPunCallbacks
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PhotonView pv = other.GetComponent<PhotonView>();

            if (pv != null && pv.IsMine)
            {
                Debug.Log($" {pv.Owner.NickName} cruzó la meta");
                GameManager.Instance.photonView.RPC("PlayerFinished", RpcTarget.MasterClient, pv.Owner.NickName);
            }
        }
    }
}