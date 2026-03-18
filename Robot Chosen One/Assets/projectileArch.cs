using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class projectileArch : MonoBehaviour
{
    private Vector3 target;
    private Vector3 startPos;
    private float speed;
    private string playerDirection;
    private int rangeDmg;

    private int playerDirectionInt;

    private Vector3 nextPos;

    private float decayTime = 10;

    private LayerMask playerMask;

    public static event Action<int> Hit;
    public static event Action<int> HitBounce;

    void Update()
    {
        nextPos.x = (target.x - startPos.x) / Mathf.Abs(target.x - startPos.x);
        transform.position += nextPos * speed * Time.deltaTime;
    }

    public void Initializeprojectile(Vector2 target, Vector2 startPos, float speed, string playerDirection, int rangeDmg)
    {
        this.target = target;
        this.startPos = startPos;
        this.speed = speed;
        this.playerDirection = playerDirection;
        this.rangeDmg = rangeDmg;

        StartCoroutine(DestroyCo());

        playerMask = LayerMask.GetMask("PlayerMask");
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
    }

    void ApplyDmg(int dmg)
    {
        Hit?.Invoke(dmg);
    }

    public void Bouce(int side)
    {
        HitBounce?.Invoke(side);
    }

    IEnumerator DestroyCo()
    {
        yield return new WaitForSeconds(decayTime);
        Destroy(gameObject);
    }
}
