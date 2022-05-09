using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant_Grow : Plant
{
    [SerializeField] private AudioClip growSound;
    [SerializeField] private GameObject vine;
    [SerializeField] private BoxCollider2D groundCollider;
    private Oxygen_State oState;

    private float growSpeed = 3;

    private bool noOxygen;
    private bool canGrow = true;

    private float timeForPitchChange;
    private float growPitch = 0.25f;
    private void Awake()
    {
        base.Awake();
        oState = gameObject.GetComponent<Oxygen_State>();
        oState.ChangedOxygenState += OnChangedOxy;
    }
    private void Update()
    {
        if(!noOxygen && vine.transform.localPosition.y > 0)
        {
            vine.transform.localScale -= Vector3.up * Time.deltaTime * growSpeed;
            vine.transform.localPosition -= Vector3.up * Time.deltaTime * growSpeed / 2;

            groundCollider.size -= Vector2.up * Time.deltaTime * growSpeed;
            groundCollider.offset -= Vector2.up * Time.deltaTime * growSpeed / 2;

            if (vine.transform.localPosition.y < 0)
            {
                vine.transform.localScale = Vector3.one;
                vine.transform.localPosition = Vector3.zero;

                groundCollider.size = Vector2.one;
                groundCollider.offset = Vector2.zero;
            }

          
        }
        else if(noOxygen && canGrow && grounded)
        {
            vine.transform.localScale += Vector3.up * Time.deltaTime * growSpeed;
            vine.transform.localPosition += Vector3.up * Time.deltaTime  * growSpeed / 2;

            groundCollider.size += Vector2.up * Time.deltaTime * growSpeed;
            groundCollider.offset += Vector2.up * Time.deltaTime * growSpeed / 2;
        }
       

    }
    protected void FixedUpdate()
    {
        base.FixedUpdate();

        timeForPitchChange += Time.fixedDeltaTime;

        if (!noOxygen && vine.transform.localPosition.y > 0)
        {
            if (timeForPitchChange >= 0.25f)
            {
                timeForPitchChange = 0;
                growPitch -= 0.025f;
                AudioManager.Instance.PlaySound(growSound, 0.05f, growPitch, false);
            }
        }
        else if (noOxygen && canGrow && grounded)
        {
            if (timeForPitchChange >= 0.25f)
            {
                timeForPitchChange = 0;
                growPitch += 0.025f;
                AudioManager.Instance.PlaySound(growSound, 0.05f, growPitch, false);
            }
        }



    }
    public void OnChangedOxy()
    {
        if (oState.oxygenState == Oxygen_State.OXYGEN.NO_OXYGEN)
            noOxygen = true;
        else
        {
            noOxygen = false;
        }
            
    }

    public void ChangeGrowState(bool newGrowState)
    {
        canGrow = newGrowState;
    }
    public void ResetGrowState()
    {
        vine.transform.localScale = Vector3.one;
        vine.transform.localPosition = Vector3.zero;

        groundCollider.size = Vector2.one;
        groundCollider.offset = Vector2.zero;

        growPitch = 0.25f;

    }
}


