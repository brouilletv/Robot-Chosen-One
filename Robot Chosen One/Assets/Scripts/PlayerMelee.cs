using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMelee : MonoBehaviour
{
    public Transform attackOrigin;
    public float attackRadius = 1f;    
    public LayerMask enemyLayer;

    public int attackDamage = 25;

    public float cooldownTime = 0.5f;
    private float cooldownTimer = 0f;

    bool attackPressed;

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(attackOrigin.position, attackRadius);
    }

    public void OnAttack(InputValue value)
    {
        if (value.isPressed)
        {
            Debug.Log("Attack Pressed");
            attackPressed = true;
        }
    }

    private void FixedUpdate()
    {
        HandleAttack();
    }

    public void HandleAttack()
    {
        if (cooldownTimer <= 0f)
        {   
            Debug.Log("Ready to Attack");
            if (attackPressed)
            {
                Debug.Log("Attacking");
                Collider2D[] enemiesinRange = Physics2D.OverlapCircleAll(attackOrigin.position, attackRadius, enemyLayer);
                foreach (var enemy in enemiesinRange)
                {
                    Debug.Log("Hit ");
                    //Change the EnemyHealth for it to work 
                    //enemy.GetComponent<EnemyHealth>().TakeDamage(attackDamage);

                }
                cooldownTimer = cooldownTime;
                attackPressed = false;
            }
        }
        else
        {
            Debug.Log("On Cooldown");
            cooldownTimer -= Time.deltaTime;
        }
    }
}
