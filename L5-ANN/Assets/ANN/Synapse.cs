using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Synapse
{
    public Neuron inputNeuron;
    public Neuron outputNeuron;
    public double weight;

    public Synapse(Neuron input, Neuron output)
    {
        inputNeuron = input;
        outputNeuron = output;
        weight = Random.Range(-1, 1);
    }
}

