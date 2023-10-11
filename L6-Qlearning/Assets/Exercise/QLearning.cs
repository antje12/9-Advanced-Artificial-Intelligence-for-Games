using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

/**
* Q Learning sample class <br/>
* <b>The goal of this code sample is for the character @ to reach the goal area G</b> <br/>
* compile using "javac QLearning.java" <br/>
* test using "java QLearning" <br/>
*
* @author A.Liapis (Original author), A. Hartzen (2015 modifications), Marco Scirea (2019 C#/Unity adaptation)
*/
namespace Exercise
{
    public class QLearning : MonoBehaviour
    {
        // --- variables
        public int size = 5;
        public int obstacles = 3;
        protected char[] map;
        char[] startingMap;
        public Visualization vs;
        [Range(0.01f, 1)] public float timestep = 0.5f;
        public bool visualizeQValues = true;

        // --- movement constants
        public static int UP = 0;
        public static int RIGHT = 1;
        public static int DOWN = 2;
        public static int LEFT = 3;

        public char[] getMap()
        {
            return (char[])map.Clone();
        }

        public void randomizeMaze()
        {
            map = new char[size * size];
            for (int i = 0; i < map.Length; i++)
                map[i] = ' ';
            // add player
            int pX = Random.Range(0, size);
            int pY = Random.Range(0, size);
            map[pX + pY * size] = '@';
            // add goal
            int gX;
            int gY;
            do
            {
                gX = Random.Range(0, size);
                gY = Random.Range(0, size);
            } while (gX == pX && gY == pY);

            map[gX + gY * size] = 'G';
            // add some obstacles
            for (int i = 0; i < obstacles; i++)
            {
                int oX;
                int oY;
                do
                {
                    oX = Random.Range(0, size);
                    oY = Random.Range(0, size);
                } while ((oX == pX && oY == pY) || (oX == gX && oY == gY));

                map[oX + oY * size] = '#';
            }

            startingMap = map;
        }

        public void resetMaze()
        {
            map = (char[])startingMap.Clone();

            vs.Setup(map, size);
        }

        public int getActionRange()
        {
            return 4;
        }

        /**
         * Returns the map state which results from an initial map state after an
         * action is applied. In case the action is invalid, the returned map is the
         * same as the initial one (no move).
         * @param action taken by the avatar ('@')
         * @param current map before the action is taken
         * @return resulting map after the action is taken
         */
        public char[] getNextState(int action, char[] map)
        {
            char[] nextMap = (char[])map.Clone();
            // get location of '@'
            int avatarIndex = getAvatarIndex(map);
            if (avatarIndex == -1)
            {
                return nextMap; // no effect
            }

            int nextAvatarIndex = getNextAvatarIndex(action, avatarIndex);
            if (nextAvatarIndex >= 0 && nextAvatarIndex < map.Length)
            {
                if (nextMap[nextAvatarIndex] != '#')
                {
                    // change the map
                    nextMap[avatarIndex] = ' ';
                    nextMap[nextAvatarIndex] = '@';
                }
            }

            return nextMap;
        }

        public char[] getNextState(int action)
        {
            char[] nextMap = (char[])map.Clone();
            // get location of '@'
            int avatarIndex = getAvatarIndex(map);
            if (avatarIndex == -1)
            {
                return nextMap; // no effect
            }

            int nextAvatarIndex = getNextAvatarIndex(action, avatarIndex);
            if (nextAvatarIndex >= 0 && nextAvatarIndex < map.Length)
            {
                if (nextMap[nextAvatarIndex] != '#')
                {
                    // change the map
                    nextMap[avatarIndex] = ' ';
                    nextMap[nextAvatarIndex] = '@';
                }
            }

            return nextMap;
        }

        public void goToNextState(int action)
        {
            map = getNextState(action);
        }

        public bool isValidMove(int action)
        {
            char[] nextMap = getNextState(action);
            return !(Enumerable.SequenceEqual(this.map, nextMap));
        }

