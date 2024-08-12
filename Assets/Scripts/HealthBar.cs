using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    Player1 player1;
    int currentHealth;
    [SerializeField] int healthBlock = 200;


    // Start is called before the first frame update
    void Start()
    {
        player1 = FindObjectOfType<Player1>();
    }

    // Update is called once per frame
    void Update()
    {
        currentHealth = player1.GetHealth();
        if (currentHealth < healthBlock)
        {
            Destroy(gameObject);
        }
    }
}
