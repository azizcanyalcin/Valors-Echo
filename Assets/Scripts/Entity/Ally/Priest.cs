using System.Collections;
using UnityEngine;

public class Priest : MonoBehaviour
{
    Player player;

    private void Start()
    {
        player = PlayerManager.instance.player;
        
    }
}