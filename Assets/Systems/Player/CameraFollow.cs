using Photon.Pun;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; // Assign this at runtime

    private void Start()
    {
        // Check if this is the local player's car
        PhotonView pv = target.GetComponent<PhotonView>();
        if (!pv.IsMine)
        {
            gameObject.SetActive(false); // Disable the camera for non-local players
        }
    }

    private void LateUpdate()
    {
        if (target != null)
        {
            transform.position = target.position + new Vector3(0, 5, -10);
            transform.LookAt(target);
        }
    }
}