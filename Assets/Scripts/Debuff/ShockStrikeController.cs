using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShockStrikeController : MonoBehaviour
{
    private Animator animator;
    [SerializeField] CharacterStats target;
    [SerializeField] private float speed;
    private bool isTriggered;
    private int damage;

    private void Start()
    {
        animator = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        if (isTriggered || !target) return;

        transform.position = Vector2.MoveTowards(transform.position, target.transform.position, speed * Time.deltaTime);
        transform.right = transform.position - target.transform.position;

        if (Vector2.Distance(transform.position, target.transform.position) < .1f)
        {
            animator.transform.SetLocalPositionAndRotation(new Vector3(0, .5f), Quaternion.identity);

            transform.localRotation = Quaternion.identity;
            transform.localScale = new Vector3(3, 3);

            Invoke("DamageAndSelfDestroy", .2f);

            isTriggered = true;
            animator.SetTrigger("Hit");

        }
    }

    public void InitializeShockStrike(int damage, CharacterStats target)
    {
        this.damage = damage;
        this.target = target;
    }

    private void DamageAndSelfDestroy()
    {
        target.TakeDamage(damage);
        Destroy(gameObject, .4f);
    }
}