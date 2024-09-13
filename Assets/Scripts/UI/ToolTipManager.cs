using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolTipManager : MonoBehaviour
{
    [SerializeField] private float xLimit = 960;    
    [SerializeField] private float yLimit = 540;    
    [SerializeField] private float xOffSet = 100;   
    [SerializeField] private float yOffSet = 80;   

    public virtual void AdjustPosition()
    {
        Vector2 mousePosition = Input.mousePosition;

        float xOffSetNew;
        float yOffSetNew;

        if (mousePosition.x > xLimit) xOffSetNew = -xOffSet;
        else xOffSetNew = xOffSet;

        if (mousePosition.y < yLimit) yOffSetNew = -yOffSet;
        else yOffSetNew = yOffSet;


        transform.position = new Vector2(mousePosition.x + xOffSetNew, mousePosition.y + yOffSetNew);
    }
}
