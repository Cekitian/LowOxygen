using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RocketObject : MonoBehaviour
{
    [SerializeField] private RocketFuel fuelRef;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private TextMeshProUGUI timeToComplete;
    [SerializeField] private TextMeshProUGUI hatsFound;
    [Space]
    [SerializeField] private Animator canvasAnimator;

    private bool startWinAnim = false;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(fuelRef.startedFueling)
        {
            Win();
            startWinAnim = true;
        }
    }
    private void FixedUpdate()
    {
        if (!startWinAnim)
            return;

        if(canvasGroup.alpha < 1)
        {
            canvasGroup.alpha += Time.deltaTime;

            if(canvasGroup.alpha == 1)
            {
                canvasGroup.interactable = true;
            }
        }

    }

    public void ExitToMainMenu()
    {
        if(GameDataManager.Instance.DATA.timePassed <= GameDataManager.Instance.DATA.recordTime ||
           (GameDataManager.Instance.DATA.recordTime == -1 && GameDataManager.Instance.DATA.timePassed != 0))
        {
            GameDataManager.Instance.DATA.recordTime = GameDataManager.Instance.DATA.timePassed;
        }

        ResetStats();
        PauseManager.Instance.ReturnToMainMenu();
    }
    private void ResetStats()
    {
        bool wonGame = GameDataManager.Instance.DATA.wonGame;
        bool gotAllHats = GameDataManager.Instance.DATA.gotAllHats;
        int recordTime = GameDataManager.Instance.DATA.recordTime;
        GameDataManager.Instance.DATA = new GameDataManager.Data();

        if (wonGame)
        { 
            GameDataManager.Instance.DATA.wonGame = true;
            GameDataManager.Instance.DATA.recordTime = recordTime;
        }
        if (gotAllHats)
        {
            GameDataManager.Instance.DATA.gotAllHats = true;
        }

        GameDataManager.Instance.SaveData();
    }
    private void Win()
    {
        GameDataManager.Instance.DATA.wonGame = true;
        GameDataManager.Instance.SaveData();

        //animation;
        canvasAnimator.Play("Outro");

        PauseManager.Instance.UpdateTimePlayed();

        HatCount();

        CalculateTime();
    }
    private void HatCount()
    {
        int hatsObtained = 0;
        foreach (bool x in GameDataManager.Instance.DATA.hasHat)
        {
            if (x)
                hatsObtained++;
        }

        hatsFound.text = hatsObtained + " l " + GameDataManager.Instance.DATA.hasHat.Length;

        if(hatsObtained == GameDataManager.Instance.DATA.hasHat.Length)
        {
            GameDataManager.Instance.DATA.gotAllHats = true;
        }
    }  
    private void CalculateTime()
    {
        int totalTimeInSeconds = GameDataManager.Instance.DATA.timePassed;
        int hours = totalTimeInSeconds / 3600;
        int minutes = (totalTimeInSeconds % 3600) / 60 % 60;
        int seconds = totalTimeInSeconds % 60;
        //time calculations
        timeToComplete.text = "";
        if (hours < 10)
            timeToComplete.text = "0" + hours.ToString();
        else
            timeToComplete.text = hours.ToString();
        if (minutes < 10)
            timeToComplete.text += " : " + "0" + minutes.ToString();
        else
            timeToComplete.text += " : " + minutes.ToString();
        if (seconds < 10)
            timeToComplete.text += " : " + "0" + seconds.ToString();
        else
            timeToComplete.text += " : " + seconds.ToString();

    }

}
