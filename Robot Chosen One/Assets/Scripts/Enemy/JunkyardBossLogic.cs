using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JunkyardBossLogic : MonoBehaviour
{
    #region Variables & Initialize
    [Header("Boss Heath & Phases")]
    [SerializeField] float BossMaxHealth = 30f;
    private float BossHealth;
    private bool BossHeathLossCooldown;
    private float BossHeathLossCooldownTime = 1f;

    [SerializeField] float[] BossPhaseTrigger = {15f, 0f};
    private int BossPhase = 1;
    public void InitializeBossLogic()
    {
        BossHealth = BossMaxHealth;
    }
    #endregion

    #region Update
    void Update()
    {
        
    }
    #endregion

    #region Health & Phases
    public void TakeDamage(float amount)
    {
        if (BossHeathLossCooldown == false)
        {
            BossHealth -= amount;
            Debug.Log(BossHealth);
            if(BossHealth <= BossPhaseTrigger[1])
            {
                BossPhase = 3;
                Debug.Log(BossPhase);
                Destroy(transform.parent.gameObject);
            }
            else if (BossHealth <= BossPhaseTrigger[0])
            {
                BossPhase = 2;
                Debug.Log(BossPhase);
            }
            StartCoroutine(Cooldown());
        }
    }

    IEnumerator Cooldown()
    {
        BossHeathLossCooldown = true;
        yield return new WaitForSeconds(BossHeathLossCooldownTime);
        BossHeathLossCooldown = false;
    }
    #endregion

    #region Attacks
    void Rush()
    {

    }

    void Shockwave()
    {

    }
    #endregion
}
