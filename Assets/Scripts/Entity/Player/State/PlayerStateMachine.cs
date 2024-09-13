using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerStateMachine
{
    public PlayerState currentState {get; private set;}
    public void Initialize(PlayerState defaultState){
        currentState = defaultState;
        currentState.Enter();
    }
    public void ChangeState(PlayerState newState){
        currentState.Exit();
        currentState = newState;
        currentState.Enter();
    }
}
