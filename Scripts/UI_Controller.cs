using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_Controller : MonoBehaviour
{
    public TMP_Text scoreText;

    public void SetScoreText(string txt)
    {
        if (scoreText) scoreText.text = txt;
    }
}
