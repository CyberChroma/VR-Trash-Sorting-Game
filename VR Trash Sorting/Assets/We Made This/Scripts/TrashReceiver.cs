using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashReceiver : MonoBehaviour
{
    public TrashType trashType;
    private ScoreUI scoreUI;

    // Start is called before the first frame update
    void Start()
    {
        scoreUI = FindObjectOfType<ScoreUI>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        TrashItem trashItem = other.GetComponent<TrashItem>();
        if (trashItem) {
            if (trashItem.trashType == trashType) {
                print("Correct Sorting!");
                scoreUI.AddScore();
                Destroy(other.gameObject);
            }
            else {
                print("Incorrect Sorting!");
                scoreUI.DeductScore();
                Destroy(other.gameObject);
            }
        }
    }
}
