using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerMelee : MonoBehaviour
{
    private PlayerMovement playerMovement;
    [SerializeField] Transform playerOrigin;
    [SerializeField] Transform attackZone;
    public LayerMask enemyLayer;

    private bool attackPressed;

    [SerializeField] float attackRadius = 1f;
    [SerializeField] int attackDamage = 1;
    [SerializeField] float attackZoneOrigin = 0.4f;
    [SerializeField] float cooldownTime = 0.5f;
    [SerializeField] float cooldownTimer = 0f;

    private AttackDirection currentAttackDirection;
    private AttackDirection previousAttackDirection;

    private void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
    }


    private enum AttackDirection
    {
        attackUp = 0,
        attackDown = 1,
        attackLeft = 2,
        attackRight = 3
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackZone.position, attackRadius);
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
        setAttackDirection();
        HandleAttack();
    }

    private void setAttackDirection()
    {
        if (playerMovement.moveDirectionY > 0)
        {
            currentAttackDirection = AttackDirection.attackUp;
        }
        else if (playerMovement.moveDirectionY < 0)
        {
            currentAttackDirection = AttackDirection.attackDown;
        }
        else
        {
            if (playerMovement.moveDirectionX < 0)
            {
                currentAttackDirection = AttackDirection.attackLeft;
            }
            else if (playerMovement.moveDirectionX > 0)
            {
                currentAttackDirection = AttackDirection.attackRight;
            }
        }

        if ((playerMovement.moveDirectionY != 0) || (playerMovement.moveDirectionX != 0))
        {
            previousAttackDirection = currentAttackDirection;
        }
    }

    private void SetAttackZone()
    {
        if (previousAttackDirection == AttackDirection.attackUp)
        {
            Vector3 modifier = new Vector3(0f, attackZoneOrigin, playerOrigin.position.z);
            attackZone.position = playerOrigin.position + modifier;
        }
        else if (previousAttackDirection == AttackDirection.attackDown)
        {
            Vector3 modifier = new Vector3(0f, -attackZoneOrigin, playerOrigin.position.z);
            attackZone.position = playerOrigin.position + modifier;
        }
        else if (previousAttackDirection == AttackDirection.attackLeft)
        {
            Vector3 modifier = new Vector3(-attackZoneOrigin, 0f, playerOrigin.position.z);
            attackZone.position = playerOrigin.position + modifier;
        }
        else if (previousAttackDirection == AttackDirection.attackRight)
        {
            Vector3 modifier = new Vector3(attackZoneOrigin, 0f, playerOrigin.position.z);
            attackZone.position = playerOrigin.position + modifier;
        }
    }

    public void HandleAttack()
    {
        if (cooldownTimer <= 0f)
        {
            attackZone.position = playerOrigin.position;
            Debug.Log("Ready to Attack");
            if (attackPressed)
            {
                Debug.Log("Attacking");
                SetAttackZone();
                Collider2D[] enemiesinRange = Physics2D.OverlapCircleAll(attackZone.position, attackRadius, enemyLayer);
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
