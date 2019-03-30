using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureGenerator : MonoBehaviour
{
    [SerializeField] GameObject creaturePrefab;
    GameObject creature;

    private void Start()
    {
        //for (int i = 0; i < 1; i++)
        //{
        //    creature = Instantiate(creaturePrefab, new Vector3(0f, 2f, 0f), Quaternion.identity);
        //    creature.GetComponent<Creature>().CreateRandom(8);//(i % 3) + 3);
        //    creature.SetActive(false);
        //    creature.SetActive(true);
        //}
    }
    [ExecuteInEditMode]
    [EasyButtons.Button]
    private void InstantiateCreatur()
    {
        creature = Instantiate(creaturePrefab, new Vector3(0f, 2f, 0f), Quaternion.identity);
        creature.GetComponent<Creature>().CreateRandom(8);
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
    }
}
