using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch_SpaceZone : Switch
{
    [SerializeField] private bool isInSpace;
    [SerializeField] private Color oxyColor;
    [SerializeField] private Color noOxyColor;
    [SerializeField] private ParticleSystem bubbleParticles;
    private SpriteRenderer spaceRenderer;
    private Player playerRef;

    private List<Oxygen_State> thingsInside = new List<Oxygen_State>();
    protected override void Init()
    {
        spaceRenderer = gameObject.GetComponent<SpriteRenderer>();

        if (currentState == true)//no O2
        {
            bubbleParticles.Play();
            spaceRenderer.color = noOxyColor;
        }
        else // O2
        {
            bubbleParticles.Stop();
            spaceRenderer.color = oxyColor;
        }
    }
    protected override void DoOnStateChange()
    {
        foreach(Oxygen_State x in thingsInside)
        {
            if (currentState == true)//no O2
            {
                x.ChangeState(Oxygen_State.OXYGEN.NO_OXYGEN);
                spaceRenderer.color = noOxyColor;
            }
            else // O2
            {
                x.ChangeState(Oxygen_State.OXYGEN.OXYGEN);
                spaceRenderer.color = oxyColor;
            }
        }

        if (currentState == true)//no O2
        {
            bubbleParticles.Play();
            spaceRenderer.color = noOxyColor;
        }
        else // O2
        {
            bubbleParticles.Stop();
            spaceRenderer.color = oxyColor;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent(out Oxygen_State oxy) )
        {
            if(currentState == true)//no O2
            {
                oxy.ChangeState(Oxygen_State.OXYGEN.NO_OXYGEN);
            }
            else // O2
            {
                oxy.ChangeState(Oxygen_State.OXYGEN.OXYGEN);
            }
            thingsInside.Add(oxy);

            if (isInSpace)
            {
                if (collision.CompareTag("Player"))
                {
                    AudioManager.Instance.IsInSpace(true);
                }
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Oxygen_State oxy))
        {
            oxy.ChangeState(Oxygen_State.OXYGEN.OXYGEN);
            thingsInside.Remove(oxy);

            if (isInSpace)
            {
                if (collision.CompareTag("Player"))
                {
                    AudioManager.Instance.IsInSpace(false);
                }
            }
        }
    }


}
