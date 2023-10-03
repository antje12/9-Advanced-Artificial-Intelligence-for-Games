using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DataSet
{
    public double[] values;
    public double[] targets;

    public DataSet(double[] values, double[] targets)
    {
        this.values = values;
        this.targets = targets;
    }
}

