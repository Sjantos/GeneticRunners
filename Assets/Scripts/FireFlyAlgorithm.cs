using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class FireFlyAlgorithm : MonoBehaviour
{
    [SerializeField] GameObject creaturePrefab;
    [SerializeField] int populationSize = 150;
    [SerializeField] int creatureSize = 7;
    [SerializeField] int iterationCount = 100;
    [SerializeField] int currentIteration = 0;
    [SerializeField] float runTime = 10f;
    [SerializeField] float lightAbsorption;
    [SerializeField] float randomMoveScale = 1f;
    [SerializeField] float originLight = 1f;

    public List<float> max;
    public List<float> avg;
    public List<float> min;
    public LineRenderer maxLine;
    public LineRenderer avgLine;
    public LineRenderer minLine;
    public Vector3 graphStart = new Vector3(10f, 10f, 0f);

    public Creature[] population;
    public float[] lightIntensities;

    private Vector3Bounds positionBounds = new Vector3Bounds(-5f, 5f, -2.5f, 2.5f, 0f, 0f);
    private IEnumerator algorithmCoroutine;


    private void Start()
    {
        max = new List<float>();
        avg = new List<float>();
        min = new List<float>();
        algorithmCoroutine = RunAlgorithm();
        StartCoroutine(algorithmCoroutine);
    }


    public IEnumerator RunAlgorithm()
    {
        SetUpAlgorithm();
        yield return new WaitForSeconds(runTime); //make first run to collect results (light intensity)
        for (int i = 0; i < population.Length; i++)
        {
            lightIntensities[i] = Vector3.Distance(Vector3.zero, population[i].GetAvgPosition());
            population[i].gameObject.SetActive(false);
        }
        PunishAnomaly();
        max.Add(lightIntensities.Max());
        avg.Add(lightIntensities.Average());
        min.Add(lightIntensities.Min());
        UpdateGraph();

        while (currentIteration < iterationCount)
        {
            for (int i = 0; i < population.Length; i++)
            {
                var modifiedCreaturesIndexes = new List<int>();
                for (int j = 0; j < population.Length; j++)
                {
                    if(lightIntensities[i] > lightIntensities[j])
                    {
                        modifiedCreaturesIndexes.Add(j);
                        var oldLight = lightIntensities[j];
                        Debug.Log(string.Format("i({0})={1} > j({2})={3}", i, lightIntensities[i], j, lightIntensities[j]));
                        var distance = FireFlyOperators.Distance(population[i].GetData(), population[j].GetData());
                        var param = originLight / (1 + lightAbsorption * distance * distance); //wzor
                        var timersStepCopy = population[j].StepTimersCopy();
                        var positionsStepCopy = population[j].StepPositionsCopy();
                        var newData = FireFlyOperators.MoveTowardsOther(population[j].GetData(), population[i].GetData(), param);
                        newData = population[j].ApplyRandomStep(newData, randomMoveScale);
                        DestroyImmediate(population[j].gameObject);
                        population[j] = InstantiateCreature(newData, positionsStepCopy, timersStepCopy);
                    }
                }
                yield return Wait();
                for (int j = 0; j < modifiedCreaturesIndexes.Count; j++)
                {
                    population[modifiedCreaturesIndexes[j]].gameObject.SetActive(false);
                    lightIntensities[modifiedCreaturesIndexes[j]] = Vector3.Distance(Vector3.zero, population[modifiedCreaturesIndexes[j]].GetAvgPosition());
                    PunishAnomaly(modifiedCreaturesIndexes[j]);
                    //Debug.Log("improvement=" + (lightIntensities[i] - oldLight)); 
                }
            }
            max.Add(lightIntensities.Max());
            avg.Add(lightIntensities.Average());
            min.Add(lightIntensities.Min());
            UpdateGraph();

            currentIteration++;
        }
    }

    private IEnumerator Wait()
    {
        var t = 0f;
        while(t < runTime)
        {
            t += Time.fixedDeltaTime;
            yield return new WaitForEndOfFrame();
        }
    }

    private void SetUpAlgorithm()
    {
        if (population != null && population.Length > 0)
        {
            for (int i = 0; i < population.Length; i++)
                Destroy(population[i].gameObject); 
        }
        population = new Creature[populationSize];
        lightIntensities = new float[populationSize];
        for (int i = 0; i < population.Length; i++)
        {
            var newCreatureData = new CreatureData()
            {
                positions = RandomExtension.RandomArray(creatureSize, positionBounds),
                structure = RandomExtension.RandomArray(creatureSize),
                timers = RandomExtension.RandomArray(creatureSize, 0.1f, 0.4f)
            };
            population[i] = InstantiateCreature(newCreatureData);
        }
    }

    private void PunishAnomaly()
    {
        double average = lightIntensities.Average();
        double sumOfSquaresOfDifferences = lightIntensities.Select(val => (val - average) * (val - average)).Sum();
        double sd = System.Math.Sqrt(sumOfSquaresOfDifferences / lightIntensities.Length);
        var min = (float)(average - 2f * sd);
        var max = (float)(average + 2f * sd);

        for (int i = 0; i < lightIntensities.Length; i++)
        {
            if (lightIntensities[i] < min || lightIntensities[i] > max)
                lightIntensities[i] = min;
        }
    }

    private void PunishAnomaly(int index)
    {
        double average = lightIntensities.Average();
        double sumOfSquaresOfDifferences = lightIntensities.Select(val => (val - average) * (val - average)).Sum();
        double sd = System.Math.Sqrt(sumOfSquaresOfDifferences / lightIntensities.Length);
        var min = (float)(average - 2f * sd);
        var max = (float)(average + 2f * sd);

        if (lightIntensities[index] < min || lightIntensities[index] > max)
            lightIntensities[index] = min;
    }

    private void UpdateGraph()
    {
        var minPoints = new List<Vector3>(min.Count);
        minPoints.Add(graphStart);
        for (int i = 0; i < min.Count; i++)
            minPoints.Add(graphStart + new Vector3((float)(i + 1), min[i], 0f));
        minLine.SetPositions(minPoints.ToArray());

        minPoints.Clear();
        minPoints.Add(graphStart);
        for (int i = 0; i < min.Count; i++)
            minPoints.Add(graphStart + new Vector3((float)(i + 1), avg[i], 0f));
        avgLine.SetPositions(minPoints.ToArray());

        minPoints.Clear();
        minPoints.Add(graphStart);
        for (int i = 0; i < min.Count; i++)
            minPoints.Add(graphStart + new Vector3((float)(i + 1), max[i], 0f));
        maxLine.SetPositions(minPoints.ToArray());
    }

    private Creature InstantiateCreature(CreatureData data, Vector3[] positions2 = null, float[] timers2 = null)
    {
        var creatureObject = Instantiate(creaturePrefab, new Vector3(0f, 2f, 0f), Quaternion.identity);
        var creature = creatureObject.GetComponent<Creature>();
        creature.CreateRandom(data, positions2, timers2);
        creatureObject.SetActive(false);
        creatureObject.SetActive(true);

        return creature;
    }

    private void OnGUI()
    {
        if(GUI.Button(new Rect(200, 500, 200, 100), "Firefly"))
        {
            if (algorithmCoroutine != null)
                StopCoroutine(algorithmCoroutine);
            SetUpAlgorithm();
            algorithmCoroutine = RunAlgorithm();
            StartCoroutine(algorithmCoroutine);
        }
    }
}
