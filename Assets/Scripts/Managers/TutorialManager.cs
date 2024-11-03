using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public GameObject[] popUps;
    private int popUpIndex;
    private Player player;
    private float playerDefaultJumpForce;

    private void Start()
    {
        InitializePlayer();
        InitializeTutorialSteps();
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
                DisablePlayerJumpAndDash();
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
        }
    }
    private void HandleCase(KeyCode key)
    {
        if(Input.GetKeyDown(key)) MoveToNextStep();
    }
    private void HandleCase(KeyCode key, bool andGate)
    {
        if(Input.GetKeyDown(key) && andGate) MoveToNextStep();
    }
    private void MoveToNextStep()
    {
        popUpIndex++;
    }

    private void DisablePlayerJumpAndDash()
    {
        player.jumpForce = 0;
        player.skill.dash.dashUnlocked = false;
    }

    private void EnablePlayerJump()
    {
        player.jumpForce = playerDefaultJumpForce;
    }

    private void EnablePlayerDash()
    {
        player.skill.dash.dashUnlocked = true;
    }

    private void InitializeTutorialSteps()
    {
        // Any additional initialization for steps can go here
    }
}
