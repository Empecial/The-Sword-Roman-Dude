using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class Player : MonoBehaviour
{

    public GameObject player, Enemy;

    public float StartHealth, Health, Damage;

    public TextMeshProUGUI HealthDisplay;
    

    // Start is called before the first frame update
    void Start()
    {
        player = gameObject;

        StartHealth = 20;

        Health = 20;


    }

    // Update is called once per frame
    void Update()
    {

        HealthDisplay.text = Health.ToString() ;

        if (Health <= 0)
        {

            print("player dead");
        }
        

    }


}


