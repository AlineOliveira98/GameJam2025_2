using UnityEngine;
using Unity.Cinemachine;

public class Room : MonoBehaviour
{
    public GameObject VirtualCam;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player") && !other.isTrigger)
        {
            VirtualCam.GetComponent<CinemachineCamera>().Priority = 10;
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if(other.CompareTag("Player") && !other.isTrigger)
        {
            VirtualCam.GetComponent<CinemachineCamera>().Priority = 0;
        }
    }
}
