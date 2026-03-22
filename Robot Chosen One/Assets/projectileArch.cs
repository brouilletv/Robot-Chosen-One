using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class projectileArch : MonoBehaviour
{
    private string playerDirection;
    private int rangeDmg;
    private int playerDirectionInt;

    private Vector2 target;
    private float T;
    private float gravity = -9.81f;

    private Vector2 startPos;
    private Vector2 velocity;

    private float decayTime = 10;

    private LayerMask playerMask;
    private LayerMask groundMask;

    public static event Action<int> Hit;
    public static event Action<int> HitBounce;

    void Update()
    {
        // Apply gravity
        velocity.y += gravity * Time.deltaTime;

        // Move
        transform.position += (Vector3)(velocity * Time.deltaTime);
    }

    public void Initializeprojectile(Vector2 target, Vector2 startPos, float time, string playerDirection, int rangeDmg)
    {
        this.target = target;
        this.startPos = startPos;
        this.T = time;
        this.playerDirection = playerDirection;
        this.rangeDmg = rangeDmg;

        Vector2 toTarget = target - startPos;

        float vx = toTarget.x / T;
        float vy = (toTarget.y - 0.5f * gravity * T * T) / T;

        velocity = new Vector2(vx, vy);

        StartCoroutine(DestroyCo());

        playerMask = LayerMask.GetMask("PlayerMask");
        groundMask = LayerMask.GetMask("Ground", "Wall");
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if ((playerMask.value & (1 << other.gameObject.layer)) > 0)
        {
            Destroy(gameObject);

            if (playerDirection == "Right")
            {
                playerDirectionInt = 1;
            }
            else if (playerDirection == "Left")
            {
                playerDirectionInt = 0;
            }

            ApplyDmg(rangeDmg);
            Bouce(playerDirectionInt);
        }
        else if ((groundMask.value & (1 << other.gameObject.layer)) > 0)
        {
            Destroy(gameObject);
        }
    }
    void ApplyDmg(int dmg)
    {
        Hit?.Invoke(dmg);
    }

    void Bouce(int side)
    {
        HitBounce?.Invoke(side);
    }

    IEnumerator DestroyCo()
    {
        yield return new WaitForSeconds(decayTime);
        Destroy(gameObject);
    }
}
