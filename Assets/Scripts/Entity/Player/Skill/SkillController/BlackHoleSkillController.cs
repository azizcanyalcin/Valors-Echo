using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackHoleSkillController : MonoBehaviour
{
    [SerializeField] private GameObject hotKeyPrefab;
    [SerializeField] private List<KeyCode> keyCodes;

    private float maxSize;
    private float growSpeed;
    private float shrinkSpeed;
    private float blackholeDuration;

    private bool canGrow = true;
    private bool canShrink;
    private bool canCreateHotkeys = true;
    private bool isAttacking;
    private bool canPlayerBeTransparent = true;

    private int amounOfAttacks = 4;
    private float cloneAttackCooldown = .3f;
    private float cloneAttackTimer;


    private List<Transform> targets = new List<Transform>();
    private List<GameObject> createdHotkey = new List<GameObject>();

    public bool canPlayerExitState { get; private set; }

    public void SetupBlackHoleSkill(float maxSize, float growSpeed, float shrinkSpeed, int amounOfAttacks, float cloneAttackCooldown, float blackholeDuration)
    {
        this.maxSize = maxSize;
        this.growSpeed = growSpeed;
        this.shrinkSpeed = shrinkSpeed;
        this.amounOfAttacks = amounOfAttacks;
        this.cloneAttackCooldown = cloneAttackCooldown;
        this.blackholeDuration = blackholeDuration;

        if (SkillManager.instance.clone.canSwapCrystalWithClone)
            canPlayerBeTransparent = false;

    }
    private void Update()
    {
        cloneAttackTimer -= Time.deltaTime;
        blackholeDuration -= Time.deltaTime;

        if (blackholeDuration < 0 && targets.Count > 0)
        {
            blackholeDuration = Mathf.Infinity;
            DoCloneAttack();
        }
        else if (Input.GetKeyDown(KeyCode.R) && targets.Count > 0)
        {
            DoCloneAttack();
        }
        else if (blackholeDuration < 0)
        {
            FinalizeBlackHoleSkill();
        }


        CloneAttack();
        Grow();
        Shrink();
    }
    private void DoCloneAttack()
    {

        DestroyHotKeys();
        isAttacking = true;
        canCreateHotkeys = false;
        if (canPlayerBeTransparent)
        {
            canPlayerBeTransparent = false;
            PlayerManager.instance.player.fx.MakeTransparent(true);
        }

    }
    private void CloneAttack()
    {
        if (cloneAttackTimer < 0 && isAttacking && amounOfAttacks > 0)
        {
            cloneAttackTimer = cloneAttackCooldown;
            int randomIndex = Random.Range(0, targets.Count);

            float xOffset;

            if (Random.Range(0, 100) > 50)
                xOffset = 2;
            else
                xOffset = -2;


            if (SkillManager.instance.clone.canSwapCrystalWithClone)
            {
                SkillManager.instance.crystal.CreateCrystal();
                SkillManager.instance.crystal.ChooseRandomEnemy();
            }

            else
                SkillManager.instance.clone.CreateClone(targets[randomIndex], new Vector3(xOffset, 0));
            amounOfAttacks--;

            if (amounOfAttacks <= 0)
            {
                Invoke("FinalizeBlackHoleSkill", 1f);
            }

        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Enemy>() != null)
        {
            collision.GetComponent<Enemy>().FreezeTime(true);
            CreateHotkey(collision);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<Enemy>() != null)
        {
            collision.GetComponent<Enemy>().FreezeTime(false);
        }
    }
    private void Grow()
    {
        if (canGrow && !canShrink)
        {
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(maxSize, maxSize), growSpeed * Time.deltaTime);
        }
    }
    private void Shrink()
    {
        if (canShrink)
        {
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(-1, -1), shrinkSpeed * Time.deltaTime);
            if (transform.localScale.x <= 0)
            {
                Destroy(gameObject);
            }
        }
    }
    private void CreateHotkey(Collider2D collision)
    {
        if (keyCodes.Count <= 0 || !canCreateHotkeys)
        {
            return;
        }


        GameObject newHotKey = Instantiate(hotKeyPrefab, collision.transform.position + new Vector3(0, 2), Quaternion.identity);
        createdHotkey.Add(newHotKey);

        KeyCode choosenKey = keyCodes[Random.Range(0, keyCodes.Count)];
        keyCodes.Remove(choosenKey);

        BlackHoleHotkeyController newHotKeyScript = newHotKey.GetComponent<BlackHoleHotkeyController>();

        newHotKeyScript.SetUpHotKey(choosenKey, collision.transform, this);
    }
    public void AddEnemyToList(Transform enemy) => targets.Add(enemy);
    private void DestroyHotKeys()
    {
        if (createdHotkey.Count <= 0)
            return;
        for (int i = 0; i < createdHotkey.Count; i++)
        {
            Destroy(createdHotkey[i]);
        }
    }
    private void FinalizeBlackHoleSkill()
    {
        DestroyHotKeys();
        canPlayerExitState = true;
        canShrink = true;
        isAttacking = false;
    }
}