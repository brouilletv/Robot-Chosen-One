using UnityEngine;
using UnityEngine.SceneManagement;

public class FollowPlayer : MonoBehaviour
{

    private Transform CameraT;
    private Transform player;

    public float damping;
    private Vector3 vel = Vector3.zero;


    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }


    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }


    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        CameraT = GetComponent<Transform>();
        player = GameObject.FindWithTag("Player").transform;
    }


    void Update()
    {
        Vector3 RobotP = player.position;
        RobotP.z = CameraT.position.z;
        CameraT.position = Vector3.SmoothDamp(CameraT.position, RobotP, ref vel, damping);
    }
}
