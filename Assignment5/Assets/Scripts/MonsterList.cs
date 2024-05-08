using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class MonsterList : MonoBehaviour {
    public static MonsterList Instance { get; set; }

    public List<GameObject> listMonster = new List<GameObject>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    void Start() {
        foreach (Transform child in this.transform.GetComponentsInChildren<Transform>())
        {
            if (child.CompareTag("Monster"))
            {
                listMonster.Add(child.gameObject);
            }
        }
    }


}