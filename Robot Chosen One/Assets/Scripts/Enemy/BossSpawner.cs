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
    private PlayerMovement PM;

    public string state = "none";
    private int bossNum = 0;

    [SerializeField] int Ecount;
    [SerializeField] float Ecooldown;
    [SerializeField] GameObject boss;

    void Start()
    {
        MaxPos = transform.Find("MaxPos");
        MinPos = transform.Find("MinPos");

        playerMask = LayerMask.GetMask("PlayerMask");
        Player = System.Array.Find(FindObjectsOfType<GameObject>(), o => ((1 << o.layer) & playerMask) != 0);
        PM = Player.GetComponent<PlayerMovement>();
        TagCheck();
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
        else if (Player.transform.position.x < MinPos.position.x || Player.transform.position.x > MaxPos.position.x || Player.transform.position.y < MinPos.position.y || Player.transform.position.y > MaxPos.position.y)
        {
            if (state == "active")
            {
                state = "inactive";
                foreach (int k in Enumerable.Range(0, transform.childCount - 2))
                {
                    Destroy(transform.GetChild(k + 2).gameObject);
                }
            }
        }
    }
    void TagCheck()
    {
        if (boss.CompareTag("Boss1") && PM.defeatedJunkyardBoss is false)
        {
            state = "inactive";
            bossNum = 1;
        }
        else if (boss.CompareTag("Boss2") && PM.defeatedMinesBoss is false)
        {
            state = "inactive";
            bossNum = 2;
        }
        else if (boss.CompareTag("Boss3") && PM.defeatedTowerBoss is false)
        {
            state = "inactive";
            bossNum = 3;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    IEnumerator Create(int n)
    {
        yield return new WaitForSeconds(n * Ecooldown);

        GameObject Clone = Instantiate(boss, transform.position, transform.rotation, transform);

        if (bossNum == 1)
        {
            JunkyardBossLogic JBL = Clone.GetComponent<JunkyardBossLogic>();
            JBL.InitializeBossLogic(Player, MinPos, MaxPos);

            BodyDmg BDT = Clone.transform.Find("Top").GetComponent<BodyDmg>();
            BodyDmg BDB = Clone.transform.Find("Bottom").GetComponent<BodyDmg>();
            BDT.InitializeBodyDmg(Player);
            BDB.InitializeBodyDmg(Player);
        }
        else if (bossNum == 2)
        {
            MineBossLogic MBL = Clone.GetComponent<MineBossLogic>();

            PowerBoxLogic PBL1 = Clone.transform.Find("Power Box 1").GetComponent<PowerBoxLogic>();
            PBL1.InitializePowerBox(Player);
            PowerBoxLogic PBL2 = Clone.transform.Find("Power Box 2").GetComponent<PowerBoxLogic>();
            PBL2.InitializePowerBox(Player);
            PowerBoxLogic PBL3 = Clone.transform.Find("Power Box 3").GetComponent<PowerBoxLogic>();
            PBL3.InitializePowerBox(Player);
            PowerBoxLogic PBL4 = Clone.transform.Find("Power Box 4").GetComponent<PowerBoxLogic>();
            PBL4.InitializePowerBox(Player);

        }
    }
}