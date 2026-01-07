using UnityEngine;
using System.Collections;
using System;

public class TouchDmg : MonoBehaviour
{
    private bool OnCooldown = false;
    [SerializeField] GameObject Player;

    public static event Action<int> Hit;
    [SerializeField] int touchDmg = 1;

    [SerializeField] private Transform Body;
    [SerializeField] private Transform PlayerT;

    public static event Action<int> HitBouce;
    private int TouchedOnRight;
    void OnCollisionStay2D(Collision2D col)
    {
        if (col.gameObject.name == Player.name && OnCooldown == false)
        {
            StartCoroutine(Cooldown());

            TakeDmg(touchDmg);

            SideTouched();
        }

        IEnumerator Cooldown()
        {
            OnCooldown = true;
            yield return new WaitForSeconds(2f);
            OnCooldown = false;
        }
    }

    public void TakeDmg(int dmg)
    {
        Hit?.Invoke(dmg);
    }

    void SideTouched()
    {
        float PlayerX = PlayerT.position.x;
        float BodyX = Body.position.x;

        if (BodyX > PlayerX)
        {
            TouchedOnRight = 0;
        }
        else if (BodyX < PlayerX)
        {
            TouchedOnRight = 1;
        }

        Bouce(TouchedOnRight);
    }
    public void Bouce(int side)
    {
        HitBouce?.Invoke(side);
    }
}
