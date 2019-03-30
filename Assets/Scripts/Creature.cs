using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Creature : MonoBehaviour
{
    [SerializeField] GameObject nodePrefab;
    [SerializeField] Node[] nodes;
    [SerializeField] int easyButtonsNodeCount = 3;

    [SerializeField] float minLengthRandom = 0.5f;
    [SerializeField] float maxLengthRandom = 1f;

    private Vector3[] positions;
    private int[] structure;

    [EasyButtons.Button("SetUp")]
    public void SetUp()
    {
        SetUp(easyButtonsNodeCount);
    }

    [EasyButtons.Button("Reset")]
    public void Reset()
    {
        foreach (var node in nodes)
        {
            node.Reset();
        }
    }

    public void CreateRandom()
    {
        //CreateRandom(Random.Range(3, 6));
    }

    public void CreateRandom(int size)
    {
        CreateRandomStructure(size);
        CreateAndPositionNodesRandomly(size);
        CreateConnectionsInStructure();
        foreach (var node in nodes)
        {
            node.Enable();
        }
    }

    private void CreateAndPositionNodesRandomly(int size)
    {
        nodes = new Node[size];
        positions = new Vector3[size];
        for (int i = 0; i < size; i++)
        {
            var randomPosition = new Vector3(Random.Range(-5f, 5f), Random.Range(-2.5f, 2.5f), 0f);
            positions[i] = randomPosition;
            var newNode = Instantiate(nodePrefab, transform);
            newNode.transform.localPosition = randomPosition;
            nodes[i] = newNode.GetComponent<Node>();
        }
    }

    private void CreateRandomStructure(int size)
    {
        structure = new int[size];
        for (int i = 0; i < size; i++)
            structure[i] = i;
        RandomExtension.Shuffle<int>(structure);
    }

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

    public void SetUp(int nodeCount)
    {
    }
}
