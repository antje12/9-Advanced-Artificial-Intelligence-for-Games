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
        // ToDo
        double sum = bias;
        foreach (Synapse input in inputs)
        {
            sum += input.inputNeuron.value * input.weight;
        }
        value = Sigmoid(sum);
        return value;
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
        // ToDo
        return 0.5 * Mathf.Pow((float) (target - value), 2);
    }

    public void CalculateGradient(double target) //for output layer
    {
        // ToDo
        //value is the y
        gradient = (target - value) * SigmoidDerivative(value);
    }

    public void CalculateGradient() //for hidden layers
    {
        // ToDo
        // Calculate the sum of the weighted gradients from the neurons in the next layer
        double sumWeightedGradients = 0.0;
        foreach (Synapse outputSynapse in outputs)
        {
            sumWeightedGradients += outputSynapse.weight * outputSynapse.outputNeuron.gradient;
        }

        // Calculate the gradient for the current neuron
        gradient = sumWeightedGradients * SigmoidDerivative(value);
    }

    public void UpdateWeights(double learningRate)
    {
        //update bias
        double biasDelta = learningRate * gradient;
        bias += biasDelta;

        // ToDo
        //update the other weights
        foreach (Synapse input in inputs)
        {
            double weightDelta = learningRate * gradient * input.inputNeuron.value;
            input.weight -= weightDelta;
        }
    }
}
