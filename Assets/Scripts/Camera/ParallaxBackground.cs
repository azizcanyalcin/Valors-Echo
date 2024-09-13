using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    private GameObject cam;
    [SerializeField] private float parallaxEffect;
    private float xPosition;
    private float lengthOfBackground;

    // Start is called before the first frame update
    void Start()
    {
        cam = GameObject.Find("Main Camera");
        lengthOfBackground = GetComponent<SpriteRenderer>().bounds.size.x;
        xPosition = transform.position.x;
    }

    // Update is called once per frame
    void Update()
    {
        float distanceMoved = cam.transform.position.x * (1 - parallaxEffect);
        float distanceToMove = cam.transform.position.x * parallaxEffect;
        transform.position = new Vector3(xPosition + distanceToMove, transform.position.y);

        if (distanceMoved > xPosition + lengthOfBackground)
            xPosition += lengthOfBackground; 
        else if( distanceMoved < xPosition - lengthOfBackground)
            xPosition -= lengthOfBackground;

    }
}
