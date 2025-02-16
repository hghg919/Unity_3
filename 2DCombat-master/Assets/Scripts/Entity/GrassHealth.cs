using Cinemachine;
using UnityEngine;

public class GrassHealth : BaseHealth
{
    public override void Damage(int amount, Vector2 attackDirection)
    {
        HasTakenDamgage = true;
        CurrentHealth -= amount;
        // Sound
        PlayRandomSFX();
        // Effect
        SpawnDamageParticle(attackDirection);

        Die();
    }
}
