using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMeleeAttack : MonoBehaviour
{
    [SerializeField] private Transform attackTransform;
    [SerializeField] private float attackRange = 1.5f;
    [SerializeField] private LayerMask attackableLayer;
    [SerializeField] private int damageAmount = 1;
    [SerializeField] private float attackCD = 0.15f;

    RaycastHit2D[] hits;
    Animator anim;
    private float attackCoolTimeCheck;

    public bool ShouldBeDamage { get; set; }
    private List<Idamagable> idamagables = new List<Idamagable>();

    public float AttackCD => attackCD;

    private void Start()
    {
        anim = GetComponent<Animator>();
        attackCoolTimeCheck = attackCD;
    }


    private void Update()
    {
        if(InputUser.Instance.control.Attack.MeleeAttack.WasPressedThisFrame() && attackCoolTimeCheck >= attackCD)
        {
            attackCoolTimeCheck = 0;
        
            anim.SetTrigger("attack");
        }

        attackCoolTimeCheck += Time.deltaTime;
    }

    public IEnumerator AttackAvailabe()
    {
        ShouldBeDamage = true;

        while (ShouldBeDamage)
        {
            hits = Physics2D.CircleCastAll(attackTransform.position, attackRange, transform.right, 0, attackableLayer);

            for (int i = 0; i < hits.Length; i++)
            {
                Idamagable enemyHealth = hits[i].collider.GetComponent<Idamagable>();

                if (enemyHealth != null && !enemyHealth.HasTakenDamgage)
                {
                    enemyHealth.Damage(damageAmount, transform.right);
                    idamagables.Add(enemyHealth);
                }
            }

            yield return null; 
        }

        // ������ �ٽ� �� �� �ִ� �����Դϴ�.
        ReturnAttackableState();
    }

    private void ReturnAttackableState()
    {
        // Idamagable �迭, List
        // idamagables ����Ʈ �ȿ� �ִ� ��� ���Ұ� HasTakenDamage�� false �ٲ��

        foreach (var damagable in idamagables)
        {
            damagable.HasTakenDamgage = false;
        }

        idamagables.Clear();
    }

    public void ShouldBeDamageTrue()
    {
        ShouldBeDamage = true;
    }

    public void ShouldBeDamageFalse()
    {
        ShouldBeDamage = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(attackTransform.position, attackRange);
    }

}
