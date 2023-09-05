using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Boo : Agent
{
    private GameObject mario;
    private Transform tMario;
    public float minimumDistance = 6f;
    public float minimumAngle = 80f;
    public float range = 3;

    private void Start()
    {
        mario = GameObject.Find("Mario");
        tMario = mario.transform;
    }

    protected override void FiniteStateMachine()
    {
        switch (state)
        {
            case 0: //Idle
                if (Vector3.Distance(transform.position, tMario.position) < range)
                    state = 1;
                break;
            case 1:
                if (!CanMarioSeeMe())
                    MoveTo(tMario);
                if (Vector3.Distance(transform.position, tMario.position) > range)
                    state = 0;
                break;
            default:
                break;
        }
    }

    bool CanMarioSeeMe()
    {
        //Debug.Log(Vector3.Distance(transform.position, mario.position));
        //Debug.Log(Vector3.Angle(transform.position, mario.forward));
        if (Vector3.Distance(transform.position, tMario.position) < minimumDistance &&
            Vector3.Angle(transform.position, tMario.forward) < minimumAngle)
        {
            Debug.Log("He sees me!");
            return true;
        }
        else
            return false;
    }
}
