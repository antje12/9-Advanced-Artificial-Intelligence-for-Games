using SimpleBehaviourTree;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BT_Agent : TreeHandler {
    private int breaker = 0;
    private bool? result = null;
    [SerializeField] private Transform point1;
    [SerializeField] private Transform point2;

    void Update() {
        Execute();
    }

    public void MoveTowardsPoint1(Node node) {
        Debug.Log("Move 1");
        transform.position = Vector3.MoveTowards(transform.position, point1.transform.position, 1);
        node.SetActionResult(true);
    }

    public void CheckIfArrivedAtPoint1(Node node) {
        Debug.Log("Check 1");
        if (Vector3.Distance(transform.position, point1.position) < 2)
            node.SetActionResult(true);
        else
            node.SetActionResult(false);
    }

    public void MoveTowardsPoint2(Node node) {
        Debug.Log("Move 2");
        transform.position = Vector3.MoveTowards(transform.position, point2.transform.position, 1);
        node.SetActionResult(true);
    }

    public void CheckIfArrivedAtPoint2(Node node) {
        Debug.Log("Check 2");
        if (Vector3.Distance(transform.position, point1.position) < 2)
            node.SetActionResult(true);
        else
            node.SetActionResult(false);
    }
}
