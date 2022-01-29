using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreUI : MonoBehaviour
{
    public float scoreAddCorrect = 10;
    public float scoreDeductIncorrect = 10;
    public Text scoreText;

    private float score;

    // Start is called before the first frame update
    void Start()
    {
        scoreText.text = "Score:\n0";
    }

    public void AddScore()
    {
        score += scoreAddCorrect;
        UpdateText();
    }

    public void DeductScore()
    {
        score -= scoreDeductIncorrect;
        UpdateText();
    }

    void UpdateText()
    {
        scoreText.text = "Score:\n" + score;
    }
}
