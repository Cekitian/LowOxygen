using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurboManager : MonoBehaviour
{
    public static TurboManager Instance;

    private bool turboActive = false;
    private void Awake()
    {
        if (Instance != null)
            Destroy(gameObject);
        Instance = this;

        DontDestroyOnLoad(gameObject);
    }
    public void ChangeTurboState()
    {
        turboActive = !turboActive;

        if (turboActive)
            Time.timeScale = 2;
        else
            Time.timeScale = 1;
    }
}
