using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class SpawnLocation : MonoBehaviour
{

    [SerializeField] CapsuleCollider2D player;
    [SerializeField] CircleCollider2D respawnCollider;
    [SerializeField] Transform spawnLocation;
    public static event Action<Transform> newLocation;

    private bool inRange;

    void Update()
    {
        if (respawnCollider.IsTouching(player) && Keyboard.current[Key.W].wasPressedThisFrame)
        {
            ChangeLocation(spawnLocation);
        }
    }

    public void ChangeLocation(Transform spawnLocation)
    {
        newLocation?.Invoke(spawnLocation);
    }
}
