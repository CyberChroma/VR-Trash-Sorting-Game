using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreUI : MonoBehaviour
{
    public float scoreAddCorrect = 10;
    public float scoreDeductIncorrect = 10;
    public float scoreComboBonus = 10;
    public int comboInterval = 5;
    public int maxComboMultiplier = 4;
    public Text scoreText;
    public Text comboText;
    public Text finalScoreText;

    private float score;
    private int combo;

    // Start is called before the first frame update
    void Start()
    {
        scoreText.text = "Score:\n0";
        comboText.text = "Combo:\n0";
    }

    public void AddScore(float bonus)
    {
        int comboMultiplier = Mathf.Min(combo / comboInterval, maxComboMultiplier);
        score += scoreAddCorrect + scoreComboBonus * comboMultiplier + bonus * (comboMultiplier + 1);
        combo += 1;
        UpdateText();
    }

    public void DeductScore(float bonus)
    {
        score -= scoreDeductIncorrect;
        score += bonus;
        combo = 0;
        UpdateText();
    }

    void UpdateText()
    {
        scoreText.text = "Score:\n" + score;
        comboText.text = "Combo:\n" + combo;
        finalScoreText.text = "Final Score:\n" + score;
        if (combo >= comboInterval * 4)
        {
            comboText.color = Color.magenta;
        }
        else if (combo >= comboInterval * 3)
        {
            comboText.color = Color.cyan;
        }
        else if (combo >= comboInterval * 2)
        {
            comboText.color = Color.green;
        }
        else if (combo >= comboInterval)
        {
            comboText.color = Color.yellow;
        }
        else
        {
            comboText.color = Color.red;
        }
    }

    public void ResetScore()
    {
        score = 0;
        combo = 0;
        UpdateText();
    }
}
