using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public class ANN
{
    public double learningRate;
    public List<Neuron> inputLayer;
    public List<List<Neuron>> hiddenLayers;
    public List<Neuron> outputLayer;

    public ANN(int inputNeurons, int hiddenNeuronsPerLayer, int outputNeurons, int numHiddenLayers = 1, double learnRate = 0.4)
    {
        learningRate = learnRate;
        inputLayer = new List<Neuron>();
        hiddenLayers = new List<List<Neuron>>();
        outputLayer = new List<Neuron>();

        for (int i = 0; i < inputNeurons; i++)
            inputLayer.Add(new Neuron());

        for (int i = 0; i < numHiddenLayers; i++)
        {
            hiddenLayers.Add(new List<Neuron>());
            for (int j = 0; j < hiddenNeuronsPerLayer; j++)
                hiddenLayers[i].Add(new Neuron(i == 0 ? inputLayer : hiddenLayers[i - 1]));
        }

        for (int i = 0; i < outputNeurons; i++)
            outputLayer.Add(new Neuron(hiddenLayers[numHiddenLayers - 1]));
    }

    public void Train(List<DataSet> data, int numEpochs)
    {
        for (int i = 0; i < numEpochs; i++)
        {
            foreach (DataSet set in data)
            {
                ForwardPropagate(set.values);
                BackPropagate(set.targets);
            }
        }
    }

    public void Train(List<DataSet> data, double minimumError)
    {
        var error = 1.0;
        var numEpochs = 0;

        while (error > minimumError && numEpochs < int.MaxValue)
        {
            var errors = new List<double>();
            foreach (var set in data)
            {
                ForwardPropagate(set.values);
                BackPropagate(set.targets);
                errors.Add(CalculateError(set.targets));
            }
            error /= outputLayer.Count; //average the error
            numEpochs++;
        }
    }

    private void ForwardPropagate(params double[] inputs)
    {
        throw new UnityException("You haven't implemented this method");
    }

    private void BackPropagate(params double[] targets)
    {
        throw new UnityException("You haven't implemented this method");
    }

    public double[] Compute(double[] inputs)
    {
        ForwardPropagate(inputs);
        List<double> result = new List<double>();
        foreach (Neuron n in outputLayer)
        {
            result.Add(n.value);
        }
        return result.ToArray();
    }

    private double CalculateError(params double[] targets)
    {
        double sum = 0;
        for (int i = 0; i < targets.Length; i++)
        {
            sum += outputLayer[i].CalculateError(targets[i]);
        }
        return sum;
    }
}
