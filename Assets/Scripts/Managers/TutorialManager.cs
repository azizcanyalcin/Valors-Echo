using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialManager : MonoBehaviour
{
    public GameObject[] popUps;
    private int popUpIndex;
    private Player player;
    private float playerDefaultJumpForce;
    private float playerDefaultMoveSpeed;
    [SerializeField] private GameObject attackDummy;
    [SerializeField] private SceneTransitionHandler sceneTransitionHandler;
    [SerializeField] private GameObject targetDummy;
    [SerializeField] private GameObject obstacle;
    [SerializeField] private SkillTreeSlotUI bounceSkill;
    private GameObject instantiatedAttackDummy;
    private GameObject instantiatedTargetDummy;
    private GameObject instantiatedObstacle;
    private void Start()
    {
        InitializePlayer();
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
        playerDefaultMoveSpeed = player.moveSpeed;
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
                HandleCase(KeyCode.D);
                break;
            case 1:
                EnablePlayerJump();
                HandleCase(KeyCode.Space);
                break;
            case 2:
                EnablePlayerDash();
                HandleCase(KeyCode.LeftShift);
                break;
            case 3:
                HandleCase(KeyCode.C, Inventory.instance.inventory.Count > 0);
                break;
            case 4:
                EnablePlayerParry();
                HandleCase(KeyCode.Q);
                break;
            case 5:
                SpawnAttackDummy();
                popUpIndex++;
                break;
            case 6:
                HandlePrimaryAttackCase();
                break;
            case 7:
                SpawnTargetDummy();
                popUpIndex++;
                break;
            case 8:
                HandleSwordThrowCase();
                break;
            case 9:
                HandleBounceUnlockCase();
                break;
            case 10:
                HandleObstacleCase();
                break;
            case 11:
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
    private void HandleBounceUnlockCase()
    {
        if (bounceSkill.unlocked) MoveToNextStep();
    }
    private void HandleSwordThrowCase()
    {
        GameObject sword = GameObject.FindWithTag("Sword");
        if (CheckTargetHit())
        {
            Destroy(sword);
            SpawnObstacle();
            SpawnTargetDummy();
            MoveToNextStep();
        }
    }
    private void HandleObstacleCase()
    {
        if (CheckTargetHit())
        {
            Destroy(instantiatedObstacle, 0.25f);
            sceneTransitionHandler.StartSceneTransition();
            MoveToNextStep();
        }
    }
    private void MoveToNextStep()
    {
        AudioManager.instance.PlaySFX(62);
        popUpIndex++;
        Debug.Log($"Moved To Next Step!!");
        Debug.Log($"Pop-up Index = " + popUpIndex);
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
        instantiatedAttackDummy = InstantiateDummy(3, 10, attackDummy);
        AudioManager.instance.PlayDelayedSFX(63, 0.2f);
    }
    private void SpawnTargetDummy()
    {
        player.skill.sword.swordUnlocked = true;
        instantiatedTargetDummy = InstantiateDummy(10, 10, targetDummy);
        AudioManager.instance.PlaySFX(63);

    }
    private void SpawnObstacle()
    {
        Destroy(instantiatedTargetDummy);
        instantiatedObstacle = InstantiateDummy(5, 10, obstacle);
        AudioManager.instance.PlaySFX(63);

    }
    private GameObject InstantiateDummy(float xOffset, float yOffset, GameObject dummy)
    {
        if (player.transform.position.x >= 25)
        {
            xOffset *= -1;
            return Instantiate(dummy, new Vector3(player.transform.position.x + xOffset, player.transform.position.y + yOffset), Quaternion.Euler(0, 180, 0));
        }
        return Instantiate(dummy, new Vector3(player.transform.position.x + xOffset, player.transform.position.y + yOffset), Quaternion.identity);
    }
    private bool CheckTargetHit()
    {
        if (instantiatedTargetDummy)
        {
            Vector2 targetDummyOverlapCircle = new(instantiatedTargetDummy.transform.position.x, instantiatedTargetDummy.transform.position.y + .2f);
            if (Physics2D.OverlapCircle(targetDummyOverlapCircle, 1f, LayerMask.GetMask("Sword"))) return true;
        }
        return false;
    }

    private void OnDrawGizmos()
    {
        if (instantiatedTargetDummy)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(instantiatedTargetDummy.transform.position, 1f);
        }
    }

}
