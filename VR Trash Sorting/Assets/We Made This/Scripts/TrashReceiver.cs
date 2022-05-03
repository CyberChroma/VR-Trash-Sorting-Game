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

    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        TrashItem trashItem = other.GetComponent<TrashItem>();
        LiquidPour liquidPouring = trashItem.GetComponent<LiquidPour>();
        float bonusPoints = 0;
        if ((trashItem.needsPrep && trashItem.isFlattened) || (liquidPouring != null && liquidPouring.liquidCapacity <= 0))
        {
            bonusPoints += 5;
        }

        if (trashItem) {
            if (trashItem.trashType == trashType) {
                print("Correct Sorting!");
                scoreUI.AddScore(bonusPoints);
                Destroy(other.gameObject);
            }
            else {
                print("Incorrect Sorting!");
                scoreUI.DeductScore(bonusPoints);
                Destroy(other.gameObject);
            }
        }
    }
}
