using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class FSM_Agent<CustomStateEnum> : MonoBehaviour {

    public NavMeshAgent nm;
    public Transform target;

    public CustomStateEnum state;

    private void Update()
    {
        FiniteStateMachine();
    }

    //Abstract method that defines the behaviour 
    protected abstract void FiniteStateMachine(); 

    //Go to
    public void MoveTo(Transform destination)
    {
        nm.SetDestination(destination.position);
    }

    public void MoveTo(Vector3 destination)
    {
        nm.SetDestination(destination);
    }
}
