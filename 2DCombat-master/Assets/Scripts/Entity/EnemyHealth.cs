using Cinemachine;
using System;
using UnityEngine;

[RequireComponent(typeof(CinemachineImpulseSource))]
public class EnemyHealth : BaseHealth
{
    private CinemachineImpulseSource _impulseSource;
    [SerializeField] private ScreenShakeSO profile;

    HPBar _hpBar;

    protected override void Start()
    {
        base.Start();
        _impulseSource = GetComponent<CinemachineImpulseSource>();
        _hpBar = GetComponentInChildren<HPBar>();
    }

    public override void Damage(int amount, Vector2 attackDirection)
    {
        HasTakenDamgage = true;
        CurrentHealth -= amount;
        _hpBar.UpdateHPBar(MaxHealth, CurrentHealth);
        CameraShakeManager.Instance.CameraShakeFromProfile(_impulseSource, profile);
        // Sound
        PlayRandomSFX();
        // Effect
        SpawnDamageParticle(attackDirection);

        Die();
    }
}
