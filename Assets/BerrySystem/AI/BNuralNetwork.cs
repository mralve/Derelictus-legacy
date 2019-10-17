/* Copyright (C) Xonomoto Studios - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Alve Larsson <zurra.alve@gmail.com>, September 2018
 */

using UnityEngine;

namespace ConstruiSystem
{

    public class BNeuralNetwork
    {
        
        public NeuralSet targetNetwork;

        public void SumInputsToFirstLayer()
        {
            for (int i = 0; i < targetNetwork.layers[0].Neurons.Length; i++)
            {
                //targetNetwork.layers[0].Neurons[i].
            }
        }


    }

    public struct NeuralSet
    {
        public float[] inputNeurons;
        public NeuronLayer[] layers;

    }

    public struct NeuronLayer
    {
        public Neuron[] Neurons;
    }

    public struct Neuron
    {
        public float weight;
        public float value;
    }
}
