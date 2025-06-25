using UnityEngine;

public interface IDamagable : IHitable
{
    void TakeDamage(Vector2 damageSourceDir, int _damageAmount, float knockBackThrust);
}