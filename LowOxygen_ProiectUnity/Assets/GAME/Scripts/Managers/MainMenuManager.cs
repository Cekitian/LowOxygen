using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private AudioClip buttonSound;
    [SerializeField] private GameObject turboButton;
    [Space]
    [SerializeField] private TMPro.TextMeshProUGUI recordTime;
    [SerializeField] private TMPro.TextMeshProUGUI escapedIn;
    [Space]
    [SerializeField] private Image blackScreen;
    [SerializeField] private Button continueButton;
    [SerializeField] private Animator introAnimator;
    [SerializeField] private CanvasGroup menuGroup;
    [SerializeField] private CanvasGroup aboutGroup;



    private void Awake()
    {
        StartCoroutine(FadeFromBlack());

        if(GameDataManager.Instance.DATA.currentCheckPoint == new GameDataManager.Data().currentCheckPoint)
        {
            continueButton.interactable = false;
        }

        if(GameDataManager.Instance.DATA.recordTime != -1)
        {
            DisplayRecordTime();
        }

        if (!GameDataManager.Instance.DATA.gotAllHats)
            turboButton.SetActive(false);
    }
    public void PlayGame()
    {
        PlayButtonSound();

        StopAllCoroutines();
        StartCoroutine(IntroCutscene());
        ResetStats();
    }
    public void ContinueGame()
    {
        PlayButtonSound();

        StopAllCoroutines();
        StartCoroutine(LoadNextScene());
    }
    public void About()
    {
        PlayButtonSound();

        // open about screen

        if (aboutGroup.alpha == 0)
        {
            aboutGroup.alpha = 1;
            aboutGroup.blocksRaycasts = true;
            aboutGroup.interactable = true;

            menuGroup.alpha = 0;
            menuGroup.blocksRaycasts = false;
            menuGroup.interactable = false;
        }
        else
        {
            aboutGroup.alpha = 0;
            aboutGroup.blocksRaycasts = false;
            aboutGroup.interactable = false;

            menuGroup.alpha = 1;
            menuGroup.blocksRaycasts = true;
            menuGroup.interactable = true;
        }
    }
    public void QuitGame()
    {
        PlayButtonSound();

        Application.Quit();
    }
    public void TurboMode()
    {
        TurboManager.Instance.ChangeTurboState();
    }
    private void ResetStats()
    {
        bool wonGame = GameDataManager.Instance.DATA.wonGame;
        bool gotAllHats = GameDataManager.Instance.DATA.gotAllHats;
        int recordTime = GameDataManager.Instance.DATA.recordTime;
        GameDataManager.Instance.DATA = new GameDataManager.Data();

        if(wonGame)
        {
            GameDataManager.Instance.DATA.wonGame = true;
            GameDataManager.Instance.DATA.recordTime = recordTime;
        }
        if(gotAllHats)
        {
            GameDataManager.Instance.DATA.gotAllHats = true;
        }


        GameDataManager.Instance.SaveData();
    }
    private void PlayButtonSound()
    {
        AudioManager.Instance.PlaySound(buttonSound, 1, 1.5f, false);
    }
    private void DisplayRecordTime()
    {
        escapedIn.text = "Escaped in :";

        int totalTimeInSeconds = GameDataManager.Instance.DATA.recordTime;
        int hours = totalTimeInSeconds / 3600;
        int minutes = (totalTimeInSeconds % 3600) / 60 % 60;
        int seconds = totalTimeInSeconds % 60;
        //time calculations
        recordTime.text = "";
        if (hours < 10)
            recordTime.text = "0" + hours.ToString();
        else
            recordTime.text = hours.ToString();
        if (minutes < 10)
            recordTime.text += " : " + "0" + minutes.ToString();
        else
            recordTime.text += " : " + minutes.ToString();
        if (seconds < 10)
            recordTime.text += " : " + "0" + seconds.ToString();
        else
            recordTime.text += " : " + seconds.ToString();

    }
    private IEnumerator IntroCutscene()
    {
        while (menuGroup.alpha > 0)
        {
            menuGroup.alpha -= Time.fixedDeltaTime / 3;

            yield return new WaitForSeconds(Time.fixedDeltaTime);
        }
        yield return new WaitForSeconds(1f);
        introAnimator.Play("Intro");
        yield return new WaitForSeconds(15f);
        StartCoroutine(LoadNextScene());
        yield break;
    }
    private IEnumerator LoadNextScene()
    {
        while(blackScreen.color.a < 1)
        {
            blackScreen.color += new Color(0, 0, 0, Time.fixedDeltaTime);

            yield return new WaitForSeconds(Time.fixedDeltaTime);
        }
        yield return new WaitForSeconds(1f);

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        yield break;
    }
    private IEnumerator FadeFromBlack()
    {
        blackScreen.color = Color.black;

        while (blackScreen.color.a > 0)
        {
            blackScreen.color -= new Color(0, 0, 0, Time.fixedDeltaTime / 2);

            yield return new WaitForSeconds(Time.fixedDeltaTime);
        }
        yield return new WaitForSeconds(1f);

        yield break;
    }
}
