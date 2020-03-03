using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private FSMSystem fsmSystem;

    private void Start()
    {
        fsmSystem = new FSMSystem();
        FSMState patrolState = new PatrolState(fsmSystem);
        FSMState chaseState = new ChaseState(fsmSystem);

        fsmSystem.AddState(patrolState);
        fsmSystem.AddState(chaseState);

        patrolState.AddTransition(Transition.FindPlayer, StateID.ChaseState);
        chaseState.AddTransition(Transition.LosePlayer, StateID.PatrolState);
    }

    private void Update()
    {
        fsmSystem.UpdateFSM();
    }
}
