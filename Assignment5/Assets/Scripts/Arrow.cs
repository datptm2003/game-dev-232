using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Target"))
        {
            print("hit " + collision.gameObject.name + " !");
            Destroy(gameObject);
        }

        if (collision.gameObject.CompareTag("Monster"))
        {
            print("hit " + collision.gameObject.name + " ! ");
            collision.gameObject.GetComponent<Monster>().TakeDamage(EquipSystem.Instance.GetWeaponDamage(), this.transform.gameObject);
        }
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
