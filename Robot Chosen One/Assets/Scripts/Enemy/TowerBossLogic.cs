using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerBossLogic : MonoBehaviour
{
    [SerializeField] int MaxHealth = 50;
    private int Health;

    private GameObject Player;
    private PlayerMovement PM;
    private HealthHeartBarV2 HHB;

    public void InitializeBossLogic(GameObject Player)
    {
        this.Player = Player;

        Health = MaxHealth;
        PM = Player.GetComponent<PlayerMovement>();
        HHB = Player.transform.Find("GUI").Find("HealthHeart").GetComponent<HealthHeartBarV2>();
    }

    /*
     * screen corrupter
     * inverse mouvement
     * inervse camera color
     * tp slash
     */

    private void Update()
    {
        
    }

    public void TakeDamage(int amount)
    {
        Health -= amount;
        if (Health <= 0)
        {
            Destroy(transform.parent.gameObject);
            PM.defeatedJunkyardBoss = true;
        }
    }

    IEnumerator Slash()
    {
        transform.position = Player.transform.position;
        yield return new WaitForSeconds(2f);
    }
}
