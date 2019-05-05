using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public enum CrossoverMethod
{
    //PMX, //not working for positions, they are not unique
    OnePoint,
    TwoPoint
}

public enum MutationMethod
{
    None,
    Swap,
    Inversion,
    Scramble
}

[System.Serializable]
public class CreatureData
{
    public Vector3[] positions;
    public int[] structure;
    public float[] timers;
}

public class Creature : MonoBehaviour
{
    public static Vector3Bounds positionBounds = new Vector3Bounds(-5f, 5f, -2.5f, 2.5f, 0f, 0f);
    public static Vector3Bounds randomStepBounds = new Vector3Bounds(-0.5f, 0.5f, -0.25f, 0.25f, 0f, 0f);
    public static float minMuscleTime = 0.1f;
    public static float maxMuscleTime = 0.4f;

    [SerializeField] GameObject nodePrefab;
    [SerializeField] Node[] nodes;

    private Vector3[] positions;
    private int[] structure;
    private float[] timers;

    //data used to specify firefly-specific random move (one for all live of one creature, modified only by parameter - randomMoveScale)
    private Vector3[] positions2;
    private float[] timers2;

    public CreatureData GetData()
    {
        return new CreatureData() { positions = this.positions, structure = this.structure, timers = this.timers };
    }

    public Vector3 GetAvgPosition()
    {
        var pos = Vector3.zero;
        foreach (var node in nodes)
            pos += node.transform.position;
        pos /= nodes.Length;
        return pos;
    }

    public void CreateFromParents(CreatureData c1, CreatureData c2, CrossoverMethod crossoverMethod, MutationMethod mutationMethod)
    {
        if (c1.positions.Length != c1.positions.Length)
        {
            Debug.LogError("Different creature size crossing is not supported");
            return;
        }

        var crossoveredStructure = GeneticOperators.Crossover(c1.structure, c2.structure, crossoverMethod);
        var crossoveredPositions = GeneticOperators.Crossover(c1.positions, c2.positions, crossoverMethod);
        var crossoveredTimers = GeneticOperators.Crossover(c1.timers, c2.timers, crossoverMethod);
        structure = GeneticOperators.Mutation(crossoveredStructure, mutationMethod);
        positions = GeneticOperators.Mutation(crossoveredPositions, mutationMethod);
        timers = GeneticOperators.Mutation(crossoveredTimers, mutationMethod);
        nodes = new Node[positions.Length];
        for (int i = 0; i < positions.Length; i++)
        {
            var newNode = Instantiate(nodePrefab, transform);
            newNode.transform.localPosition = positions[i];
            nodes[i] = newNode.GetComponent<Node>();
        }
        CreateConnectionsInStructure();
        foreach (var node in nodes)
        {
            node.Enable();
        }
    }

    public void CreateRandom(CreatureData data, Vector3[] positions2 = null, float[] timers2 = null)
    {
        this.structure = data.structure;
        this.positions = data.positions;
        this.timers = data.timers;
        CreateAndPositionNodes();
        CreateConnectionsInStructure();
        foreach (var node in nodes)
        {
            node.Enable();
        }

        if (positions2 == null)
        {
            this.positions2 = new Vector3[data.positions.Length];
            this.positions2 = RandomExtension.RandomArray(data.positions.Length, randomStepBounds); 
        }
        else
        {
            this.positions2 = positions2;
        }
        if (timers2 == null)
        {
            this.timers2 = new float[data.timers.Length];
            this.timers2 = RandomExtension.RandomArray(data.timers.Length, 0.25f * minMuscleTime, 0.25f * maxMuscleTime); 
        }
        else
        {
            this.timers2 = timers2;
        }
    }

    private void CreateAndPositionNodes()
    {
        nodes = new Node[positions.Length];
        for (int i = 0; i < positions.Length; i++)
        {
            var newNode = Instantiate(nodePrefab, transform);
            newNode.transform.localPosition = positions[i];
            nodes[i] = newNode.GetComponent<Node>();
        }
    }

    private void CreateConnectionsInStructure()
    {
        for (int i = structure.Length - 1; i > 0; i--)
        {
            var parentIndex = (i - 1) / 2;
            if (parentIndex >= 0)
            {
                nodes[parentIndex].AddConnectedNode(nodes[i], timers[i]);
            }
        }
    }

    public void UpdateCreatureData(CreatureData data)
    {
        this.positions = data.positions;
        this.structure = data.structure;
        this.timers = data.timers;
    }

    public CreatureData ApplyRandomStep(CreatureData data, float t)
    {
        var posCount = data.positions.Length < this.positions2.Length ? data.positions.Length : this.positions2.Length;
        for (int i = 0; i < posCount; i++)
            data.positions[i] += t * this.positions2[i];
        var timCount = data.timers.Length < this.timers2.Length ? data.timers.Length : this.timers2.Length;
        for (int i = 0; i < timCount; i++)
            data.timers[i] += t * this.timers2[i];

        return GetData();
    }

    public Vector3[] StepPositionsCopy()
    {
        return this.positions2.Clone() as Vector3[];
    }

    public float[] StepTimersCopy()
    {
        return this.timers2.Clone() as float[];
    }
}
