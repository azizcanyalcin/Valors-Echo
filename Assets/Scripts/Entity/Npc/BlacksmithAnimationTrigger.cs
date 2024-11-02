using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlacksmithAnimationTrigger : MonoBehaviour
{
    private Blacksmith blacksmith => GetComponent<Blacksmith>();
    public void TriggerSmashEffect()
    {
        AudioManager.instance.PlaySFX(60, blacksmith.transform);
    }
}
