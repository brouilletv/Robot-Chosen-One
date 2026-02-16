using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class projectileStraight : MonoBehaviour
{

    [SerializeField] GameObject projectile;
    [SerializeField] Transform projectileT;
    [SerializeField] CircleCollider2D projectileC;
    [SerializeField] Transform playerT;
    [SerializeField] CapsuleCollider2D playerC;

    [SerializeField] int rangeDmg;

    private Vector2 startPos;
    private Vector2 targetPos;
    [SerializeField] float speed = 0.01f;

    private float distance;
    private float nextX;
    private Vector2 nextPos;

    private int playerDirectionInt;

    public static event Action<int> Hit;
    public static event Action<int> HitBounce;

    void Start()
    {

        startPos = projectileT.position;
        targetPos = playerT.position;
        distance = targetPos.x - startPos.x;

        Debug.Log(playerT.position);

        if ((targetPos.x - startPos.x) / Mathf.Abs(targetPos.x - startPos.x) == 1)
        {
            playerDirectionInt = 1;
        }
        else
        {
            playerDirectionInt = 0;
        }
    }

 
    void Update()
    {
        nextX = speed * ((targetPos.x-startPos.x) / Mathf.Abs(targetPos.x - startPos.x));

        nextPos = new Vector2(nextX + projectileT.position.x, projectileT.position.y);

        projectileT.position = nextPos;

        if (projectileC.IsTouching(playerC))
        {
            Debug.Log("work");
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
}
