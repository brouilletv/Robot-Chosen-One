using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleDoorColliderInteract : MonoBehaviour
{
    public bool canOpenDoorFromOtherSide = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            canOpenDoorFromOtherSide = true;
        }
    }
}
