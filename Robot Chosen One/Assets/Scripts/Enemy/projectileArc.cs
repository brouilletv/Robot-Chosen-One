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
    private Vector3 targetPos;
    [SerializeField] float speed = 5f;
    [SerializeField] float arcHeight = 2f;

    private float distance;
    private float nextX;
    private Vector3 nextPos;

    void Start()
    {
        startPos = projectileT.position;
        targetPos = projectileT.position;
        distance = targetPos.x - startPos.x;
    }

 
    void Update()
    {
        nextX = 0.01f;

        float baseY = 0;
        float arc = 0;
        nextPos = new Vector3(nextX, baseY + arc, 0);

        projectileT.position += nextPos;

        if (nextPos == targetPos)
        {
            Destroy(gameObject);
        }
    }
}
