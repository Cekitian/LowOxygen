using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Player_Dialogue : MonoBehaviour
{
   [SerializeField] private AudioClip[] voiceClip;
   [SerializeField] private TextMeshPro textBox;

    private string textDisplayed = "";
    private int lengthDisplayed = 0;

    private float timeToDisplay = 1;
    private float timeBetweenLetters = 0.025f;

    private float duration;
    private int letterCount = 0;
    private void Awake()
    {
        textBox.outlineWidth = 5;
        textBox.outlineColor = Color.black;
    }
    private void FixedUpdate()
    {
        if(textDisplayed != "")
        {
            if(lengthDisplayed <= textDisplayed.Length && Time.time >= timeToDisplay)
            {
                textBox.text = textDisplayed.Substring(0,lengthDisplayed);

                timeToDisplay = Time.time + timeBetweenLetters;

                if(textDisplayed.Substring(0,lengthDisplayed).EndsWith(","))
                {
                    timeToDisplay += 0.2f;
                }
                else if(textDisplayed.Substring(0, lengthDisplayed).EndsWith("."))
                {
                    timeToDisplay += 0.5f;
                }
                lengthDisplayed++;
                letterCount++;
                if(letterCount == 3)
                {
                    letterCount = 0;
                    AudioManager.Instance.PlaySound(voiceClip[Random.Range(0, voiceClip.Length)], 0.05f, 1.3f, false);
                }
            }
            else if(Time.time >= duration && Time.time >= timeToDisplay)
            {
                //timeToDisplay = Time.time + timeBetweenLetters / 2;

                if (textBox.text.Length < 2)
                {
                    textBox.text = "";
                }
                else
                textBox.text = textBox.text.Substring(1, textBox.text.Length - 2);
            }
        }
    }
    public void DisplayDialogue(string text, float newDuration)
    {
        textDisplayed = text;
        timeToDisplay = 0;
        lengthDisplayed = 0;
        duration = Time.time + newDuration;
    }
}
