using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuCinematicScript : MonoBehaviour
{
    [SerializeField] private GameObject secondPlanet;
    [SerializeField] private AudioClip rocketSound;
    private void Awake()
    {
        if(GameDataManager.Instance.DATA.wonGame)
        {
            secondPlanet.SetActive(true);
        }
    }
    public void RocketShake()
    {
        //AudioManager.Instance.PlaySound(rocketSound, 1f, 0.5f, false);
        CameraManager.Instance.ScreenShake(0.2f, 5f,CameraManager.FadeType.OUT);
    }
}
