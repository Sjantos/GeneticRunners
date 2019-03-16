using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Creature : MonoBehaviour
{
    [SerializeField] Node[] nodes;
    [SerializeField] int easyButtonsNodeCount = 3;

    [EasyButtons.Button("SetUp")]
    public void SetUp()
    {
        SetUp(easyButtonsNodeCount);
        StartCoroutine(Coro(easyButtonsNodeCount));
    }
    private IEnumerator Coro(int nodeCount)
    {
        //foreach (var node in nodes)
        //{
        //    node.gameObject.SetActive(false);
        //}
        yield return new WaitForEndOfFrame();
        for (int i = 0; i < nodeCount; i++)
        {
            nodes[i].gameObject.SetActive(true);
        }
    }

    [EasyButtons.Button("Reset")]
    public void Reset()
    {
        foreach (var node in nodes)
        {
            node.Reset();
        }
    }

    public void SetUp(int nodeCount)
    {
        for (int i = 0; i < nodeCount; i++)
        {
            var dist = Vector3.Distance(nodes[i].transform.position, nodes[(i+1) % nodeCount].transform.position);
            var muscles = new Muscle[] { };
            Debug.Log("dist=" + dist);
            if (i <2)
            {
                muscles = new Muscle[]
            {
                new Muscle()
                {
                    baseObj = nodes[i].gameObject,
                    connectedObj = nodes[(i+1) % nodeCount].Rigidbody2D,
                    minLength = dist-0.1f,
                    maxLength = dist + Random.Range(0.5f, 1f)
                }
            };
            }
            nodes[i].SetUp(Random.Range(0f, 1f), muscles);
        }
    }

    private void OnGUI()
    {
        if(GUI.Button(new Rect(100, 100, 100, 100), "click"))
        {
            SetUp(easyButtonsNodeCount);
        }
    }
}
