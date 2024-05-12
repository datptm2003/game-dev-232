using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : MonoBehaviour
{
    public static PlayerState Instance { get; set; }

    float distanceTravelled = 0;
    Vector3 lastPosition;

    public GameObject playerBody;
    public int money;

    [Header("Related to health")]
    // ---------- Player Health ---------- //
    public int currentHealth;
    public int maxHealth;
    public int maxRegularHealth;

    // ---------- Player Calories ---------- //
    public int currentCalories;
    public int maxCalories;


    // ---------- Player Hydration ---------- //
    public int currentHydrationPercentage;
    public int maxHydrationPercentage;

    public bool isHydrationActive = true;

    // Player Stats
    [Header("Player Stats")]
    public int strength;
    public int defense;
    public int agility;
    public int luckily;
    public int weaponDamage;
    public string typeWeapon;
    public int damageRegular;


    private void Awake()
    {
        Instance = this;

    }

    // Start is called before the first frame update
    void Start()
    {
        maxHealth = maxRegularHealth;
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

    void UpdateStats()
    {
        // Update stats of weaponEquipment
        if (EquipSystem.Instance.weaponSlot)
        {
            setStrength(EquipSystem.Instance.weaponSlot.GetComponent<InventoryItem>().strength);
            setAgility(EquipSystem.Instance.weaponSlot.GetComponent<InventoryItem>().agility);
            setLuckily(EquipSystem.Instance.weaponSlot.GetComponent<InventoryItem>().luckily);
            setWeaponDamage(EquipSystem.Instance.weaponSlot.GetComponent<InventoryItem>().damage);
        }

        maxHealth = maxRegularHealth + strength * 2;
    }

    public void setHealth(int newHealth)
    {
        currentHealth = newHealth;
    }

    public void setCalories(int newCalories)
    {
        currentCalories = newCalories;
    }

    public void setHydration(int newHydration)
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

    // public int GetDamage(int typeWeapon) {

    // }
}
