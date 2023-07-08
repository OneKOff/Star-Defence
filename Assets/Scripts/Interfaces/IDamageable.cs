using UnityEngine;

public interface IDamageable
{
    void CheckTrigger(Collider2D other);
    void CheckCollision(Collision2D other);
    void TakeDamage(int amount);
    void GetDestroyed();
}
