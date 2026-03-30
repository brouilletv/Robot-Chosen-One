using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class projectileArch : MonoBehaviour
{
    private string playerDirection;
    private float rangeDmg = 1f;
    private int playerDirectionInt;

    private Vector2 target;
    private float T;
    private float gravity = -9.81f;
    private float distance;

    private Vector2 startPos;
    private Vector2 velocity;

    private float decayTime = 10;

    private LayerMask playerMask;
    private LayerMask groundMask;

    public static event Action<float> Hit;
    public static event Action<int> HitBounce;

    void Update()
    {
        velocity.y += gravity * Time.deltaTime;
        transform.position += (Vector3)(velocity * Time.deltaTime);
    }

    public void Initializeprojectile(Vector2 target, Vector2 startPos, float speed, string playerDirection, int rangeDmg)
    {
        this.target = target;
        this.startPos = startPos;
        this.T = speed;
        this.playerDirection = playerDirection;
        this.rangeDmg = rangeDmg;

        Vector2 toTarget = target - startPos;

        distance = Mathf.Abs(target.x - startPos.x) + Mathf.Abs(target.y - startPos.y);
        T = T * distance / 10;

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
    void ApplyDmg(float dmg)
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
