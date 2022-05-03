using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeChange : MonoBehaviour
{
    private Text volText;

    void Start()
    {
        volText = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        volText.text = "Volume:\n" + Mathf.RoundToInt(100 * AudioListener.volume)+"%";
    }

    public void AddVolume(float inc)
    {
        AudioListener.volume = Mathf.Min(Mathf.Max(AudioListener.volume + inc, 0), 1);
    }
}
