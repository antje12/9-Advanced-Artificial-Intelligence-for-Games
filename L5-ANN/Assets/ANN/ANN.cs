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
        // ToDo
        // Set input values
        for (int i = 0; i < inputLayer.Count; i++)
            inputLayer[i].value = inputs[i];

        // Propagate through hidden layers
        foreach (var hiddenLayer in hiddenLayers)
        {
            foreach (var neuron in hiddenLayer)
                neuron.CalculateValue();
        }

        // Propagate through output layer
        foreach (var neuron in outputLayer)
            neuron.CalculateValue();
    }

    private void BackPropagate(params double[] targets)
    {
        // ToDo
        // Calculate output layer gradients
        for (int i = 0; i < outputLayer.Count; i++)
            outputLayer[i].CalculateGradient(targets[i]);

        // Calculate hidden layer gradients
        for (int i = hiddenLayers.Count - 1; i >= 0; i--)
            foreach (var neuron in hiddenLayers[i])
                neuron.CalculateGradient();

        // Update weights
        foreach (var hiddenLayer in hiddenLayers)
            foreach (var neuron in hiddenLayer)
                neuron.UpdateWeights(learningRate);

        foreach (var neuron in outputLayer)
            neuron.UpdateWeights(learningRate);
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
