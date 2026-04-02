using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EJump : MonoBehaviour
{
    [SerializeField] float JumpPower = 5f;
    [SerializeField] float cooldownTime = 3f;

    private bool cooldown = false;
    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("World") && cooldown is false)
        {
            transform.parent.GetComponent<Rigidbody2D>().velocity = new Vector2(transform.parent.GetComponent<Rigidbody2D>().velocity.x, JumpPower);
            StartCoroutine(Cooldown());
        }
    }
    IEnumerator Cooldown()
    {
        cooldown = true;
        yield return new WaitForSeconds(cooldownTime);
        cooldown = false;
    }
}
