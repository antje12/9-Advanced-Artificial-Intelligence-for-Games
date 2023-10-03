using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Neuron
{
    public List<Synapse> inputs;
    public List<Synapse> outputs;
    public double value;
    public double gradient;
    double bias;

    public Neuron()
    {
        inputs = new List<Synapse>();
        outputs = new List<Synapse>();
        bias = Random.Range(-1, 1);
    }

    public Neuron(IEnumerable<Neuron> inputNeurons) : this()
    {
        foreach (Neuron inputNeuron in inputNeurons)
        {
            Synapse s = new Synapse(inputNeuron, this);
            inputNeuron.outputs.Add(s);
            inputs.Add(s);
        }
    }

    public double CalculateValue()
    {
        throw new UnityException("You haven't implemented this method");
    }

    double Sigmoid(double x)
    {
        return 1 / (1 + Mathf.Exp((float)-x));
    }

    double SigmoidDerivative(double x)
    {
        return x * (1 - x);
    }

    public double CalculateError(double target)
    {
        throw new UnityException("You haven't implemented this method");
    }

    public void CalculateGradient(double target) //for output layer
    {
        //value is the y
        throw new UnityException("You haven't implemented this method");
    }

    public void CalculateGradient() //for hidden layers
    {
        throw new UnityException("You haven't implemented this method");
    }

    public void UpdateWeights(double learningRate)
    {
        //update bias
        double biasDelta = learningRate * gradient;
        bias += biasDelta;

        //update the other weights
        throw new UnityException("You haven't implemented this method");
    }
}

