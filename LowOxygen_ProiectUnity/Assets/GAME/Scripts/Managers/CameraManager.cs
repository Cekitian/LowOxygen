using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager Instance;

    public float initSize;
    private CinemachineVirtualCamera camComponent;
    private CinemachineFramingTransposer framingTransposer;
    private CinemachineBasicMultiChannelPerlin noiseComponent;

    private float cameraSize;
    private float trackOffset;
    private float timeToUpdate;

    private float shakeIntensity = 0;
    private float shakeStopTime = 0;
    private float shakeDuration = 0;
    private FadeType shakeFade = FadeType.NONE;
    //private bool shakeFade = false;

    public enum FadeType
    {
        NONE = 0, IN = 1, OUT = 2
    }

    private CinemachineCore.Stage stages = new CinemachineCore.Stage();
     private void Awake()
    {
        Instance = this;

        camComponent = gameObject.GetComponent<CinemachineVirtualCamera>();
        if(camComponent != null)
        {
            framingTransposer = camComponent.GetCinemachineComponent<CinemachineFramingTransposer>();
            noiseComponent = camComponent.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
            cameraSize = camComponent.m_Lens.OrthographicSize;
            initSize = cameraSize;
            trackOffset = framingTransposer.m_TrackedObjectOffset.y;
        }
      
    }
    private void FixedUpdate()
    {
        if (camComponent == null)
            return;

        UpdateCameraSize();
        UpdateCameraShake();
    }
    public void ScreenShake(float strength, float duration, FadeType fadeOut)
    {
        shakeIntensity = strength;
        shakeStopTime = Time.time + duration;
        shakeDuration = duration;
        shakeFade = fadeOut;
    }
    public void ChangeTarget(Transform follow, Transform lookAt)
    {
        camComponent.Follow = follow;
        camComponent.LookAt = lookAt;
    }
    public void ChangeCameraSize(float intendedSize, float timeForUpdate)
    {
        trackOffset = (trackOffset * intendedSize) / cameraSize;
        cameraSize = intendedSize;

        timeToUpdate = timeForUpdate;      
    }
    private void UpdateCameraShake()
    {
        if (shakeStopTime < Time.time)
        {
            noiseComponent.m_AmplitudeGain = 0;
            return;
        }

        switch (shakeFade)
        {
            case FadeType.NONE:
                noiseComponent.m_AmplitudeGain = shakeIntensity;
                break;
            case FadeType.IN:
                noiseComponent.m_AmplitudeGain = shakeIntensity * (1 - (shakeStopTime - Time.time) / shakeDuration);
                break;
            case FadeType.OUT:
                noiseComponent.m_AmplitudeGain = shakeIntensity * ((shakeStopTime - Time.time) / shakeDuration);
                break;
        }

    }
    private void UpdateCameraSize()
    {
        if(timeToUpdate == 0)
        {
            camComponent.m_Lens.OrthographicSize = cameraSize;
        }

        if (camComponent.m_Lens.OrthographicSize < cameraSize)
        {
            camComponent.m_Lens.OrthographicSize += Time.fixedDeltaTime / timeToUpdate * cameraSize;

            if (camComponent.m_Lens.OrthographicSize > cameraSize)
                camComponent.m_Lens.OrthographicSize = cameraSize;
        }
        else if (camComponent.m_Lens.OrthographicSize > cameraSize)
        {
            camComponent.m_Lens.OrthographicSize -= Time.fixedDeltaTime / timeToUpdate * cameraSize;

            if (camComponent.m_Lens.OrthographicSize < cameraSize)
                camComponent.m_Lens.OrthographicSize = cameraSize;
        }

        framingTransposer.m_TrackedObjectOffset.y = (trackOffset * camComponent.m_Lens.OrthographicSize) / cameraSize;

    }
}
