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
    [SerializeField] Animator animator;
    [SerializeField] Transform weaponTransform;
    public LayerMask enemyLayer;

    private bool attackPressed;
    public bool healPressed = false;

    [SerializeField] float attackRadius = 1f;
    [SerializeField] int attackDamage = 1;
    [SerializeField] float attackZoneOrigin = 0.4f;
    [SerializeField] float cooldownTime = 0.5f;
    [SerializeField] float cooldownTimer = 0f;
    public float attackCount = 0;

    [SerializeField, Tooltip("0: up, 1: down, 2:left, 3: right")] List<string> commands;
    [SerializeField] string currentCommand;

    private AttackDirection currentAttackDirection;
    private AttackDirection previousAttackDirection;

    private void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
        weaponTransform = GameObject.FindGameObjectWithTag("weapon").transform;
        animator = GameObject.FindGameObjectWithTag("weapon").GetComponent<Animator>();
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
            attackPressed = true;
        }
    }

    public void OnHeal(InputValue value)
    {
        if (value.isPressed)
        {
            healPressed = true;
        }
    }

    private void FixedUpdate()
    {
        setAttackDirection();
        HandleAttack();
    }

    private void setAttackDirection()
    {
        if (playerMovement.moveDirectionX < 0)
        {
            currentAttackDirection = AttackDirection.attackLeft;
            currentCommand = commands[2];
        }
        else if (playerMovement.moveDirectionX > 0)
        {
            currentAttackDirection = AttackDirection.attackRight;
            currentCommand = commands[3];
        }
        else
        {
            if (playerMovement.moveDirectionY > 0)
            {
                currentAttackDirection = AttackDirection.attackUp;
                currentCommand = commands[0];
            }
            else if (playerMovement.moveDirectionY < 0)
            {
                currentAttackDirection = AttackDirection.attackDown;
                currentCommand = commands[1];
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
        weaponTransform.position = attackZone.position;
    }

    public void HandleAttack()
    {
        if (cooldownTimer <= 0f)
        {
            attackZone.position = playerOrigin.position;
            if (attackPressed)
            {
                animator.SetTrigger(currentCommand);
                SetAttackZone();
                Collider2D[] enemiesinRange = Physics2D.OverlapCircleAll(attackZone.position, attackRadius, enemyLayer);
                foreach (var enemy in enemiesinRange)
                {
                    if (enemy.GetComponent<EHealth>() != null)
                    {
                        enemy.GetComponent<EHealth>().TakeDamage(attackDamage);
                        if (attackCount < 10)
                        {
                            attackCount = attackCount + 1;
                        }
                    }
                    else if(enemy.GetComponentInParent<JunkyardBossLogic>() != null)
                    {
                        enemy.GetComponentInParent<JunkyardBossLogic>().TakeDamage(attackDamage);
                        if (attackCount < 10)
                        {
                            attackCount = attackCount + 1;
                        }
                    }
                }
                cooldownTimer = cooldownTime;
                attackPressed = false;
            }
        }
        else
        {
            cooldownTimer -= Time.deltaTime;
        }
    }
}
