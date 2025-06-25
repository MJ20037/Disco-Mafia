using UnityEngine;

public class Movement : MonoBehaviour
{
    public bool CanMove => _canMove;
    [SerializeField] private float _moveSpeed = 10f;

    private float _moveX;
    private bool _canMove=true;
    private Rigidbody2D _rigidBody;

    private Knockback knockback;

    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
        knockback= GetComponent<Knockback>();
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void OnEnable()
    {
        knockback.OnKnockbackStart += CanMoveFalse;
        knockback.OnKnockbackEnd += CanMoveTrue;
    }

    private void OnDisable()
    {
        knockback.OnKnockbackStart -= CanMoveFalse;
        knockback.OnKnockbackEnd -= CanMoveTrue;
    }

    private void CanMoveTrue()
    {
        _canMove = true;
    }

    private void CanMoveFalse()
    {
        _canMove = false;
    }

    public void SetCurrentDirection(float currentDirection)
    {
        _moveX = currentDirection;
    }

    private void Move()
    {
        if (!_canMove) return;
        Vector2 movement = new Vector2(_moveX * _moveSpeed, _rigidBody.linearVelocityY);
        _rigidBody.linearVelocity = movement;
    }
}
