using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Rigidbody _rb;
    [SerializeField] private float _speed = 5f;
    [SerializeField] private float _turnSpeed = 360f;

    private Vector3 _movementInput;

    void Update()
    {
        HandleInput();
        Look();
    }

    void FixedUpdate()
    {
        Move();
    }

    void HandleInput()
    {
        float horizontal = Input.GetAxis("Horizontal"); 
        float vertical = Input.GetAxis("Vertical"); 


        _movementInput = new Vector3(horizontal, 0, vertical).normalized;
    }

    void Look()
    {
        if (_movementInput != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(_movementInput, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, _turnSpeed * Time.deltaTime);
        }
    }

    void Move()
    {
       
        Vector3 moveDirection = _movementInput * _speed * Time.fixedDeltaTime;
        _rb.MovePosition(transform.position + moveDirection);
    }
}
