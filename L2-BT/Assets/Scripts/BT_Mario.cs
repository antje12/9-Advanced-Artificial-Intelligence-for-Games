using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using SimpleBehaviourTree;
using UnityEditor;

public class BT_Mario : TreeHandler
{
    public int score = 0;
    public NavMeshAgent nm;
    public Transform target;

    void Update()
    {
        Execute();
    }

    public void GetClosestCoin(Node node)
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
        this.target = closestCoin.transform;
        nm.SetDestination(this.target.position);
		node.SetActionResult(true);
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
