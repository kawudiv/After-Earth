using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Rigidbody _rb;
    [SerializeField] private float _speed = 5f;
    [SerializeField] private float _sprintMultiplier = 2f;

    private Vector3 _movementInput;
    private bool _isSprinting;

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

        _isSprinting = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
    }

    void Look()
    {
        if (_movementInput != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(_movementInput, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, 360f * Time.deltaTime);
        }
    }

    void Move()
    {
        float speed = _isSprinting ? _speed * _sprintMultiplier : _speed;
        Vector3 moveDirection = _movementInput * speed * Time.fixedDeltaTime;
        _rb.MovePosition(transform.position + moveDirection);
    }
}
