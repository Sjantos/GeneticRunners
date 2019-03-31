using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TMPro;

public class GeneticAlgorithm : MonoBehaviour
{
    public bool canRun = false;
    [SerializeField] GameObject creaturePrefab;
    [SerializeField] int populationSize = 100;
    [SerializeField] int creatureSize = 7;
    [SerializeField] int tournamentSize = 3;
    [SerializeField] float mutationChance = 0.2f;
    [SerializeField] float runTime = 10f;
    [SerializeField] TextMeshProUGUI desc;

    private float[] generationResults;
    private float[] onlyPositive;

    private int parentsCount;
    private int childCount;
    private CreatureData[] parents;
    private CreatureData[] childrens;
    private Creature[] currentPopulation;

    private Vector3Bounds positionBounds;

    private float timer = 0f;
    private float bestEverSolution = -100f;
    private int generationCounter = 0;

    private void Start()
    {
        positionBounds = new Vector3Bounds(-5f, 5f, -2.5f, 2.5f, 0f, 0f);
        //Parents and childrens make new population as 1/3 population size for parents and 2/3 for childrens
        while (populationSize % 3 != 0)
            populationSize++;
        parentsCount = populationSize / 3;
        childCount = 2 * parentsCount;
        currentPopulation = new Creature[populationSize];
        parents = new CreatureData[parentsCount];
        childrens = new CreatureData[childCount];

        MakeFirstRandomPopulation();
    }

    private void FixedUpdate()
    {
        if (!canRun)
            return;
        timer += Time.fixedDeltaTime;
        if(timer >= runTime)
        {
            timer = 0f;
            generationResults = new float[currentPopulation.Length];
            for (int i = 0; i < currentPopulation.Length; i++)
                generationResults[i] = currentPopulation[i].GetAvgPosition().x;
            var best = generationResults.Max();
            if (best > bestEverSolution)
                bestEverSolution = best;
            desc.text = string.Format("Generation {0}\nMaxDistance {1}\nCurrent {2}", generationCounter++, bestEverSolution, best);
            Selection();
            Crossover();
            Mutation();
            MakePopulation();
        }
    }

    private void Selection()
    {
        //for (int i = 0; i < parents.Length; i++)
        //    parents[i] = currentPopulation[Roulette()].GetData();
        for (int i = 0; i < parents.Length; i++)
            parents[i] = currentPopulation[Tournament()].GetData();
    }

    private void Crossover()
    {
        for (int i = 0; i < childrens.Length; i += 2)
        {
            //Make childrens, pick randomly two parents and make two children (crossing p1+p2  and p2+p1)
            int p1Index = Random.Range(0, parents.Length), p2Index = -1;
            do
            {
                //Make sure it will be two different indexes
                p2Index = Random.Range(0, parents.Length);
            } while (p1Index == p2Index);
            CreatureData p1 = parents[p1Index], p2 = parents[p2Index];
            var ch1 = new CreatureData()
            {
                positions = GeneticOperators.Crossover(p1.positions, p2.positions, CrossoverMethod.OnePoint),
                structure = GeneticOperators.Crossover(p1.structure, p2.structure, CrossoverMethod.OnePoint),
                timers = GeneticOperators.Crossover(p1.timers, p2.timers, CrossoverMethod.OnePoint)
            };
            var ch2 = new CreatureData()
            {
                positions = GeneticOperators.Crossover(p2.positions, p1.positions, CrossoverMethod.OnePoint),
                structure = GeneticOperators.Crossover(p2.structure, p1.structure, CrossoverMethod.OnePoint),
                timers = GeneticOperators.Crossover(p2.timers, p1.timers, CrossoverMethod.OnePoint)
            };
            childrens[i] = ch1;
            childrens[i + 1] = ch2;
        }
    }

    private void Mutation()
    {
        for (int i = 0; i < childrens.Length; i++)
        {
            if(Random.Range(0f, 1f) < mutationChance)
            {
                childrens[i].positions = GeneticOperators.PositionRandomMutation(childrens[i].positions, positionBounds);
                childrens[i].structure = GeneticOperators.Mutation(childrens[i].structure, MutationMethod.Swap);
                childrens[i].timers = GeneticOperators.Mutation(childrens[i].timers, MutationMethod.Swap);
            }
        }
    }

    private void MakePopulation()
    {
        //This method is highly unoptimized but let it be for now (for simplicity)
        for (int i = 0; i < currentPopulation.Length; i++)
        {
            DestroyImmediate(currentPopulation[i].gameObject);
        }
        int parentIndex = 0, childIndex = 0;
        for (int i = 0; i < currentPopulation.Length; i++)
        {
            if (i < parents.Length)
                currentPopulation[i] = InstantiateCreature(parents[parentIndex++]);
            else
                currentPopulation[i] = InstantiateCreature(childrens[childIndex++]);
        }
    }

    private int Tournament()
    {
        int[] participants = new int[tournamentSize];
        for (int i = 0; i < tournamentSize; i++)
            participants[i] = Random.Range(0, generationResults.Length);

        int bestSolutionIndex = 0;
        for (int i = 1; i < tournamentSize; i++)
        {
            if(generationResults[participants[i]] > generationResults[participants[bestSolutionIndex]])
                bestSolutionIndex = i;
        }
        Debug.Log("Tournament bestIndex=" + participants[bestSolutionIndex]);
        return participants[bestSolutionIndex];
    }

    private int Roulette()
    {
        onlyPositive = new float[generationResults.Length];
        var minOfMin = generationResults.Min();
        for (int i = 0; i < generationResults.Length; i++)
        {
            if (i == 0)
                onlyPositive[i] = generationResults[i] + minOfMin;
            else
                onlyPositive[i] = onlyPositive[i - 1] + generationResults[i] + minOfMin;
        }
        var rand = Random.Range(0f, onlyPositive[onlyPositive.Length - 1]);
        for (int i = 0; i < onlyPositive.Length; i++)
        {
            if (onlyPositive[i] >= rand)
                return i;
        }

        return onlyPositive.Length - 1;
    }

    private void MakeFirstRandomPopulation()
    {
        for (int i = 0; i < populationSize; i++)
        {
            var data = new CreatureData();
            data.positions = RandomExtension.RandomArray(creatureSize, positionBounds);
            data.structure = RandomExtension.RandomArray(creatureSize);
            data.timers = RandomExtension.RandomArray(creatureSize, 0.1f, 0.4f);
            currentPopulation[i] = InstantiateCreature(data);
        }
        canRun = true;
    }

    private Creature InstantiateCreature(CreatureData data)
    {
        var creatureObject = Instantiate(creaturePrefab, new Vector3(0f, 2f, 0f), Quaternion.identity);
        var creature = creatureObject.GetComponent<Creature>();
        creature.CreateRandom(data);
        creatureObject.SetActive(false);
        creatureObject.SetActive(true);

        return creature;
    }

    private Creature InstantiateCreature(CreatureData c1, CreatureData c2, CrossoverMethod crossoverMethod, MutationMethod mutationMethod)
    {
        var creatureObject = Instantiate(creaturePrefab, new Vector3(0f, 2f, 0f), Quaternion.identity);
        var creature = creatureObject.GetComponent<Creature>();
        creature.CreateFromParents(c1, c2, crossoverMethod, mutationMethod);
        creatureObject.SetActive(false);
        creatureObject.SetActive(true);

        return creature;
    }
}
