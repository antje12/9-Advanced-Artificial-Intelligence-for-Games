using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mario : Agent
{
    public int score = 0;
    public float range = 2;

    protected override void FiniteStateMachine()
    {
        GameObject boo = IsBooClose();
        state = boo != null ? 1 : 0;
        MoveTo(GetClosestCoin());
        switch (state)
        {
            case 0:
                MoveTo(GetClosestCoin());
                break;
            case 1:
                MoveTo(FleeFrom(boo));
                break;
        }
    }

    GameObject IsBooClose()
    {
        GameObject[] boos = GameObject.FindGameObjectsWithTag("Boo");
        float minDistance = range;
        GameObject closestBoo = null;
        foreach (GameObject boo in boos)
        {
            float distance = Vector3.Distance(boo.transform.position, transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                closestBoo = boo;
            }
        }
        return closestBoo;
    }

    Transform GetClosestCoin()
    {
        GameObject[] coins = GameObject.FindGameObjectsWithTag("Coin");
        float minDistance = float.PositiveInfinity;
        GameObject closestCoin = null;
        foreach (GameObject coin in coins)
        {
            float distance = Vector3.Distance(coin.transform.position, transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                closestCoin = coin;
            }
        }
        return closestCoin.transform;
    }

    Vector3 FleeFrom(GameObject boo)
    {
        Debug.Log("Run!");
        Vector3 dirToBoo = transform.position - boo.transform.position;
        Vector3 newPos = transform.position + dirToBoo;
        return newPos;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Coin")
        {
            score++;
            Debug.Log("Got a coin!");
            Destroy(other.gameObject);
        }
        if (other.gameObject.tag == "Boo")
        {
            Debug.Log("Mamma mia! [score:" + score + "]");
            Destroy(gameObject);
        }
    }
}
