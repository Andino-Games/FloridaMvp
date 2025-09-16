using Photon.Pun;
using UnityEngine;

namespace Systems.Player
{
    [RequireComponent(typeof(Rigidbody), typeof(PhotonView))]
    public class Movement : MonoBehaviour
    {
        [SerializeField] private float speed = 5f;
        [SerializeField] private float jumpForce = 5f;
        [SerializeField] private float raycastLength = 1.1f;
        [SerializeField] private LayerMask groundLayer;
        
        private bool canMove = false;

        private Rigidbody _rb;
        private PhotonView _photonView;
        private Vector3 _moveDirection;
        private bool _isGrounded;

        void Awake()
        {
            _rb = GetComponent<Rigidbody>();
            _photonView = GetComponent<PhotonView>();
        }

        void Update()
        {
            if (!_photonView.IsMine) return; 

            CheckGrounded();
            MoveDirection();
            Move();

            if (Input.GetKeyDown(KeyCode.Space) && _isGrounded)
            {
                Jump();
            }
        }

        private void MoveDirection()
        {
            _moveDirection.x = Input.GetAxisRaw("Horizontal");
            _moveDirection.z = Input.GetAxisRaw("Vertical");
            _moveDirection.y = 0;
            _moveDirection *= speed;
        }

        public void EnableMovement()
        {
            canMove = true; 
        }

        private void Move()
        {
            if (!canMove) return; 

            _rb.velocity = new Vector3(_moveDirection.x, _rb.velocity.y, _moveDirection.z);
        }

        private void Jump()
        {
            _rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            _isGrounded = false;
        }

        private void CheckGrounded()
        {
            RaycastHit hit;
            _isGrounded = Physics.Raycast(transform.position, Vector3.down, out hit, raycastLength, groundLayer);
            
            Debug.DrawRay(transform.position, Vector3.down * raycastLength, _isGrounded ? Color.green : Color.red);
            if (_isGrounded) Debug.Log(" En el suelo");
            else Debug.Log(" En el aire");
        }
    }
}
