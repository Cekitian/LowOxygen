using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class AirlockZone : MonoBehaviour
{
    [SerializeField] private bool enterFromLeft;
    [SerializeField] private ParticleSystem oxygenParticles;
    [SerializeField] private Switch rightDoor;
    private bool activatedAirlock = false;
    private bool activatedDoor = false;

    private float openDoorTime;
    private float timeInside = 7f;

    private Volume localVolume;
    private float volumeStrength = 0;
    private void Awake()
    {
        localVolume = gameObject.GetComponentInChildren<Volume>();
    }
    private void FixedUpdate()
    {
        if (Time.time >= openDoorTime && activatedAirlock && !activatedDoor)
        {            
            activatedDoor = true;
            rightDoor.ChangeState(true);

        }
            UpdateVolumeStrength();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player") && !activatedAirlock)
        {
            ActivateAirlock();
            activatedAirlock = true;
            openDoorTime = Time.time + timeInside;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            activatedDoor = false;
            activatedAirlock = false;
        }
    }
    private void UpdateVolumeStrength()
    {
        if(openDoorTime >= Time.time)
        {
            if (openDoorTime - Time.time <= 2)
            {
                volumeStrength -= Time.fixedDeltaTime;
            }
            else
            {
                volumeStrength += Time.fixedDeltaTime / (timeInside - 2);
            }
            localVolume.weight = volumeStrength;
        }
       
    }
    private void ActivateAirlock()
    {
        oxygenParticles.Play();
    }
}
