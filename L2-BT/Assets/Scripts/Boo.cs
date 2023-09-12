using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum BooStates
{
    Chase,
    Freeze,
    Reset
}

public class Boo : FSM_Agent<BooStates> {

    private Transform mario;
    private BT_Mario marioScript;
    public float minimumDistance = 6f;
    public float minimumAngle = 80f;

    private Vector3 startPosition;
    private bool frozen;
    private int lastResetScore;
    private SpriteRenderer renderer;
    private CapsuleCollider collider;

    private void Start()
    {
        mario = GameObject.Find("Mario").transform;
        marioScript = mario.GetComponent<BT_Mario>();
        startPosition = transform.position;
        renderer = GetComponent<SpriteRenderer>();
        collider = GetComponent<CapsuleCollider>();
    }

    protected override void FiniteStateMachine()
    {
        switch (state)
        {
            case BooStates.Chase:
                //reset boo being visible and collidable
                nm.isStopped = false;
                renderer.enabled = true;
                collider.enabled = true;
                // chase mario
                MoveTo(mario);
                //transitions
                if (marioScript.score % 5 == 0 && lastResetScore != marioScript.score)
                    state = BooStates.Reset;
                if (CanMarioSeeMe())
                    state = BooStates.Freeze;
                break;
            case BooStates.Freeze:
                //boo stops and becomes invisible
                nm.isStopped = true;
                renderer.enabled = false;
                collider.enabled = false;
                //transition
                if (!CanMarioSeeMe())
                    state = BooStates.Chase;
                break;
            case BooStates.Reset:
                transform.position = startPosition;
                lastResetScore = marioScript.score;
                state = BooStates.Freeze;
                break;
        }
        
    }

    bool CanMarioSeeMe()
    {
        //Debug.Log(Vector3.Distance(transform.position, mario.position));
        //Debug.Log(Vector3.Angle(transform.position, mario.forward));
        if (Vector3.Distance(transform.position, mario.position) < minimumDistance &&
            Vector3.Angle(transform.position, mario.forward) < minimumAngle)
        {
            Debug.Log("He sees me!");
            return true;
        }
        else
            return false;
    }
    
}
