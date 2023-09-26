using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Exercise
{
    public class GeneticAlgorithm : MonoBehaviour
    {
        private void Awake()
        {
            StartEvolution();
        }
        // --- constants
        public static int CHROMOSOME_SIZE = 10;
        public static int POPULATION_SIZE = 500;
        public static int MAX_GENERATIONS = 1000;

        // --- variables:

        /**
         * The population contains an ArrayList of genes (the choice of arrayList over
         * a simple array is due to extra functionalities of the arrayList, such as sorting)
         */
        List<GeneA> mPopulation;

        // --- functions:

        /**
         * Creates the starting population of Gene classes, whose chromosome contents are random
         * @param size: The size of the popultion is passed as an argument from the main class
         */
        public void InitializeAlgorithm(int size)
        {
            // initialize the arraylist and each gene's initial weights HERE
            mPopulation = new List<GeneA>();
            for (int i = 0; i < size; i++)
            {
                GeneA g = new GeneA();
                g.RandomizeChromosome();
                mPopulation.Add(g);
            }
        }

        /**
         * For all members of the population, runs a heuristic that evaluates their fitness
         * based on their phenotype. The evaluation of this problem's phenotype is fairly simple,
         * and can be done in a straightforward manner. In other cases, such as agent
         * behavior, the phenotype may need to be used in a full simulation before getting
         * evaluated (e.g based on its performance)
         */
        public void EvaluateGeneration()
        {
            foreach (GeneA g in mPopulation)
            {
                g.CalculateFitness();
            }
        }

        /**
         * With each gene's fitness as a guide, chooses which genes should mate and produce offspring.
         * The offspring are added to the population, replacing the previous generation's Genes either
         * partially or completely. The population size, however, should always remain the same.
         * If you want to use mutation, this function is where any mutation chances are rolled and mutation takes place.
         */
        public void ProduceNextGeneration()
        {
            // use one of the offspring techniques suggested in class (also applying any mutations) HERE
            List<GeneA> newPopulation = new List<GeneA>();
            var half = POPULATION_SIZE / 2;
            
            mPopulation = mPopulation.OrderByDescending(x => x.GetFitness()).ToList();
            newPopulation.AddRange(mPopulation.Skip(0).Take(half));

            for (int i = 0; i < half; i += 2){
                GeneA parent1 = mPopulation[i];
                GeneA parent2 = mPopulation[i+1];
                var children = parent1.Reproduce(parent2);
                newPopulation.Add((GeneA) children[0]);
                newPopulation.Add((GeneA) children[1]);
            }
            mPopulation = newPopulation;
        }

        // accessors
        /**
         * @return the size of the population
         */
        public int Size() { return mPopulation.Count; }

        /**
         * Returns the Gene at position <b>index</b> of the mPopulation arrayList
         * @param index: the position in the population of the Gene we want to retrieve
         * @return the Gene at position <b>index</b> of the mPopulation arrayList
         */
        public GeneA GetGene(int index) { return mPopulation[index]; }

        // Genetic Algorithm maxA testing method
        public void StartEvolution()
        {
            // Initializing the population (we chose 500 genes for the population,
            // but you can play with the population size to try different approaches)
            InitializeAlgorithm(POPULATION_SIZE);
            int generationCount = 0;
            // For the sake of this sample, evolution goes on forever.
            // If you wish the evolution to halt (for instance, after a number of
            //   generations is reached or the maximum fitness has been achieved),
            //   this is the place to make any such checks
            while (generationCount < MAX_GENERATIONS)
            {
                // --- evaluate current generation:
                EvaluateGeneration();
                // --- print results here:
                // we choose to print the average fitness,
                // as well as the maximum and minimum fitness
                // as part of our progress monitoring
                float avgFitness = 0;
                float minFitness = float.PositiveInfinity;
                float maxFitness = float.NegativeInfinity;
                string bestIndividual = "";
                string worstIndividual = "";
                for (int i = 0; i < Size(); i++)
                {
                    float currFitness = GetGene(i).GetFitness();
                    avgFitness += currFitness;
                    if (currFitness < minFitness)
                    {
                        minFitness = currFitness;
                        worstIndividual = GetGene(i).GetPhenotype();
                    }
                    if (currFitness > maxFitness)
                    {
                        maxFitness = currFitness;
                        bestIndividual = GetGene(i).GetPhenotype();
                    }
                }
                if (Size() > 0) { avgFitness = avgFitness / Size(); }
                string output = "Generation: " + generationCount + " Population: " + Size();
                output += "\t AvgFitness: " + avgFitness;
                output += "\t MinFitness: " + minFitness + " (" + worstIndividual + ")";
                output += "\t MaxFitness: " + maxFitness + " (" + bestIndividual + ")";
                Debug.Log(output);
                // produce next generation:
                ProduceNextGeneration();
                generationCount++;
            }
        }
    }
}
