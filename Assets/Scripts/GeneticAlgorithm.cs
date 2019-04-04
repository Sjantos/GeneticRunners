using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TMPro;
using System.IO;

public enum SelectionMethod
{
    Tournament,
    Roulette,
    Ranking
}

public class Triple<T>
{
    public T item1;
    public T item2;
    public T item3;
}

public class GeneticAlgorithm : MonoBehaviour
{
    public bool canRun = false;
    [SerializeField] GameObject creaturePrefab;
    [SerializeField] SelectionMethod selectionMethod = SelectionMethod.Tournament;
    [SerializeField] CrossoverMethod crossoverMethod = CrossoverMethod.OnePoint;
    [SerializeField] MutationMethod mutationMethod = MutationMethod.Swap;
    [SerializeField] int populationSize = 150;
    [SerializeField] int creatureSize = 7;
    [SerializeField] int tournamentSize = 3;
    [SerializeField] float mutationChance = 0.2f;
    [SerializeField] float runTime = 10f;
    [SerializeField] TextMeshProUGUI desc;

    private float[] generationResults;
    private float[] onlyPositive;
    private float[] generationResultRanking;

    private int parentsCount;
    private int childCount;
    private CreatureData[] parents;
    private CreatureData[] childrens;
    private Creature[] currentPopulation;

    private Vector3Bounds positionBounds;

    private float timer = 0f;
    private float bestEverSolution = -100f;
    private int generationCounter = 0;
    private List<Triple<float>> saveData = new List<Triple<float>>();

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

    int generationCount = 0;
    int mutationCount = 1;
    private void FixedUpdate()
    {
        if (!canRun)
            return;
        #region single algorithm run
        if (mutationCount < 5)
        {
            //300 generations loop
            if (generationCount < 250)
            {
                timer += Time.fixedDeltaTime;
                if (timer >= runTime)
                {
                    timer = 0f;
                    generationResults = new float[currentPopulation.Length];
                    for (int i = 0; i < currentPopulation.Length; i++)
                        generationResults[i] = currentPopulation[i].GetAvgPosition().x;
                    PunishAnomalyAndSaveResult();
                    var best = generationResults.Max();
                    if (best > bestEverSolution)
                        bestEverSolution = best;
                    desc.text = string.Format("Generation {0}\nMaxDistance {1}\nCurrent {2}", generationCounter++, bestEverSolution, best);
                    if (generationCount < 249) // don't do this for last iteration
                    {
                        Selection();
                        Crossover();
                        Mutation();
                        MakePopulation();
                        generationCount++;
                    }
                    else
                    {
                        var filename = string.Format("sel_{0}_cross_{1}_mut_{2}_chance_{3}", selectionMethod, crossoverMethod, mutationMethod, mutationChance);
                        SaveData(filename);
                        Restart();
                        MakeFirstRandomPopulation();
                        generationCount = 0;
                        if(mutationCount < 4)
                        {
                            mutationChance += 0.2f;
                            mutationCount++;
                        }
                        else 
                        {
                            if (mutationMethod == MutationMethod.Scramble)
                            {
                                if (crossoverMethod == CrossoverMethod.TwoPoint)
                                {
                                    if (selectionMethod == SelectionMethod.Ranking)
                                    {
                                        canRun = false;
                                        Debug.Break();
                                    }
                                    else
                                    {
                                        selectionMethod = (SelectionMethod)((int)selectionMethod + 1);
                                        crossoverMethod = CrossoverMethod.OnePoint;
                                        mutationMethod = MutationMethod.Swap;
                                        mutationCount = 1;
                                        mutationChance = 0.2f;
                                    }
                                }
                                else
                                {
                                    crossoverMethod = (CrossoverMethod)((int)crossoverMethod + 1);
                                    mutationMethod = MutationMethod.Swap;
                                    mutationCount = 1;
                                    mutationChance = 0.2f;
                                }
                            }
                            else
                            {
                                mutationMethod = (MutationMethod)((int)mutationMethod + 1);
                                mutationCount = 1;
                                mutationChance = 0.2f;
                            }
                        }
                    }
                }
            } 
        }
        #endregion
    }

    private void Restart()
    {
        saveData.Clear();
        for (int i = 0; i < currentPopulation.Length; i++)
        {
            DestroyImmediate(currentPopulation[i].gameObject);
        }
        currentPopulation = new Creature[populationSize];
        parents = new CreatureData[parentsCount];
        childrens = new CreatureData[childCount];
        generationResults = null;
        onlyPositive = null;
        generationResultRanking = null;
}

