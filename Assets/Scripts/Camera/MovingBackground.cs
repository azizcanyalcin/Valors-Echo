using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingBackground : MonoBehaviour
{
    private GameObject cam;
    [SerializeField] private float speed;
    private float xPosition;
    private float lengthOfBackground;

    // Start is called before the first frame update
    void Start()
    {
        cam = GameObject.Find("Main Camera");
        //lengthOfBackground = GetComponent<SpriteRenderer>().bounds.size.x;
        xPosition = transform.position.x;
    }

    // Update is called once per frame
    void Update()
    {
        xPosition += speed;
        float distanceMoved = cam.transform.position.x;

        transform.position = new Vector3(xPosition, transform.position.y);

        // if (distanceMoved > xPosition + lengthOfBackground)
        //     xPosition += lengthOfBackground;
        // else if (distanceMoved < xPosition - lengthOfBackground)
        //     xPosition -= lengthOfBackground;


    }
}
