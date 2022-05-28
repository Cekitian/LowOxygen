using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ModWinManager : MonoBehaviour
{
    public static ModWinManager Instance;
    public CanvasGroup canvasGroup;
    private void Awake()
    {
        Instance = this;
    }
    public void Win()
    {
        canvasGroup.alpha = 1;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
    }
    public void ExitToMainMenu()
    {
        SceneManager.LoadScene(0);
    }
   
}
