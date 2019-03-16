using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureGenerator : MonoBehaviour
{
    [SerializeField] Creature creature;

    private void Start()
    {
        creature.SetUp(3);
        creature.gameObject.SetActive(false);
        //yield return new WaitForEndOfFrame();
        creature.gameObject.SetActive(true);
    }
}
