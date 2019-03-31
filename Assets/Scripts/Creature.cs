using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public enum CrossoverMethod
{
    //PMX, //not working for positions, they are not unique
    OX,
    OnePoint
}

public enum MutationMethod
{
    None,
    Swap,
    Scramble,
    Inversion
}

public class CreatureData
{
    public Vector3[] positions;
    public int[] structure;
}

public class Creature : MonoBehaviour
{
    [SerializeField] GameObject nodePrefab;
    [SerializeField] Node[] nodes;

    private Vector3[] positions;
    private int[] structure;

    public CreatureData GetData()
    {
        return new CreatureData() { positions = this.positions, structure = this.structure };
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
        structure = GeneticOperators.Mutation(crossoveredStructure, mutationMethod);
        positions = GeneticOperators.Mutation(crossoveredPositions, mutationMethod);
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

    public void CreateRandom(CreatureData data)
    {
        this.structure = data.structure;
        this.positions = data.positions;
        CreateAndPositionNodes();
        CreateConnectionsInStructure();
        foreach (var node in nodes)
        {
            node.Enable();
        }
    }

    private void CreateAndPositionNodes()
    {
        nodes = new Node[positions.Length];
        //positions = new Vector3[size];
        for (int i = 0; i < positions.Length; i++)
        {
            //var randomPosition = new Vector3(Random.Range(-5f, 5f), Random.Range(-2.5f, 2.5f), 0f);
            //positions[i] = randomPosition;
            var newNode = Instantiate(nodePrefab, transform);
            newNode.transform.localPosition = positions[i];
            nodes[i] = newNode.GetComponent<Node>();
        }
    }

    //private void CreateRandomStructure(int size)
    //{
    //    structure = new int[size];
    //    for (int i = 0; i < size; i++)
    //        structure[i] = i;
    //    RandomExtension.Shuffle<int>(structure);
    //}

    private void CreateConnectionsInStructure()
    {
        for (int i = structure.Length - 1; i > 0; i--)
        {
            var parentIndex = (i - 1) / 2;
            if (parentIndex >= 0)
            {
                nodes[parentIndex].AddConnectedNode(nodes[i]);
            }
        }
    }
}
