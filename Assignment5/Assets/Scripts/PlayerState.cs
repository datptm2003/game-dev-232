using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : MonoBehaviour
{
    public static PlayerState Instance { get; set; }

    // ---------- Player Health ---------- //
    public float currentHealth;
    public float maxHealth;

    // ---------- Player Calories ---------- //
    public float currentCalories;
    public float maxCalories;

    float distanceTravelled = 0;
    Vector3 lastPosition;

    public GameObject playerBody;


    // ---------- Player Hydration ---------- //
    public float currentHydrationPercentage;
    public float maxHydrationPercentage;

    public bool isHydrationActive = true;

    // Player Stats
    public int strength;
    public int defense;
    public int agility;
    public int luckily;
    public int weaponDamage;
    public int damageRegular;

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

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        currentCalories = maxCalories;
        currentHydrationPercentage = maxHydrationPercentage;

        StartCoroutine(decreaseHydration());
    }

    IEnumerator decreaseHydration()
    {
        while (isHydrationActive)
        {
            currentHydrationPercentage -= 1;

            yield return new WaitForSeconds(2);
        }
    }

    // Update is called once per frame
    void Update()
    {
        distanceTravelled += Vector3.Distance(playerBody.transform.position, lastPosition);
        lastPosition = playerBody.transform.position;

        if (distanceTravelled >= 5)
        {
            distanceTravelled = 0;
            currentCalories -= 1;
        }

        // Testing the health bar
        if (Input.GetKeyDown(KeyCode.N))
        {
            currentHealth -= 10;
        }
    }

    public void setHealth(float newHealth)
    {
        currentHealth = newHealth;
    }

    public void setCalories(float newCalories)
    {
        currentCalories = newCalories;
    }

    public void setHydration(float newHydration)
    {
        currentHydrationPercentage = newHydration;
    }

    public void setWeaponDamage(int newWeaponDamage)
    {
        weaponDamage = newWeaponDamage;
    }

    public void setStrength(int newStrength)
    {
        strength = newStrength;
    }

    public void setAgility(int newAgility)
    {
        agility = newAgility;
    }

    public void setLuckily(int newLuckily)
    {
        luckily = newLuckily;
    }

    public int GetDamage()
    {
        return damageRegular + weaponDamage;
    }
}
