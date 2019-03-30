using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureGenerator : MonoBehaviour
{
    [SerializeField] GameObject creaturePrefab;
    [SerializeField] int size = 7;
    [SerializeField] int count = 10;
    GameObject creature;

    [ExecuteInEditMode]
    [EasyButtons.Button]
    private void InstantiateCreatur()
    {
        creature = Instantiate(creaturePrefab, new Vector3(0f, 2f, 0f), Quaternion.identity);
        creature.GetComponent<Creature>().CreateRandom(size);
        creature.SetActive(false);
        creature.SetActive(true);
    }

    private void OnGUI()
    {
        if(GUI.Button(new Rect(100, 100, 100, 100), "tu"))
        {
            DestroyImmediate(creature);
            InstantiateCreatur();
        }
        if (GUI.Button(new Rect(200, 100, 100, 100), "tu"))
        {
            for (int i = 0; i < count; i++)
            {
                creature = Instantiate(creaturePrefab, new Vector3(0f, 2f, 0f), Quaternion.identity);
                creature.GetComponent<Creature>().CreateRandom(size);//(i % 3) + 3);
                creature.SetActive(false);
                creature.SetActive(true);
            }
        }
    }
}
