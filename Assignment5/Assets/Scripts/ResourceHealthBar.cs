using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourceHealthBar : MonoBehaviour
{
    private Slider slider;
    private float currentHealth, maxHealth;

    public GameObject globalState;

    void Awake()
    {
        slider = GetComponent<Slider>();
    }

    // // Start is called before the first frame update
    // void Start()
    // {

    // }

    // Update is called once per frame
    private void Update()
    {
        currentHealth = globalState.GetComponent<GlobalState>().resourceHealth;
        maxHealth = globalState.GetComponent<GlobalState>().resourceMaxHealth;

        float fillValue = currentHealth / maxHealth;
        slider.value = fillValue;
    }
}
