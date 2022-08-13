using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Enemy : MonoBehaviour
{

    GameObject enemy;

    public TextMeshProUGUI HealthDisplayEnemy;

    public float StartHealth, Health, Damage;

    [SerializeField]
    public string[] EnemyType = { "slime", "orc" };

    public RawImage EnemyImage;

    // Start is called before the first frame update
    void Start()
    {
        print(EnemyType[0]);

        StartHealth = 10;

        enemy = gameObject;
        


    }

    // Update is called once per frame
    void Update()
    {
        //sets the UI health of enemy to the float value Health
        HealthDisplayEnemy.text = Health.ToString();

        //will check if enemy health is 0 or lower. if yes, enemy dissappears
        if (Health <= 0)
        {
            EnemyImage.enabled = false;
        }
        else if (Health > 0)
        {
            EnemyImage.enabled = true;
        }
    }
}
