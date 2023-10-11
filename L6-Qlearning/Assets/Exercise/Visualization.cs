using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Visualization : MonoBehaviour
{
    public GameObject agentPrefab;
    public GameObject goalPrefab;
    public GameObject obstaclePrefab;
    public GameObject planePrefab;
    public GameObject QPrefab;

    GameObject agent;
    GameObject goal;
    List<GameObject> obstacles = new List<GameObject>();
    GameObject plane;

    GameObject[] qObjects;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void Setup(char[] map, int size)
    {
        Cleanup();
        if (qObjects == null)
            qObjects = new GameObject[size * size];

        plane = Instantiate(planePrefab, new Vector3(size / 2, -0.5f, size / 2), Quaternion.identity);
        plane.transform.localScale = new Vector3(size / 10f, 1, size / 10f);

        for (int x = 0; x < size; x++)
        {
            for (int y = 0; y < size; y++)
            {
                switch (map[x + y * size])
                {
                    case '@':
                        agent = Instantiate(agentPrefab, new Vector3(x, 0, y), Quaternion.identity);
                        break;
                    case '#':
                        obstacles.Add(Instantiate(obstaclePrefab, new Vector3(x, 0, y), Quaternion.identity));
                        break;
                    case 'G':
                        goal = Instantiate(goalPrefab, new Vector3(x, 0, y), Quaternion.identity);
                        break;
                    default:
                        break;
                }

                if (qObjects[x + y * size] == null)
                    qObjects[x + y * size] = Instantiate(QPrefab, new Vector3(x, 0, y), Quaternion.identity);
            }
        }
    }

    void Cleanup()
    {
        Destroy(agent);
        Destroy(goal);
        Destroy(plane);
        foreach (GameObject g in obstacles)
            Destroy(g);
        /*
        if (qObjects != null)
            foreach (GameObject g in qObjects)
                Destroy(g);
        */
    }

    public void UpdateMap(char[] map, int size)
    {
        for (int i = 0; i < map.Length; i++)
        {
            if (map[i] == '@')
            {
                int x = i % size;
                int y = i / size;
                agent.transform.position = new Vector3(x, 0, y);
                break;
            }
        }
    }

    public void UpdateQRepresentation(int x, int y, float maxValue, int size)
    {
        Debug.Log("Update to: " + maxValue);
        qObjects[x + y * size].transform.localScale = new Vector3(maxValue / 10, maxValue / 10, maxValue / 10);
    }
}