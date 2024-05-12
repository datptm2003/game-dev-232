using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MarketSystem : MonoBehaviour
{

    public bool isOpen;

    public static MarketSystem Instance { get; set; }

    // Start is called before the first frame update
    public void Awake() {
        Instance = this;
    }
    
    void Start()
    {
        isOpen = true;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