        public bool isValidMove(int action, char[] map)
        {
            char[] nextMap = getNextState(action, map);
            return !(Enumerable.SequenceEqual(map, nextMap));
        }

        public int getAvatarIndex()
        {
            int avatarIndex = -1;
            for (int i = 0; i < map.Length; i++)
            {
                if (map[i] == '@')
                {
                    avatarIndex = i;
                }
            }

            return avatarIndex;
        }

        public int getAvatarIndex(char[] map)
        {
            int avatarIndex = -1;
            for (int i = 0; i < map.Length; i++)
            {
                if (map[i] == '@')
                {
                    avatarIndex = i;
                }
            }

            return avatarIndex;
        }

        public bool isGoalReached()
        {
            int goalIndex = -1;
            for (int i = 0; i < map.Length; i++)
            {
                if (map[i] == 'G')
                {
                    goalIndex = i;
                }
            }

            return (goalIndex == -1);
        }

        public bool isGoalReached(char[] map)
        {
            int goalIndex = -1;
            for (int i = 0; i < map.Length; i++)
            {
                if (map[i] == 'G')
                {
                    goalIndex = i;
                }
            }

            return (goalIndex == -1);
        }

        public int getNextAvatarIndex(int action, int currentAvatarIndex)
        {
            int x = currentAvatarIndex % size;
            int y = currentAvatarIndex / size;
            if (action == UP)
            {
                y--;
            }

            if (action == RIGHT)
            {
                x++;
            }
            else if (action == DOWN)
            {
                y++;
            }
            else if (action == LEFT)
            {
                x--;
            }

            if (x < 0 || y < 0 || x >= size || y >= size)
            {
                return currentAvatarIndex; // no move
            }

            return x + size * y;
        }

        public void printMap()
        {
            vs.UpdateMap(map, size);
        }

        public String getMoveName(int action)
        {
            String result = "ERROR";
            if (action == UP)
            {
                result = "UP";
            }
            else if (action == RIGHT)
            {
                result = "RIGHT";
            }
            else if (action == DOWN)
            {
                result = "DOWN";
            }
            else if (action == LEFT)
            {
                result = "LEFT";
            }

            return result;
        }

        IEnumerator runLearningLoop()
        {
            QTable q = new QTable(getActionRange());
            int moveCounter = 0;

            while (true)
            {
                // PRINT MAP
                Debug.Log("MOVE " + moveCounter);
                printMap();
                char[] prevState = (char[])map.Clone();
                // CHECK IF WON, THEN RESET
                if (isGoalReached())
                {
                    Debug.Log("GOAL REACHED IN " + moveCounter + " MOVES!");
                    resetMaze();
                    moveCounter = 0;
                }

                // DETERMINE ACTION
                int action = q.getNextAction(getMap());
                Debug.Log("MOVING: " + getMoveName(action));
                goToNextState(action);
                moveCounter++;

                // REWARDS AND ADJUSTMENT OF WEIGHTS SHOULD TAKE PLACE HERE
                // ToDo
                int reward = isGoalReached() ? 1 : 0; // Set a reward (1 if goal reached, 0 otherwise)
                q.updateQvalue(reward, getMap());

                //VISUALIZE Q UPDATES
                if (visualizeQValues)
                {
                    float[] qValues = q.getActionsQValues(q.prevState);
                    float max = float.NegativeInfinity;
                    foreach (float v in qValues)
                        if (v > max)
                            max = v;
                    int playerPos = getAvatarIndex(q.prevState);
                    vs.UpdateQRepresentation(playerPos % size, playerPos / size, max, size);
                }

                // COMMENT THE SLEEP FUNCTION IF YOU NEED FAST TRAINING WITHOUT
                // NEEDING TO ACTUALLY SEE IT PROGRESS
                yield return new WaitForSeconds(timestep);
            }
        }

        private void Start()
        {
            randomizeMaze();
            resetMaze();
            StartCoroutine(runLearningLoop());
        }
    }
}