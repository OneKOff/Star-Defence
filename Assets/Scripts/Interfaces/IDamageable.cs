using UnityEngine;

public interface IDamageable
{
    int MaxHealth { get; set; }
    int CurrentHealth { get; set; }
    
    void CheckTrigger(Collider2D other);
    void CheckCollision(Collision2D other);
    void TakeDamage(int amount);
    void GetDestroyed();
}
