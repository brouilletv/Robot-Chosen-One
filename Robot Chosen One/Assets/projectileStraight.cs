using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class projectileStraight : MonoBehaviour
{
    private Vector3 target;
    private Vector3 startPos;
    private float speed;

    private Vector3 nextPos;

    private float decayTime = 10;

    void Update()
    {
        nextPos.x = (target.x - startPos.x)/Mathf.Abs(target.x - startPos.x);
        transform.position += nextPos * speed * Time.deltaTime;
    }

    public void Initializeprojectile(Vector2 target, Vector2 startPos, float speed)
    {
        this.target = target;
        this.startPos = startPos;
        this.speed = speed;

        StartCoroutine(DestroyCo());
    }

    IEnumerator DestroyCo()
    {
        yield return new WaitForSeconds(decayTime);
        Destroy(gameObject);
    }
}
