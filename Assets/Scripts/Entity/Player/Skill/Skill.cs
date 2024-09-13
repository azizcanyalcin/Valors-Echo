using System.Collections;
using UnityEngine;

public class Skill : MonoBehaviour
{
    public float cooldown;
    public float cooldownTimer;
    protected Player player;
    protected virtual void Start()
    {
        player = PlayerManager.instance.player;
        StartCoroutine(LoadUnlockedSkills());
        
    }
    protected virtual void Update()
    {
        cooldownTimer -= Time.deltaTime;
    }
    protected virtual void CheckUnlock()
    {
        Debug.Log($"Unlock Checked!!");
    }
    public virtual bool CanUseSkill()
    {
        if (cooldownTimer < 0)
        {
            UseSkill();
            cooldownTimer = cooldown;
            return true;
        }
        return false;
    }

    public virtual void UseSkill()
    {

    }
    protected virtual Transform FindClosestEnemy(Transform checkTransform)
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(checkTransform.position, 25);
        float closestDistance = Mathf.Infinity;
        Transform closestEnemy = null;

        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null)
            {
                float distanceToEnemy = Vector2.Distance(checkTransform.position, hit.transform.position);
                if (distanceToEnemy < closestDistance)
                {
                    closestDistance = distanceToEnemy;
                    closestEnemy = hit.transform;
                }
            }
        }

        return closestEnemy;
    }
    IEnumerator LoadUnlockedSkills()
    {
        yield return new WaitForSeconds(0.5f);
        CheckUnlock();
    }
}