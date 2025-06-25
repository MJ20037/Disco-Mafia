using System;
using UnityEngine;
using UnityEngine.Pool;
using Unity.Cinemachine;
using System.Collections;

public class Gun : MonoBehaviour
{
    public static Action OnShoot;
    public static Action OnGrenadeShoot;

    [SerializeField] private Transform _bulletSpawnPoint;
    [Header("Bullet")]
    [SerializeField] private Bullet _bulletPrefab;
    [SerializeField] private float _gunFireCD = 0.5f;
    [SerializeField] private GameObject _muzzleFlash;
    [SerializeField] private float _muzzleFlashTime = 0.05f;
    [Header("Grenade")]
    [SerializeField] private GameObject _grenadePrefab;
    [SerializeField] private float _grenadeShootCD = 0.8f;

    private Coroutine _muzzleFlashRoutine;
    private ObjectPool<Bullet> _bulletPool;
    private static readonly int FIRE_HASH = Animator.StringToHash("Fire");
    private Vector2 _mousePos;
    private float _lastFireTime = 0f;
    private float _lastGrenadeTime = 0f;

    private PlayerInput _playerInput;
    private FrameInput _frameInput;
    private CinemachineImpulseSource _impulseSource;

    private Animator _animator;

    private void Awake()
    {
        _impulseSource = GetComponent<CinemachineImpulseSource>();
        _animator = GetComponent<Animator>();
        _playerInput = GetComponentInParent<PlayerInput>();
        _frameInput= _playerInput.FrameInput;
    }

    private void Start()
    {
        CreateBulletPool();
    }

    private void Update()
    {
        GatherInput();
        RotateGun();
        Shoot();
    }

    private void OnEnable()
    {
        OnShoot += ShootProjectile;
        OnShoot += ResetLastFireTime;
        OnShoot += FireAnimation;
        OnShoot += GunScreenShake;
        OnShoot += MuzzleFlash;
        OnGrenadeShoot += ShootGrenade;
        OnGrenadeShoot += FireAnimation;
        OnGrenadeShoot += ResetGrenadeTime;
    }

    public void ReleaseBulletFromPool(Bullet bullet)
    {
        _bulletPool.Release(bullet);
    }

    private void GatherInput()
    {
        _frameInput = _playerInput.FrameInput;
    }

    private void OnDisable()
    {
        OnShoot -= ShootProjectile;
        OnShoot -= ResetLastFireTime;
        OnShoot -= FireAnimation;
        OnShoot -= GunScreenShake;
        OnShoot -= MuzzleFlash;
        OnGrenadeShoot -= ShootGrenade;
        OnGrenadeShoot -= FireAnimation;
        OnGrenadeShoot -= ResetGrenadeTime;
    }

    private void CreateBulletPool()
    {
        _bulletPool = new ObjectPool<Bullet>(
            () =>
            {
                return Instantiate(_bulletPrefab);
            }, bullet =>
            {
                bullet.gameObject.SetActive(true);
            }, bullet =>
            {
                bullet.gameObject.SetActive(false);
            }, bullet =>
            {
                Destroy(bullet);
            }, false, 20, 40
        );
    }

    private void Shoot()
    {
        _lastFireTime += Time.deltaTime;
        _lastGrenadeTime += Time.deltaTime;
        if (Input.GetMouseButton(0) && _lastFireTime >= _gunFireCD)
        {
            OnShoot?.Invoke();
        }

        if (_frameInput.Grenade && _lastGrenadeTime >= _grenadeShootCD)
        {
            OnGrenadeShoot?.Invoke();
        }
    }

    private void ShootGrenade()
    {
        Instantiate(_grenadePrefab, _bulletSpawnPoint.position, Quaternion.identity);
        
    }

    private void ResetLastFireTime()
    {
        _lastFireTime = 0;
    }

    private void ResetGrenadeTime()
    {
        _lastGrenadeTime = 0;
    }

    private void ShootProjectile()
    {
        Bullet newBullet = _bulletPool.Get();
        newBullet.InIt(this, _bulletSpawnPoint.position, _mousePos);
    }

    private void FireAnimation()
    {
        _animator.Play(FIRE_HASH, 0, 0.0f);
    }

    private void GunScreenShake()
    {
        _impulseSource.GenerateImpulse();
    }

    private void RotateGun()
    {
        _mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        Vector2 direction = PlayerController.Instance.transform.InverseTransformPoint(_mousePos);
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.localRotation = Quaternion.Euler(0, 0, angle);
    }

    private void MuzzleFlash()
    {
        if (_muzzleFlashRoutine != null)
        {
            StopCoroutine(MuzzleFlashRoutine());
        }
        _muzzleFlash.SetActive(true);
        _muzzleFlashRoutine = StartCoroutine(MuzzleFlashRoutine());
    }

    private IEnumerator MuzzleFlashRoutine()
    {
        yield return new WaitForSeconds(_muzzleFlashTime);
        _muzzleFlash.SetActive(false);
    }
}
