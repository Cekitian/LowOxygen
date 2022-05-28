using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class PauseManager : MonoBehaviour
{
    public static PauseManager Instance;

    [SerializeField] private TextMeshProUGUI canisters;
    [SerializeField] private TextMeshProUGUI timePlayed;

    private CanvasGroup canvasGroup;
    
    private bool paused = false;
    private float timeScale;
    private int timeSinceUpdate;
    private void Awake()
    {
        Instance = this;

        canvasGroup = gameObject.GetComponent<CanvasGroup>();
        timeSinceUpdate = (int)Time.time;
        timeScale = Time.timeScale;
    }
    private void Update()
    {
        CheckForChange();
    }
    public void ReturnToMainMenu()
    {
        Time.timeScale = timeScale;
        GameDataManager.Instance.SaveData();

        SceneManager.LoadScene(0);
    }
    public void UpdateTimePlayed()
    {
        GameDataManager.Instance.DATA.timePassed += (int)Time.time - timeSinceUpdate;
        timeSinceUpdate = (int)Time.time;
    }
    private void CheckForChange()
    {
        if (Input.GetKeyDown(PlayerInputKeys.pause))
        {
            paused = !paused;

            if (paused)
            {
                timeScale = Time.timeScale;
                Time.timeScale = 0;
            }
            else
            {
                Time.timeScale = timeScale;
            }
            ChangeCanvas();
        }
    }
    private void ChangeCanvas()
    {
        UpdateTimePlayed();
        UpdateStats();

        if(paused)
        {
            canvasGroup.alpha = 1;
            canvasGroup.interactable = true;
        }
        else
        {
            canvasGroup.alpha = 0;
            canvasGroup.interactable = false;
        }
    }
    private void UpdateStats()
    {
        canisters.text = GameDataManager.Instance.DATA.canistersCollected.ToString();

        //time calculations
        int totalTimeInSeconds = GameDataManager.Instance.DATA.timePassed;
        int hours = totalTimeInSeconds / 3600;
        int minutes = (totalTimeInSeconds % 3600) / 60 % 60;
        int seconds = totalTimeInSeconds % 60;
        //time calculations
        timePlayed.text = "";
        if (hours < 10)
            timePlayed.text = "0" + hours.ToString();
        else
            timePlayed.text = hours.ToString();
        if (minutes < 10)
            timePlayed.text += " : " + "0" + minutes.ToString();
        else
            timePlayed.text += " : " + minutes.ToString();
        if (seconds < 10)
            timePlayed.text += " : " + "0" + seconds.ToString();
        else
            timePlayed.text += " : " + seconds.ToString();


    }
}
