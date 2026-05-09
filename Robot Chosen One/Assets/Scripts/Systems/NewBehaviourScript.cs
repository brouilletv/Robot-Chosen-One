using UnityEngine;
using UnityEngine.SceneManagement;

public class ColliderArenaDoorObjectOneTimeInteract : MonoBehaviour
{
    [SerializeField] GameObject arenaDoorObject;
    private DoorBehaviour doorBehaviour;

    private bool canOpenDoor = true;
    public bool doorOpenDownward;
    public bool doorOpenUpward;


    private void Awake()
    {
        doorBehaviour = arenaDoorObject.transform.GetComponentInChildren<DoorBehaviour>();

        doorBehaviour.doorOpenDownward = doorOpenDownward;
        doorBehaviour.doorOpenUpward = doorOpenUpward;
    }


    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        doorBehaviour.isDoorOpen = !doorBehaviour.isDoorOpen;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (canOpenDoor)
            {
                if (canOpenDoor)
                {
                    canOpenDoor = false;
                    doorBehaviour.isDoorOpen = !doorBehaviour.isDoorOpen;
                }
            }
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }


    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
