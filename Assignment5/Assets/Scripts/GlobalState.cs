using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalState : MonoBehaviour
{
    public static GlobalState Instance { get; set; }

    public float resourceHealth;
    public float resourceMaxHealth;

    private void Awake()
    {

        Instance = this;
        
    }




    // // Start is called before the first frame update
    // void Start()
    // {

    // }

    // // Update is called once per frame
    // void Update()
    // {

    // }
}
