using UnityEngine;
using UnityEngine.UI; 

public class DashBar : MonoBehaviour
{
    private GameObject player;
    private PlayerMovement playerMovement;
    private Slider slider;

    private void Awake()
    {
        player = GameObject.FindWithTag("Player");
        playerMovement = player.GetComponent<PlayerMovement>();
        slider = GetComponent<Slider>();
    }

    private void Update()
    {
        slider.value = playerMovement.DashCooldownNormalized;
    }
}