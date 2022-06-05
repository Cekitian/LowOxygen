using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeInScreen : MonoBehaviour
{
    [SerializeField] private Image blackScreen;

    private IEnumerator Start()
    {
        blackScreen.color = Color.black;

        AudioSource musicSource = GameManager.Instance.GetGameMusic();
        while (blackScreen.color.a > 0)
        {
            musicSource.volume = (1 - blackScreen.color.a) / 5;
            blackScreen.color -= new Color(0, 0, 0, Time.fixedDeltaTime / 2);
            yield return new WaitForSeconds(Time.fixedDeltaTime);
        }
        yield break;
    }
}
