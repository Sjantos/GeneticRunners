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

        #region oldCode
        //List<Node> refreshedNodes = new List<Node>();
        //for (int i = 0; i < nodes.Length; i++)
        //{
        //    if (i < size)
        //        refreshedNodes.Add(nodes[i]);
        //    else
        //        DestroyImmediate(nodes[i].gameObject);
        //}
        //nodes = refreshedNodes.ToArray();

        //RepositionNodesRandomly();
        //int n = size;
        //int additionalMuscleCount = Random.Range(0, ((n * (n - 1)) / 2) - (n - 1));
        //Debug.Log(string.Format("nodeCount={0} muscleCount={1}", n, (additionalMuscleCount + (n - 1))));
        //List<int> nodesLeft = new List<int>();
        //for (int i = 0; i < n; i++)
        //{
        //    nodesLeft.Add(i);
        //}

        ////base connections
        //List<int> baseConnectedNodes = new List<int>() { 0 };
        //var randomIndex = Random.Range(1, nodes.Length);
        //baseConnectedNodes.Add(randomIndex);
        //var secondNode = nodes[randomIndex];
        //var firstDist = Vector3.Distance(nodes[0].transform.position, secondNode.transform.position);
        //nodes[0].MuscleCount++;
        //secondNode.MuscleCount++;
        //nodes[0].SetUp(Random.Range(0f, 1f), new Muscle[]
        //{
        //    new Muscle()
        //    {
        //        baseObj = nodes[0].gameObject,
        //        connectedObj = secondNode.Rigidbody2D,
        //        minLength = firstDist - Random.Range(0f, minLengthRandom),
        //        maxLength = firstDist + Random.Range(0f, maxLengthRandom)
        //    }
        //});
        //secondNode.SetUp(Random.Range(0f, 1f), new Muscle[] { });

        //for (int i = 1; i < n; i++)
        //{
        //    if (baseConnectedNodes.Contains(i)) //(nodes[i].MuscleCount != 0)
        //        continue;
        //    var second = nodes[baseConnectedNodes[Random.Range(0, baseConnectedNodes.Count)]];
        //    var d = Vector3.Distance(nodes[i].transform.position, second.transform.position);
        //    nodes[i].MuscleCount++;
        //    second.MuscleCount++;
        //    nodes[i].SetUp(Random.Range(0f, 1f), new Muscle[]
        //    {
        //    new Muscle()
        //    {
        //        baseObj = nodes[i].gameObject,
        //        connectedObj = second.Rigidbody2D,
        //        minLength = d - Random.Range(0f, minLengthRandom),
        //        maxLength = d + Random.Range(0f, maxLengthRandom)
        //    }
        //    });
        //} 
        #endregion
    }

    private void CreateAndPositionNodesRandomly(int size)
    {
        nodes = new Node[size];
        for (int i = 0; i < size; i++)
        {
            var randomPosition = new Vector3(Random.Range(-5f, 5f), Random.Range(-2.5f, 2.5f), 0f);
            //Transform parent = transform;
            //var parentIndex = (i - 1) / 2;
            //if (parentIndex > 0)
            //    parent = nodes[parentIndex].transform;
            var newNode = Instantiate(nodePrefab, transform);
            newNode.transform.localPosition = randomPosition;
            nodes[i] = newNode.GetComponent<Node>();
        }

        #region oldCode
        //for (int i = 0; i < nodes.Length; i++)
        //{
        //    var goodPosition = true;
        //    var possiblePosition = Vector3.zero;
        //    do
        //    {
        //        goodPosition = true;
        //        possiblePosition = new Vector3(Random.Range(-2.5f, 2.5f), Random.Range(-2.5f, 2.5f), 0f);
        //        for (int j = i - 1; j >= 0; j--)
        //        {
        //            if (Vector3.Distance(possiblePosition, nodes[j].gameObject.transform.position) < 2 * minLengthRandom)
        //            {
        //                goodPosition = false;
        //                break;
        //            }
        //        }
        //    } while (!goodPosition);
        //    nodes[i].transform.localPosition = possiblePosition;
        //} 
        #endregion
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
        #region oldCode
        //for (int i = 0; i < nodeCount; i++)
        //{
        //    var dist = Vector3.Distance(nodes[i].transform.position, nodes[(i + 1) % nodeCount].transform.position);
        //    var muscles = new Muscle[]
        //    {
        //        new Muscle()
        //        {
        //            baseObj = nodes[i].gameObject,
        //            connectedObj = nodes[(i+1) % nodeCount].Rigidbody2D,
        //            minLength = dist-0.1f,
        //            maxLength = dist + Random.Range(0.5f, 1f)
        //        }
        //    };
        //    nodes[i].SetUp(Random.Range(0f, 1f), muscles);
        //} 
        #endregion
    }
}
