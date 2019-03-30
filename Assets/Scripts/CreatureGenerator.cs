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
        if (GUI.Button(new Rect(300, 100, 100, 100), "OnePoint"))
        {
            var creature1 = Instantiate(creaturePrefab, new Vector3(0f, 2f, 0f), Quaternion.identity);
            creature1.GetComponent<Creature>().CreateRandom(7);
            creature1.SetActive(false);
            creature1.SetActive(true);

            var creature2 = Instantiate(creaturePrefab, new Vector3(0f, 2f, 0f), Quaternion.identity);
            creature2.GetComponent<Creature>().CreateRandom(7);
            creature2.SetActive(false);
            creature2.SetActive(true);

            creature = Instantiate(creaturePrefab, new Vector3(0f, 2f, 0f), Quaternion.identity);
            creature.GetComponent<Creature>().CreateFromParents(creature1.GetComponent<Creature>(), creature2.GetComponent<Creature>(), CrossoverMethod.OnePoint);
            creature.SetActive(false);
            creature.SetActive(true);
        }

        if (GUI.Button(new Rect(400, 100, 100, 100), "PMX"))
        {
            var creature1 = Instantiate(creaturePrefab, new Vector3(0f, 2f, 0f), Quaternion.identity);
            creature1.GetComponent<Creature>().CreateRandom(7);
            creature1.SetActive(false);
            creature1.SetActive(true);

            var creature2 = Instantiate(creaturePrefab, new Vector3(0f, 2f, 0f), Quaternion.identity);
            creature2.GetComponent<Creature>().CreateRandom(7);
            creature2.SetActive(false);
            creature2.SetActive(true);

            creature = Instantiate(creaturePrefab, new Vector3(0f, 2f, 0f), Quaternion.identity);
            creature.GetComponent<Creature>().CreateFromParents(creature1.GetComponent<Creature>(), creature2.GetComponent<Creature>(), CrossoverMethod.PMX);
            creature.SetActive(false);
            creature.SetActive(true);
        }
        if (GUI.Button(new Rect(500, 100, 100, 100), "OX"))
        {
            var creature1 = Instantiate(creaturePrefab, new Vector3(0f, 2f, 0f), Quaternion.identity);
            creature1.GetComponent<Creature>().CreateRandom(7);
            creature1.SetActive(false);
            creature1.SetActive(true);

            var creature2 = Instantiate(creaturePrefab, new Vector3(0f, 2f, 0f), Quaternion.identity);
            creature2.GetComponent<Creature>().CreateRandom(7);
            creature2.SetActive(false);
            creature2.SetActive(true);

            creature = Instantiate(creaturePrefab, new Vector3(0f, 2f, 0f), Quaternion.identity);
            creature.GetComponent<Creature>().CreateFromParents(creature1.GetComponent<Creature>(), creature2.GetComponent<Creature>(), CrossoverMethod.OnePoint);
            creature.SetActive(false);
            creature.SetActive(true);
        }
    }
}
