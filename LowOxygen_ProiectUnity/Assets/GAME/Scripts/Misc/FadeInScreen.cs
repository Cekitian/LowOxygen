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
        while(blackScreen.color.a > 0)
        {
            blackScreen.color -= new Color(0, 0, 0, Time.fixedDeltaTime / 2);
            yield return new WaitForSeconds(Time.fixedDeltaTime);
        }
        yield break;
    }
}
