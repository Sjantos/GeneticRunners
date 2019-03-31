using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureGenerator : MonoBehaviour
{
    public Creature[] CurrentGenerationCreatures;
    private List<Creature> currentGenerationCreatures = new List<Creature>();
    [SerializeField] GeneticAlgorithm geneticAlgorithm;
    [SerializeField] GameObject creaturePrefab;

    //[ExecuteInEditMode]
    //[EasyButtons.Button]
    //private void InstantiateCreatur()
    //{
    //    creatureObject = Instantiate(creaturePrefab, new Vector3(0f, 2f, 0f), Quaternion.identity);
    //    creatureObject.GetComponent<Creature>().CreateRandom(size);
    //    creatureObject.SetActive(false);
    //    creatureObject.SetActive(true);
    //}

    //public void InstantiateCreature(int creatureSize)
    //{
    //    var creatureObject = Instantiate(creaturePrefab, new Vector3(0f, 2f, 0f), Quaternion.identity);
    //    var creature = creatureObject.GetComponent<Creature>();
    //    currentGenerationCreatures.Add(creature);
    //    creature.CreateRandom(creatureSize);
    //    creatureObject.SetActive(false);
    //    creatureObject.SetActive(true);
    //}

    //public void InstantiateCreature(Creature c1, Creature c2, CrossoverMethod method)
    //{
    //    var creatureObject = Instantiate(creaturePrefab, new Vector3(0f, 2f, 0f), Quaternion.identity);
    //    var creature = creatureObject.GetComponent<Creature>();
    //    currentGenerationCreatures.Add(creature);
    //    creature.CreateFromParents(c1, c2, method);
    //    creatureObject.SetActive(false);
    //    creatureObject.SetActive(true);
    //}

    //private void OnGUI()
    //{
    //    if(GUI.Button(new Rect(100, 100, 100, 100), "tu"))
    //    {
    //        DestroyImmediate(creatureObject);
    //        InstantiateCreatur();
    //    }
    //    if (GUI.Button(new Rect(200, 100, 100, 100), "tu"))
    //    {
    //        for (int i = 0; i < count; i++)
    //        {
    //            creatureObject = Instantiate(creaturePrefab, new Vector3(0f, 2f, 0f), Quaternion.identity);
    //            var creature = creatureObject.GetComponent<Creature>();
    //            currentGenerationCreatures.Add(creature);
    //            creature.CreateRandom(size);//(i % 3) + 3);
    //            creatureObject.SetActive(false);
    //            creatureObject.SetActive(true);

    //        }
    //        CurrentGenerationCreatures = currentGenerationCreatures.ToArray();
    //        geneticAlgorithm.canRun = true;
    //    }
    //    if (GUI.Button(new Rect(300, 100, 100, 100), "OnePoint"))
    //    {
    //        var creature1 = Instantiate(creaturePrefab, new Vector3(0f, 2f, 0f), Quaternion.identity);
    //        creature1.GetComponent<Creature>().CreateRandom(7);
    //        creature1.SetActive(false);
    //        creature1.SetActive(true);

    //        var creature2 = Instantiate(creaturePrefab, new Vector3(0f, 2f, 0f), Quaternion.identity);
    //        creature2.GetComponent<Creature>().CreateRandom(7);
    //        creature2.SetActive(false);
    //        creature2.SetActive(true);

    //        creatureObject = Instantiate(creaturePrefab, new Vector3(0f, 2f, 0f), Quaternion.identity);
    //        creatureObject.GetComponent<Creature>().CreateFromParents(creature1.GetComponent<Creature>(), creature2.GetComponent<Creature>(), CrossoverMethod.OnePoint);
    //        creatureObject.SetActive(false);
    //        creatureObject.SetActive(true);
    //    }

    //    //if (GUI.Button(new Rect(400, 100, 100, 100), "PMX"))
    //    //{
    //    //    var creature1 = Instantiate(creaturePrefab, new Vector3(0f, 2f, 0f), Quaternion.identity);
    //    //    creature1.GetComponent<Creature>().CreateRandom(7);
    //    //    creature1.SetActive(false);
    //    //    creature1.SetActive(true);

    //    //    var creature2 = Instantiate(creaturePrefab, new Vector3(0f, 2f, 0f), Quaternion.identity);
    //    //    creature2.GetComponent<Creature>().CreateRandom(7);
    //    //    creature2.SetActive(false);
    //    //    creature2.SetActive(true);

    //    //    creatureObject = Instantiate(creaturePrefab, new Vector3(0f, 2f, 0f), Quaternion.identity);
    //    //    creatureObject.GetComponent<Creature>().CreateFromParents(creature1.GetComponent<Creature>(), creature2.GetComponent<Creature>(), CrossoverMethod.PMX);
    //    //    creatureObject.SetActive(false);
    //    //    creatureObject.SetActive(true);
    //    //}
    //    if (GUI.Button(new Rect(500, 100, 100, 100), "OX"))
    //    {
    //        var creature1 = Instantiate(creaturePrefab, new Vector3(0f, 2f, 0f), Quaternion.identity);
    //        creature1.GetComponent<Creature>().CreateRandom(7);
    //        creature1.SetActive(false);
    //        creature1.SetActive(true);

    //        var creature2 = Instantiate(creaturePrefab, new Vector3(0f, 2f, 0f), Quaternion.identity);
    //        creature2.GetComponent<Creature>().CreateRandom(7);
    //        creature2.SetActive(false);
    //        creature2.SetActive(true);

    //        creatureObject = Instantiate(creaturePrefab, new Vector3(0f, 2f, 0f), Quaternion.identity);
    //        creatureObject.GetComponent<Creature>().CreateFromParents(creature1.GetComponent<Creature>(), creature2.GetComponent<Creature>(), CrossoverMethod.OnePoint);
    //        creatureObject.SetActive(false);
    //        creatureObject.SetActive(true);
    //    }
    //}
}
