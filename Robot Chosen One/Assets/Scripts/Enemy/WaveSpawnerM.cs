using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WaveSpawnerM : MonoBehaviour
{
    private Transform MaxPos;
    private Transform MinPos;

    private GameObject Player;
    private LayerMask playerMask;

    public string state = "inactive";
    private int CurrentWave = 1;
    [SerializeField] int WaveAmount = 1;

    [SerializeField] GameObject[] EWave1;
    [SerializeField] GameObject[] EWave2;
    [SerializeField] GameObject[] EWave3;
    [SerializeField] GameObject[] EWave4;
    [SerializeField] GameObject[] EWave5;
    [SerializeField] Vector3[] PWave1;
    [SerializeField] Vector3[] PWave2;
    [SerializeField] Vector3[] PWave3;
    [SerializeField] Vector3[] PWave4;
    [SerializeField] Vector3[] PWave5;

    private GameObject[] CEWave;
    private Vector3[] CPWave;

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
        }
        else if (Player.transform.position.x < MinPos.position.x || Player.transform.position.x > MaxPos.position.x || Player.transform.position.y < MinPos.position.y || Player.transform.position.y > MaxPos.position.y)
        {
            if (state == "active")
            {
                state = "inactive";
                CurrentWave = 1;
                foreach (int k in Enumerable.Range(0, transform.childCount - 2))
                {
                    Destroy(transform.GetChild(k + 2).gameObject);
                }
            }
        }
        else if (state == "active")
        {
            foreach (int wave in Enumerable.Range(0, WaveAmount))
            {
                if (wave + 1 == CurrentWave && transform.childCount == 2)
                {
                    Debug.Log("next wave");
                    LocateList();
                    foreach (int i in Enumerable.Range(0, CEWave.Length))
                    {
                        Create(i);
                    }

                    CurrentWave += 1;
                }
                else if (CurrentWave == WaveAmount+1 && transform.childCount == 2)
                {
                    state = "done";
                }

            }
        }
    }

    void LocateList()
    {
        if (CurrentWave == 1)
        {
            CEWave = EWave1;
            CPWave = PWave1;
        }
        else if (CurrentWave == 2)
        {
            CEWave = EWave2;
            CPWave = PWave2;
        }
        else if (CurrentWave == 3)
        {
            CEWave = EWave3;
            CPWave = PWave3;
        }
        else if (CurrentWave == 4)
        {
            CEWave = EWave4;
            CPWave = PWave4;
        }
        else if (CurrentWave == 5)
        {
            CEWave = EWave5;
            CPWave = PWave5;
        }
    }

    void Create(int i)
    {
        GameObject Clone = Instantiate(CEWave[i], transform.position + CPWave[i], transform.rotation, transform);

        PathFinder EPathFinder = Clone.GetComponent<PathFinder>();
        EPathFinder.InitializePathFinder(Player.transform, MaxPos, MinPos);

        BasicAttackPatern BAP = Clone.GetComponent<BasicAttackPatern>();
        BAP.InitializeBAP(Player);

        TouchDmg TD = Clone.GetComponent<TouchDmg>();
        TD.InitializeTD(Player);
    }
}
