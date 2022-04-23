using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConveyorTextureScroll : MonoBehaviour
{
    public float scrollSpeed;

    private Material conveyorMat;
    // Start is called before the first frame update
    void Start()
    {
        conveyorMat = GetComponent<MeshRenderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
        conveyorMat.mainTextureOffset = new Vector2(0, conveyorMat.mainTextureOffset.y - scrollSpeed * Time.deltaTime);
    }
}
