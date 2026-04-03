using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorBehaviour : MonoBehaviour
{
    private Vector3 doorClosedPos;
    private Vector3 doorOpenUpPos;
    private Vector3 doorOpenDownPos;
    private float doorHeight;
    public float doorSpeed = 5f;
    public bool doorOpenDownward;
    public bool doorOpenUpward;
    public bool isDoorOpen = false;
    public bool doorIsClosed;
    public bool doorIsOpen;


    void Awake()
    {
        doorHeight = GetComponent<CompositeCollider2D>().bounds.size.y;
        doorClosedPos = transform.position;
        doorOpenUpPos = new Vector3(transform.position.x, transform.position.y + doorHeight - 0.75f, transform.position.z);
        doorOpenDownPos = new Vector3(transform.position.x, transform.position.y - doorHeight + 0.75f, transform.position.z);
    }


    void FixedUpdate()
    {
        if (isDoorOpen)
        {
            OpenDoor();
        }
        else if (!isDoorOpen)
        {
            CloseDoor();
        }

        if ((transform.position == doorOpenUpPos) || (transform.position == doorOpenDownPos))
        {
            doorIsClosed = false;
            doorIsOpen = true;
        }
        else if (transform.position == doorClosedPos)
        {
            doorIsClosed = true;
            doorIsOpen = false;
        }
        else
        {
            doorIsClosed = false;
            doorIsOpen = false;
        }
    }


    void OpenDoor()
    {
        if ((transform.position != doorOpenUpPos) && doorOpenUpward)
        {
            transform.position = Vector3.MoveTowards(transform.position, doorOpenUpPos, doorSpeed * Time.deltaTime);
        }
        else if ((transform.position != doorOpenDownPos) && doorOpenDownward)
        {
            transform.position = Vector3.MoveTowards(transform.position, doorOpenDownPos, doorSpeed * Time.deltaTime);
        }
    }


    void CloseDoor()
    {
        if (transform.position != doorClosedPos)
        {
            transform.position = Vector3.MoveTowards(transform.position, doorClosedPos, doorSpeed * Time.deltaTime);
        }
    }
}
