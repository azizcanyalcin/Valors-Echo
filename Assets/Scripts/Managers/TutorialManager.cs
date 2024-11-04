using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public GameObject[] popUps;
    private int popUpIndex;
    private Player player;
    private float playerDefaultJumpForce;

    [SerializeField] private GameObject attackDummy;
    [SerializeField] private GameObject targetDummy;
    private GameObject instantiatedAttackDummy;
    private GameObject instantiatedTargetDummy;
    private PolygonCollider2D targetCollider;

    private void Start()
    {
        InitializePlayer();
        targetCollider = targetDummy.GetComponent<PolygonCollider2D>();
    }

    private void Update()
    {
        UpdatePopUps();
        HandleTutorialStep();
    }

    private void InitializePlayer()
    {
        player = PlayerManager.instance.player;
        playerDefaultJumpForce = player.jumpForce;
    }

    private void UpdatePopUps()
    {
        for (int i = 0; i < popUps.Length; i++)
        {
            popUps[i].SetActive(i == popUpIndex);
        }
    }

    private void HandleTutorialStep()
    {
        switch (popUpIndex)
        {
            case 0:
                DisablePlayerSkills();
                HandleCase(KeyCode.A, Inventory.instance.inventory.Count > 0);
                break;
            case 1:
                EnablePlayerJump();
                HandleCase(KeyCode.C);
                break;
            case 2:
                HandleCase(KeyCode.Space);
                break;
            case 3:
                EnablePlayerDash();
                HandleCase(KeyCode.LeftShift);
                break;
            case 4:
                EnablePlayerParry();
                HandleCase(KeyCode.Q);
                break;
            case 5:
                SpawnAttackDummy();
                break;
            case 6:
                HandlePrimaryAttackCase();
                break;
            case 7:
                SpawnTargetDummy();
                break;
            case 8:
                HandleSwordThrowCase();
                break;
        }
    }
    private void HandleCase(KeyCode key)
    {
        if (Input.GetKeyDown(key)) MoveToNextStep();
    }
    private void HandleCase(KeyCode key, bool andGate)
    {
        if (Input.GetKeyDown(key) && andGate) MoveToNextStep();
    }
    private void HandleCase(KeyCode key, float delay)
    {
        if (Input.GetKeyDown(key)) Invoke(nameof(MoveToNextStep), delay);
    }
    private void HandlePrimaryAttackCase()
    {
        if (!instantiatedAttackDummy) MoveToNextStep();
    }
    private void HandleSwordThrowCase()
    {
        if (Input.GetKeyUp(KeyCode.Mouse1)) MoveToNextStep();
    }
    private void MoveToNextStep()
    {
        AudioManager.instance.PlaySFX(62);
        popUpIndex++;
    }

    private void DisablePlayerSkills()
    {
        player.jumpForce = 0;
        player.skill.dash.dashUnlocked = false;
        player.skill.parry.parryUnlocked = false;
    }

    private void EnablePlayerJump() => player.jumpForce = playerDefaultJumpForce;

    private void EnablePlayerDash() => player.skill.dash.dashUnlocked = true;

    private void EnablePlayerParry() => player.skill.parry.parryUnlocked = true;

    private void SpawnAttackDummy()
    {
        instantiatedAttackDummy = Instantiate(attackDummy, new Vector3(player.transform.position.x + 2f, player.transform.position.y + 10), Quaternion.identity);
        AudioManager.instance.PlayDelayedSFX(63, 0.2f);
        popUpIndex++;
    }
    private void SpawnTargetDummy()
    {
        player.skill.sword.swordUnlocked = true;
        instantiatedTargetDummy = Instantiate(targetDummy, new Vector3(player.transform.position.x + 7f, player.transform.position.y + 10), Quaternion.identity);
        AudioManager.instance.PlaySFX(63);
        popUpIndex++;
    }

}
