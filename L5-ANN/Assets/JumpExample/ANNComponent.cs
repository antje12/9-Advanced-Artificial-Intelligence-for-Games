using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ANNComponent : MonoBehaviour
{

    List<DataSet> trainingSet = new List<DataSet>();

    public int maxSets = 60;
    public int epochsToTrain = 200;
    bool trained = false;
    ANN net;

    public Player player;

    // Start is called before the first frame update
    void Start()
    {
        net = new ANN(2, 3, 1);
    }

    // Update is called once per frame
    void Update()
    {
        if (trained)
        {
            double[] results = net.Compute(new double[] {player.distanceToProjectile, player.CanJump()});
            Debug.Log("Output: "+results[0]);
            if (results[0] > 0.5)
                player.Jump();
        }
    }

    public void SaveData(double distance, double canJump, double jumped)
    {
        double[] inp = { distance, canJump };
        double[] outp = { jumped };
        trainingSet.Add(new DataSet(inp, outp));
        Debug.Log("Added new set, total: "+trainingSet.Count);

        if (! trained && trainingSet.Count == maxSets)
        {
            Debug.Log("Got enough data, starting training");
            net.Train(trainingSet, epochsToTrain);
            trained = true;
            Debug.Log("Done training");
        }
    }
}
