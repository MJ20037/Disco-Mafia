using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 10f;
    [SerializeField] private GameObject _bulletVFX;
    [SerializeField] private int _damageAmount = 1;
    [SerializeField] private float _knockbackThrust = 10f;

    private Vector2 _fireDirection;
    private Gun _gun;
    private Rigidbody2D _rigidBody;

    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
    }

    public void InIt(Gun gun, Vector2 bulletSpawnPos, Vector2 mousePos)
    {
        _gun = gun;
        transform.position=bulletSpawnPos;
        _fireDirection = (mousePos - bulletSpawnPos).normalized;
    }

    private void FixedUpdate()
    {
        _rigidBody.linearVelocity = _fireDirection * _moveSpeed;
    }

    private void OnTriggerEnter2D(Collider2D other) {

         Instantiate(_bulletVFX, transform.position, Quaternion.identity);

        IHitable iHitable= other.gameObject.GetComponent<IHitable>();
        iHitable?.TakeHit();

        IDamagable iDamagable= other.gameObject.GetComponent<IDamagable>();
        iDamagable?.TakeDamage(transform.position,_damageAmount,_knockbackThrust);

        _gun.ReleaseBulletFromPool(this);
    }
}