    private void PunishAnomalyAndSaveResult()
    {
        double average = generationResults.Average();
        double sumOfSquaresOfDifferences = generationResults.Select(val => (val - average) * (val - average)).Sum();
        double sd = System.Math.Sqrt(sumOfSquaresOfDifferences / generationResults.Length);
        var min = (float)(average - 3f * sd);
        var max = (float)(average + 3f * sd);

        var minToSave = generationResults.Where(p => p >= min && p <= max).Min();
        var avgToSave = generationResults.Where(p => p >= min && p <= max).Average();
        var maxToSave = generationResults.Where(p => p >= min && p <= max).Max();
        saveData.Add(new Triple<float>()
        {
            item1 = minToSave,
            item2 = avgToSave,
            item3 = maxToSave
        });

        for (int i = 0; i < generationResults.Length; i++)
        {
            if (generationResults[i] < min || generationResults[i] > max)
                generationResults[i] = min;
        }
    }

    private void Selection()
    {
        switch (selectionMethod)
        {
            case SelectionMethod.Tournament:
                for (int i = 0; i < parents.Length; i++)
                    parents[i] = currentPopulation[Tournament()].GetData();
                break;
            case SelectionMethod.Roulette:
                PrepareRoulette();
                for (int i = 0; i < parents.Length; i++)
                    parents[i] = currentPopulation[Roulette()].GetData();
                break;
            case SelectionMethod.Ranking:
                PrepareRanking();
                for (int i = 0; i < parents.Length; i++)
                    parents[i] = currentPopulation[Ranking()].GetData();
                break;
            default:
                break;
        }        
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
                positions = GeneticOperators.Crossover(p1.positions, p2.positions, crossoverMethod),
                structure = GeneticOperators.Crossover(p1.structure, p2.structure, crossoverMethod),
                timers = GeneticOperators.Crossover(p1.timers, p2.timers, crossoverMethod)
            };
            var ch2 = new CreatureData()
            {
                positions = GeneticOperators.Crossover(p2.positions, p1.positions, crossoverMethod),
                structure = GeneticOperators.Crossover(p2.structure, p1.structure, crossoverMethod),
                timers = GeneticOperators.Crossover(p2.timers, p1.timers, crossoverMethod)
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
                childrens[i].structure = GeneticOperators.Mutation(childrens[i].structure, mutationMethod);
                childrens[i].timers = GeneticOperators.Mutation(childrens[i].timers, mutationMethod);
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

    private void PrepareRoulette()
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
    }

    private int Roulette()
    {
        var rand = Random.Range(0f, onlyPositive[onlyPositive.Length - 1]);
        for (int i = 0; i < onlyPositive.Length; i++)
        {
            if (onlyPositive[i] >= rand)
                return i;
        }

        return onlyPositive.Length - 1;
    }

    private void PrepareRanking()
    {
        var alreadyAdded = new int[generationResults.Length];
        generationResultRanking = new float[generationResults.Length];
        var totalDiv = (float)(((populationSize + 1) * populationSize) / 2);
        for (int j = generationResults.Length - 1; j >=0 ; j--)
        {
            int bestIndex = 0;
            for (int b = 0; b < generationResults.Length; b++)
            {
                if(!alreadyAdded.Contains(b))
                {
                    bestIndex = b;
                    break;
                }
            }

            for (int i = 0; i < generationResults.Length; i++)
            {
                if (generationResults[i] > generationResults[bestIndex] && !alreadyAdded.Contains(i))
                    bestIndex = i;
            }
            alreadyAdded[j] = bestIndex;
        }
        for (int i = 0; i < generationResultRanking.Length; i++)
        {
            generationResultRanking[i] = ((float)(alreadyAdded[i] + 1)) / totalDiv;
        }
    }

    private int Ranking()
    {
        var rand = Random.Range(0f, generationResultRanking[generationResultRanking.Length - 1]);
        for (int i = 0; i < generationResultRanking.Length; i++)
        {
            if (generationResultRanking[i] >= rand)
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

    public void SaveData(string filename)
    {
        var builder = new System.Text.StringBuilder();
        foreach (var item in saveData)
        {
            builder.AppendLine(string.Format("{0};{1};{2}", item.item1.ToString().Replace('.', ','), item.item2.ToString().Replace('.', ','), item.item3.ToString().Replace('.', ',')));
        }

        using (var w = new StreamWriter(@"A:\GA\" + (filename + ".csv"), false))
        {
            w.WriteLine(builder.ToString());
            w.Flush();
            w.Close();
        }

        saveData.Clear();
    }
}
