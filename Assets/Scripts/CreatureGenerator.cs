using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureGenerator : MonoBehaviour
{
    [SerializeField] GameObject creaturePrefab;

    private void Start()
    {
        //var creature = Instantiate(creaturePrefab, new Vector3(0f, 2f, 0f), Quaternion.identity);
        //creature.GetComponent<Creature>().CreateRandom(3);
        //creature.SetActive(false);
        //creature.SetActive(true);
        //Debug.Break();
        for (int i = 0; i < 30; i++)
        {
            var creature = Instantiate(creaturePrefab, new Vector3(0f, 2f, 0f), Quaternion.identity);
            creature.GetComponent<Creature>().CreateRandom((i % 3) + 3);
            creature.SetActive(false);
            creature.SetActive(true);
        }
    }
}
