using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public GameObject[] popUps;
    private int popUpIndex;
    private Player player;
    private float playerDefaultJumpForce;

    [SerializeField] private GameObject attackDummy;
    [SerializeField] private GameObject targetDummy;
    [SerializeField] private GameObject portal;
    private GameObject instantiatedAttackDummy;
    private GameObject instantiatedTargetDummy;

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
        if (Input.GetKeyUp(KeyCode.Mouse1))
        {
            portal.GetComponent<Collider2D>().isTrigger = true;
            MoveToNextStep();
        }
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
        instantiatedAttackDummy = InstantiateDummy(2, 10, attackDummy);
        AudioManager.instance.PlayDelayedSFX(63, 0.2f);
        popUpIndex++;
    }
    private void SpawnTargetDummy()
    {
        player.skill.sword.swordUnlocked = true;
        instantiatedTargetDummy = InstantiateDummy(7, 10, targetDummy);
        AudioManager.instance.PlaySFX(63);
        popUpIndex++;
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

}
