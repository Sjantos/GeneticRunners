using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Creature : MonoBehaviour
{
    [SerializeField] Node[] nodes;
    [SerializeField] int easyButtonsNodeCount = 3;

    [SerializeField] float minLengthRandom = 0.5f;
    [SerializeField] float maxLengthRandom = 1f;

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
        CreateRandom(Random.Range(3, 6));
    }

    public void CreateRandom(int size)
    {
        RepositionNodesRandomly();
        //int n = Random.Range(3, 6);
        int n = size;
        int additionalMuscleCount = Random.Range(0, ((n * (n - 1)) / 2) - (n - 1));
        Debug.Log(string.Format("nodeCount={0} muscleCount={1}", n, (additionalMuscleCount + (n - 1))));
        List<int> nodesLeft = new List<int>();
        for (int i = 0; i < n; i++)
        {
            nodesLeft.Add(i);
        }

        for (int i = 0; i < n - 1; i++)
        {
            //int randIndex = 0;
            //do
            //{
            //    randIndex = Random.Range(0, nodesLeft.Count);
            //} while (nodesLeft[randIndex] == i);
            var nextNode = nodesLeft[(i+1) % n];
            //nodesLeft.Remove(nextNode);

            int additionalMuscles = 0;
            if (additionalMuscleCount > 0)
                additionalMuscles = Random.Range(0, additionalMuscleCount < (n-2) ? additionalMuscleCount : (n-2));
            additionalMuscleCount -= additionalMuscles;

            List<Muscle> muscleList = new List<Muscle>();
            //base - to make sure all nodes are connected
            var dist = Vector3.Distance(nodes[i].transform.position, nodes[nextNode].transform.position);
            Debug.Log(string.Format("Connecting nodes {0} with node {1}", i, nextNode));
            muscleList.Add(new Muscle()
            {
                baseObj = nodes[i].gameObject,
                connectedObj = nodes[nextNode].Rigidbody2D,
                minLength = dist - Random.Range(0f, minLengthRandom),
                maxLength = dist + Random.Range(0f, maxLengthRandom)
            });
            ////now additional muscles
            //List<int> usedNodes = new List<int>() { i, nextNode };
            //for (int m = 0; m < additionalMuscles; m++)
            //{
            //    int next = 0;
            //    do
            //    {
            //        next = Random.Range(0, n);
            //    } while (usedNodes.Contains(next));
            //    usedNodes.Add(next);
            //    var d = Vector3.Distance(nodes[i].transform.position, nodes[next].transform.position);
            //    muscleList.Add(new Muscle()
            //    {
            //        baseObj = nodes[i].gameObject,
            //        connectedObj = nodes[next].Rigidbody2D,
            //        minLength = d - Random.Range(0f, minLengthRandom),
            //        maxLength = d + Random.Range(0f, maxLengthRandom)
            //    });
            //}

            nodes[i].SetUp(Random.Range(0f, 1f), muscleList.ToArray());
            Debug.Log("Finished creating one node and it's connections");
        }

        nodes[n - 1].SetUp(Random.Range(0f, 1f), new Muscle[] { });
    }

    private void RepositionNodesRandomly()
    {
        for (int i = 0; i < nodes.Length; i++)
        {
            nodes[i].transform.localPosition = new Vector3(Random.Range(-2.5f, 2.5f), Random.Range(-2.5f, 2.5f), 0f);
        }
    }

    public void SetUp(int nodeCount)
    {
        for (int i = 0; i < nodeCount; i++)
        {
            var dist = Vector3.Distance(nodes[i].transform.position, nodes[(i+1) % nodeCount].transform.position);
            var muscles = new Muscle[]
            {
                new Muscle()
                {
                    baseObj = nodes[i].gameObject,
                    connectedObj = nodes[(i+1) % nodeCount].Rigidbody2D,
                    minLength = dist-0.1f,
                    maxLength = dist + Random.Range(0.5f, 1f)
                }
            };
            nodes[i].SetUp(Random.Range(0f, 1f), muscles);
        }
    }
}
