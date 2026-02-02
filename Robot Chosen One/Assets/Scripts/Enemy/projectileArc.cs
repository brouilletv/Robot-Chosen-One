using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class projectileArc : MonoBehaviour
{

    [SerializeField] GameObject projectile;
    [SerializeField] Transform projectileT;
    [SerializeField] Transform playerT;

    private Vector2 startPos;
    private Vector2 targetPos;
    [SerializeField] float speed = 5f;
    [SerializeField] float arcHeight = 2f;

    private float distance;
    private float nextX;
    private Vector2 nextPos;

    void Start()
    {
        startPos = projectileT.position;
        targetPos = projectileT.position;
        distance = targetPos.x - startPos.x;
    }

    /*
    void Update()
    {
        nextX = Mathf.MoveTowards(projectileT.position.x, targetPos.x, speed * Time.deltaTime);

        float baseY = Mathf.Lerp(startPos.y, targetPos.y, (nextX - startPos.x) / distance);
        float arc = arcHeight * (nextX - startPos.x) * (nextX - targetPos.x) / (-0.25f * distance * distance);
        nextPos = new Vector2(nextX, baseY + arc);

        projectileT.position = nextPos;

        if (nextPos == targetPos)
        {
            Destroy(gameObject);
        }
    }
    */
}
