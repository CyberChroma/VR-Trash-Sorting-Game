using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerUI : MonoBehaviour
{
    public float startMinutes;
    public float startSeconds;
    public Text timerText;

    private float minutesLeft;
    private float secondsLeft;

    public GameObject endGameBoard;
    public GameObject spawner;
    public GameObject menuThrow;

    public AudioSource menuMusic;
    public AudioSource gameMusic;

    // Start is called before the first frame update
    void Start()
    {
        minutesLeft = startMinutes;
        secondsLeft = startSeconds;
        timerText.text = string.Format("Time:\n{0:00}:{1:00}", minutesLeft, secondsLeft);
        StartCoroutine(CountDown());
    }

    public void ResetTime()
    {
        minutesLeft = startMinutes;
        secondsLeft = startSeconds;
    }

    IEnumerator CountDown()
    {
        while(true) {
            yield return new WaitForSeconds(1);
            secondsLeft--;
            if (secondsLeft == -1) {
                minutesLeft--;
                if (minutesLeft == -1) {
                    print("Times Up!!");
                    endGameBoard.SetActive(true);
                    spawner.SetActive(false);
                    menuThrow.SetActive(true);
                    gameMusic.Stop();
                    menuMusic.Play();
                    break;
                }
                secondsLeft = 59;
            }
            timerText.text = string.Format ("Time:\n{0:00}:{1:00}", minutesLeft, secondsLeft); 
        }
    }
}
