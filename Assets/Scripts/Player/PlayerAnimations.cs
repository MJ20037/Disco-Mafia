using Unity.Cinemachine;
using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
    [SerializeField] private ParticleSystem _moveDustVFX;
    [SerializeField] private ParticleSystem _poofDustVFX;

    [SerializeField] private float _tiltAngle = 15f;
    [SerializeField] private float _tiltSpeed = 5f;
    [SerializeField] private float _hatTiltSpeed = 2f;
    [SerializeField] private Transform _playerSpriteTransform;
    [SerializeField] private Transform _hatSpriteTransform;
    [SerializeField] private float _yLandVelocityCheck = -20f;

    private Vector2 _velocityBeforePhysicsUpdate;
    private Rigidbody2D _rigidbody;
    private CinemachineImpulseSource _impulseSource;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _impulseSource=GetComponents<CinemachineImpulseSource>()[1];
    }

    private void Update()
    {
        DetectMoveDust();
        ApplyTilt();
    }
    private void OnEnable()
    {
        PlayerController.OnJump += PlayPoofDustVFX;
    }

    private void OnDisable()
    {
        PlayerController.OnJump -= PlayPoofDustVFX;
    }

    private void FixedUpdate()
    {
        _velocityBeforePhysicsUpdate = _rigidbody.linearVelocity;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (_velocityBeforePhysicsUpdate.y < _yLandVelocityCheck)
        {
            PlayPoofDustVFX();
            _impulseSource.GenerateImpulse();
        }      
    }

    private void DetectMoveDust()
    {
        if (PlayerController.Instance.CheckGrounded())
        {
            if (!_moveDustVFX.isPlaying)
            {
                _moveDustVFX.Play();
            }
        }
        else
        {
            if (_moveDustVFX.isPlaying)
            {
                _moveDustVFX.Stop();
            }
        }
    }

    private void PlayPoofDustVFX() {
        _poofDustVFX.Play();
    }

    private void ApplyTilt()
    {
        float targetAnglePlayer;
        float targetAngleHat;

        if (PlayerController.Instance.MoveInput.x < 0f)
        {
            targetAnglePlayer = _tiltAngle;
            targetAngleHat = -_tiltAngle;
        }
        else if (PlayerController.Instance.MoveInput.x > 0f)
        {
            targetAnglePlayer = -_tiltAngle;
            targetAngleHat = _tiltAngle;
        }
        else
        {
            targetAnglePlayer = 0f;
            targetAngleHat = 0f;
        }

        //Player Sprite
        Quaternion currentCharacterRotation = _playerSpriteTransform.rotation;
        Quaternion targetCharacterRotation = Quaternion.Euler(currentCharacterRotation.eulerAngles.x, currentCharacterRotation.eulerAngles.y, targetAnglePlayer);

        _playerSpriteTransform.rotation = Quaternion.Lerp(currentCharacterRotation, targetCharacterRotation, _tiltSpeed * Time.deltaTime);

        //Hat Sprite
        Quaternion currentHatRotation = _hatSpriteTransform.rotation;
        Quaternion targetHatRotation = Quaternion.Euler(currentHatRotation.eulerAngles.x, currentHatRotation.eulerAngles.y, targetAngleHat);
        _hatSpriteTransform.rotation = Quaternion.Lerp(currentHatRotation, targetHatRotation, _hatTiltSpeed * Time.deltaTime);
    }
}
