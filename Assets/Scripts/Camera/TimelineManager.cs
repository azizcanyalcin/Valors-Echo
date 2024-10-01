using UnityEngine;

public class TimelineManager : MonoBehaviour
{
    Player player;
    private void Start()
    {
        player = PlayerManager.instance.player;
    }
    private void Update()
    {
        if (player.isPlayerDeadOnce) Destroy(gameObject);
    }
}