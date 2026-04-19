using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BossSpawner : MonoBehaviour
{
    private Transform MaxPos;
    private Transform MinPos;

    private GameObject Player;
    private LayerMask playerMask;

    public string state = "inactive";

    [SerializeField] int Ecount;
    [SerializeField] float Ecooldown;
    [SerializeField] GameObject boss;

    void Start()
    {
        MaxPos = transform.Find("MaxPos");
        MinPos = transform.Find("MinPos");

        playerMask = LayerMask.GetMask("PlayerMask");
        Player = System.Array.Find(FindObjectsOfType<GameObject>(), o => ((1 << o.layer) & playerMask) != 0);
    }

    void Update()
    {
        if (Player.transform.position.x >= MinPos.position.x && Player.transform.position.x <= MaxPos.position.x && Player.transform.position.y >= MinPos.position.y && Player.transform.position.y <= MaxPos.position.y && state == "inactive")
        {
            state = "active";
            foreach (int i in Enumerable.Range(0, Ecount - (transform.childCount - 2)))
            {
                StartCoroutine(Create(i));
            }
        }
    }

    IEnumerator Create(int n)
    {
        yield return new WaitForSeconds(n * Ecooldown);

        GameObject Clone = Instantiate(boss, transform.position, transform.rotation, transform);
    }
}