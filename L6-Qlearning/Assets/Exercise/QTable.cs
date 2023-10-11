﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
* The heart of the Q-learning algorithm, the QTable contains the table
* which maps states, actions and their Q values. This class has elaborate
* documentation, and should be the focus of the students' body of work
* for the purposes of this tutorial.
*
* @author A.Liapis (Original author), A. Hartzen (2015 modifications), Marco Scirea (2019 C#/Unity adaptation) 
*/
namespace Exercise
{
    public class QTable
    {
        /**
         * for creating random numbers
         */
        System.Random randomGenerator;

        /**
         * the table variable stores the Q-table, where the state is saved
         * directly as the actual map. Each map state has an array of Q values
         * for all the actions available for that state.
         */
        Dictionary<string, float[]> table;

        /**
         * the actionRange variable determines the number of actions available
         * at any map state, and therefore the number of Q values in each entry
         * of the Q-table.
         */
        int actionRange;

        // E-GREEDY Q-LEARNING SPECIFIC VARIABLES
        /**
         * for e-greedy Q-learning, when taking an action a random number is
         * checked against the explorationChance variable: if the number is
         * below the explorationChance, then exploration takes place picking
         * an action at random. Note that the explorationChance is not a final
         * because it is customary that the exploration chance changes as the
         * training goes on.
         */
        float explorationChance = 0.4f;

        /**
         * the discount factor is saved as the gammaValue variable. The
         * discount factor determines the importance of future rewards.
         * If the gammaValue is 0 then the AI will only consider immediate
         * rewards, while with a gammaValue near 1 (but below 1) the AI will
         * try to maximize the long-term reward even if it is many moves away.
         */
        float gammaValue = 0.9f;

        /**
         * the learningRate determines how new information affects accumulated
         * information from previous instances. If the learningRate is 1, then
         * the new information completely overrides any previous information.
         * Note that the learningRate is not a final because it is
         * customary that the learningRate changes as the
         * training goes on.
         */
        float learningRate = 0.15f;

        //PREVIOUS STATE AND ACTION VARIABLES
        /**
         * Since in Q-learning the updates to the Q values are made ONE STEP
         * LATE, the state of the world when the action resulting in the reward
         * was made must be stored.
         */
        public char[] prevState;

        /**
         * Since in Q-learning the updates to the Q values are made ONE STEP
         * LATE, the index of the action which resulted in the reward must be
         * stored.
         */
        int prevAction;

        /**
         * Q table constructor, initiates variables.
         * @param the number of actions available at any map state
         */
        public QTable(int actionRange)
        {
            randomGenerator = new System.Random();
            this.actionRange = actionRange;
            table = new Dictionary<string, float[]>();
        }

        /**
         * For this example, the getNextAction function uses an e-greedy
         * approach, having exploration happen if the exploration chance
         * is rolled.
         *
         * @param the current map (state)
         * @return the action to be taken by the calling program
         */
        public int getNextAction(char[] map)
        {
            prevState = (char[])map.Clone();
            if (randomGenerator.NextDouble() < explorationChance)
            {
                prevAction = explore();
            }
            else
            {
                prevAction = getBestAction(map);
            }

            return prevAction;
        }

        /**
         * The getBestAction function uses a greedy approach for finding
         * the best action to take. Note that if all Q values for the current
         * state are equal (such as all 0 if the state has never been visited
         * before), then getBestAction will always choose the same action.
         * If such an action is invalid, this may lead to a deadlock as the
         * map state never changes: for situations like these, exploration
         * can get the algorithm out of this deadlock.
         *
         * @param the current map (state)
         * @return the action with the highest Q value
         */
        int getBestAction(char[] map)
        {
            // ToDo
            string state = map.ToString();
            float[] qValues = table[state];
            // value for each action {0, 1, 2, 3} possible in the state
            // ex: {0.2, 0.1, 0.2, ...}

            int bestAction = 0;
            float bestQ = float.MinValue;
            for (int i = 0; i < qValues.Length; i++)
            {
                if (bestQ < qValues[i])
                {
                    bestAction = i;
                    bestQ = qValues[i];
                }
            }

            return bestAction;
        }

        /**
         * The explore function is called for e-greedy algorithms.
         * It can choose an action at random from all available,
         * or can put more weight towards actions that have not been taken
         * as often as the others (most unknown).
         *
         * @return index of action to take
         */
        int explore()
        {
            // ToDo
            return randomGenerator.Next(0, actionRange);
        }

        /**
         * The updateQvalue is the heart of the Q-learning algorithm. Based on
         * the reward gained by taking the action prevAction while being in the
         * state prevState, the updateQvalue must update the Q value of that
         * {prevState, prevAction} entry in the Q table. In order to do that,
         * the Q value of the best action of the current map state must also
         * be calculated.
         *
         * @param reward at the current map state
         * @param the current map state (for finding the best action of the
         * current map state)
         */
        public void updateQvalue(int reward, char[] map)
        {
            // ToDo
            var bestAction = getBestAction(map);
            float[] qValues = getActionsQValues(map);

            // Q-learning update rule
            float updatedQValue = qValues[prevAction] +
                                  learningRate * (reward + gammaValue * qValues[bestAction] - qValues[prevAction]);

            // Update the Q-value for the previous state and action
            qValues[prevAction] = updatedQValue;
        }

        /**
         * This helper function is used for entering the map state into the
         * HashMap
         * @param map
         * @return String used as a key for the HashMap
         */
        String getMapString(char[] map)
        {
            String result = "";
            for (int x = 0; x < map.Length; x++)
            {
                result += "" + map[x];
            }

            return result;
        }

        /**
         * The getActionsQValues function returns an array of Q values for
         * all the actions available at any state. Note that if the current
         * map state does not already exist in the Q table (never visited
         * before), then it is initiated with Q values of 0 for all of the
         * available actions.
         *
         * @param the current map (state)
         * @return an array of Q values for all the actions available at any state
         */
        public float[] getActionsQValues(char[] map)
        {
            float[] actions = getValues(map);
            if (actions == null)
            {
                float[] initialActions = new float[actionRange];
                for (int i = 0; i < actionRange; i++)
                    initialActions[i] = 0;
                table.Add(getMapString(map), initialActions);
                return initialActions;
            }

            return actions;
        }

        /**
         * printQtable is included for debugging purposes and uses the
         * action labels used in the maze class (even though the Qtable
         * is written so that it can more generic).
         */
        void printQtable()
        {
            foreach (KeyValuePair<string, float[]> entry in table)
            {
                char[] key = entry.Key.ToCharArray();
                float[] values = entry.Value;

                Debug.Log(key[0] + "" + key[1] + "" + key[2]);
                Debug.Log("  UP   RIGHT  DOWN  LEFT");
                Debug.Log(key[3] + "" + key[4] + "" + key[5]);
                Debug.Log(": " + values[0] + "   " + values[1] + "   " + values[2] + "   " + values[3]);
                Debug.Log(key[6] + "" + key[7] + "" + key[8]);
            }
        }

        /**
         * Helper function to find the Q-values of a given map state.
         *
         * @param the current map (state)
         * @return the Q-values stored of the Qtable entry of the map state, otherwise null if it is not found
         */
        float[] getValues(char[] map)
        {
            String mapKey = getMapString(map);
            float[] value;
            if (table.TryGetValue(mapKey, out value))
            {
                return value;
            }

            return null;
        }
    }
}