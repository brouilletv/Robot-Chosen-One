using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;

public class BossSpawner : MonoBehaviour
{
    private Transform MaxPos;
    private Transform MinPos;

    private GameObject Player;
    private LayerMask playerMask;
    private PlayerMovement PM;

    public string state = "inactive";
    private int bossNum = 0;

    [SerializeField] int Ecount;
    [SerializeField] float Ecooldown;
    [SerializeField] GameObject boss;

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        MaxPos = transform.Find("Max");
        MinPos = transform.Find("Min");

        playerMask = LayerMask.GetMask("PlayerMask");
        Player = GameObject.FindWithTag("Player");
        PM = Player.GetComponent<PlayerMovement>();
        TagCheck();
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }


    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
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
                if (bossNum == 2)
                {
                    StartCoroutine(Create(0));
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
        else if (boss.CompareTag("Boss2"))
        {
            state = "inactive";
            bossNum = 2;
            foreach (int i in Enumerable.Range(0, Ecount - (transform.childCount - 2)))
            {
                StartCoroutine(Create(i));
            }
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
            BDT.InitializeBodyDmg(Player);
        }
        else if (bossNum == 2)
        {
            PowerBoxLogic PBL1 = Clone.transform.Find("Power Box 1").GetComponent<PowerBoxLogic>();
            PowerBoxLogic PBL2 = Clone.transform.Find("Power Box 2").GetComponent<PowerBoxLogic>();
            PowerBoxLogic PBL3 = Clone.transform.Find("Power Box 3").GetComponent<PowerBoxLogic>();
            PowerBoxLogic PBL4 = Clone.transform.Find("Power Box 4").GetComponent<PowerBoxLogic>();
            MineBossLogic MBL = Clone.GetComponent<MineBossLogic>();

            if (PM.defeatedMinesBoss is false)
            {
                PBL1.InitializePowerBox(Player, true);
                PBL2.InitializePowerBox(Player, true);
                PBL3.InitializePowerBox(Player, true);
                PBL4.InitializePowerBox(Player, true);

                MBL.InitializeMineBoss(Player, PBL1, PBL2, PBL3, PBL4, MinPos, MaxPos, true);
            }
            else
            {
                PBL1.InitializePowerBox(Player, false);
                PBL2.InitializePowerBox(Player, false);
                PBL3.InitializePowerBox(Player, false);
                PBL4.InitializePowerBox(Player, false);

                MBL.InitializeMineBoss(Player, PBL1, PBL2, PBL3, PBL4, MinPos, MaxPos, false);
            }
        }
    }
}