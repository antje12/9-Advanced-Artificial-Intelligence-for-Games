using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UCT : MonoBehaviour
{
    /*
	 * Maze used to control the game
	 */
    public Map maze;

    /*
	 * rootNode is the starting point of the present state
	 */
    Node rootNode;

    /*
	 * Exploration coefficient
	 */
    public float C = (float)(1.0 / Mathf.Sqrt(2));

    /*
	 * Computational limit
	 */
    public const int maxIterations = 100;

    public MapManager mapManager;

    /**
	 * Initialize the maze game
	 */
    void Awake()
    {
        maze = new Map();
        maze.ResetMaze();
    }

    /**
	 * run the UCT search and find the optimal action for the root node state
	 * @return the int representing the best action to take 
	 * @throws InterruptedException
	 */
    public int RunUCT()
    {

        /*
         * Create root node with the present state
         */
        rootNode = new Node((char[])maze.map.Clone());

        /*
         * Apply UCT search inside computational budget limit (default=100 iterations) 
         */
        int iterations = 0;
        while (!Terminate(iterations))
        {
            iterations++;

            //TODO Implement UCT algorithm here
            Node v = TreePolicy(rootNode);
            float delta = DefaultPolicy(v);
            Backpropagate(v, delta);
        }

        /*
         * Get the action that directs to the best node
         */
        //rootNode is the one we are interested in (the state we are in now)
        //finding the child that has the best reward gives us the best action
        int bestAction = 0;
        float bestReward = 0;
        // TODO calculate which is the best action
        foreach (Node v in rootNode.children)
        {
            if (v.reward > bestReward)
            {
                bestReward = v.reward;
                bestAction = v.parentAction;
            }
        }

        return bestAction;
    }

    /**
	 * Expand the best nonterminal node with one available child. 
	 * Chose a node to expand with BestChild(C) method
	 */
    private Node TreePolicy(Node v)
    {
        //TODO implement the tree policy
        while (!TerminalState(v.state))
        {
            if (!FullyExpanded(v))
            {
                return Expand(v);
            }
            else
            {
                v = BestChild(v, C);
            }
        }

        return v;
    }

    /**
     * Check if the node is fully expanded
     * @param a Node v
     * @return
     */
    private bool FullyExpanded(Node v)
    {
        // TODO check if the node nt is fully expanded or not
        // 4 available actions
        if (v.childern.count == 4)
        {
            return true;
        }
        return false;
    }

    /**
     * Expand the node v by adding new child to it
     */
    private Node Expand(Node v)
    {
        /*
         * Choose untried action
         */
        int action = UntriedAction(v);

        /*
         * Create a child, set its fields and add it to currentNode.children
         */
        Node child = new Node(
            maze.GetNextGhostState(
                RandomGhostAction(maze.GetNextState(action, v.state)),
                maze.GetNextState(action, v.state)
                )
            );

        // TODO: add child to the children of the current node, set parent and parentAction of child, change currentNode
        v.children.add(child);
        child.parent = v;
        child.parentAction = parent.children.count-1;
        return child;
    }

    /**
     * Choose the best child according to the UCT value and return it
     * @param c Exploration coefficient
     */
    private Node BestChild(Node v, float c)
    {
        Node bestChild = null;
        
        // TODO find best child // the arg max part
        bestChild = v.childern.first();
        float score = UCTvalue(bestChild);

        foreach (Node child in v.childern)
        {
            float newScore = UCTvalue(child, C);
            if (newScore > score)
            {
                bestChild = child;
                score = newScore;
            }
        }

        return bestChild;
    }

    /**
     * Calculate UCT value of a node v
     * @param the node v you want to apply the UCT calculation on
     * @param c Exploration coefficient
     * @return
     */
    private float UCTvalue(Node v, float c)
    {
        // TODO: calculate the UCT value for the node n and return it
        throw new UnityException("You didn't implement something!");
        return 0;
    }

    /**
     * Simulation of the game. Choose random actions up until the game is over (goal reached or dead)
     * @return reward (1 for win, 0 for loss)
     */
    private float DefaultPolicy(Node v)
    {
        char[] st = (char[])v.state.Clone();
        while (!TerminalState(st))
        {
            int action = RandomAction(st);
            st = maze.GetNextState(action, st);
            int ghostAction = RandomGhostAction(st);
            st = maze.GetNextGhostState(ghostAction, st);
        }
        return maze.GetReward(st);
    }

    /**
     * Assign the received reward to every parent of the node v up to the rootNode
     * Increase the visited count of every node included in backpropagation
     * @param the starting node v
     * @param reward
     */
    private void Backpropagate(Node v, float reward)
    {
        // TODO update the reward and timesvisited of the current node and all its parents until you reach the root of the tree
        throw new UnityException("You didn't implement something!");
    }

    /**
     * Check if the state is the end of the game
     * @param state
     * @return
     */
    private bool TerminalState(char[] state)
    {
        return maze.IsGoalReached(state) || maze.IsAvatarDead(state);
    }

    /**
     * Returns the first untried action of the node
     * @param n
     * @return
     */
    private int UntriedAction(Node n)
    {
        List<int> possibleActions = new List<int> { 0, 1, 2, 3 };
        for (int k = 0; k < n.children.Count; k++)
        {
            if (possibleActions.Contains(n.children[k].parentAction))
            {
                possibleActions.Remove(n.children[k].parentAction);
            }
        }
        if (possibleActions.Count == 0)
            return -1;
        else
        {
            int randomAction = possibleActions[Random.Range(0, possibleActions.Count)];
            return randomAction;
        }
    }

    /**
     * Check if the algorithm is to be terminated, e.g. reached number of iterations limit
     * @param i
     * @return
     */
    private bool Terminate(int i)
    {
        if (i > maxIterations) return true;
        return false;
    }

    /**
     * Used in game simulation to pick random action for the agent
     * @param state st
     * @return action
     */
    private int RandomAction(char[] st)
    {
        int action = Random.Range(0, 4);
        while (!maze.IsValidMove(action, st))
        {
            action = Random.Range(0, 4);
        }
        return action;
    }

    /**
     * Used in game simulation to pick random action for the ghost
     * @param state st
     * @return action
     */
    private int RandomGhostAction(char[] st)
    {
        int action = Random.Range(0, 4);
        while (!maze.IsValidGhostMove(action, st))
        {
            action = Random.Range(0, 4);
        }
        return action;
    }

    /**
     * UCT maze solving test
     * @param args
     * @throws InterruptedException 
     */
    public IEnumerator Main()
    {
        Debug.Log("starting UCT");

        while (true)
        {
            // PRINT MAP
            maze.PrintMap();
            mapManager.ArrangeMap(maze.map);
            // CHECK IF WON OR LOST, THEN RESET
            if (maze.IsGoalReached())
            {
                Debug.Log("GOAL REACHED");
                maze.ResetMaze();
                yield return new WaitForSeconds(3);
                maze.PrintMap();
                mapManager.ArrangeMap(maze.map);
            }

            if (maze.IsAvatarDead(maze.map))
            {
                Debug.Log("AVATAR DEAD");
                maze.ResetMaze();
                yield return new WaitForSeconds(3);
                maze.PrintMap();
                mapManager.ArrangeMap(maze.map);
            }

            //FIND THE OPTIMAL ACTION VIA UTC
            int bestAction = RunUCT();

            //ADVANCE THE GAME WITH MOVES OF AGENT AND GHOST
            maze.GoToNextState(bestAction);
            int bestGhostAction = Random.Range(0, 4);
            while (!maze.IsValidGhostMove(bestGhostAction))
            {
                bestGhostAction = Random.Range(0, 4);
            }
            maze.GoToNextGhostState(bestGhostAction);

            //TRACK THE GAME VISUALY
            yield return new WaitForSeconds(1);
        }

    }

    void Start()
    {
        StartCoroutine("Main");
    }
}
