using UnityEngine;

public class FollowPlayer : MonoBehaviour
{

    private Transform CameraT;
    public Transform RobotT;

    public float damping;
    private Vector3 vel = Vector3.zero;

    void Start()
    {
        CameraT = GetComponent<Transform>();
    }

    void Update()
    {
        Vector3 RobotP = RobotT.position;
        RobotP.z = CameraT.position.z;
        CameraT.position = Vector3.SmoothDamp(CameraT.position, RobotP, ref vel, damping);
    }
}
