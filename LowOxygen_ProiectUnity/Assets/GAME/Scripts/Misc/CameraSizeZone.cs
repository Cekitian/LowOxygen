using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSizeZone : MonoBehaviour
{
    [SerializeField] private bool oneTime;
    [Space]
    [SerializeField] private float newSize;
    [SerializeField] private float timeToEnter;
    [SerializeField] private float timeToExit;
    private CameraManager cameraRef;

    private void Start()
    {
        cameraRef = CameraManager.Instance;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        cameraRef.ChangeCameraSize(newSize,timeToEnter);
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            cameraRef.ChangeCameraSize(cameraRef.initSize, timeToExit);
            if (oneTime)
                Destroy(gameObject);
        }
    }
}
