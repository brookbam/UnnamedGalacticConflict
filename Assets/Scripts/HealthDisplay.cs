using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthDisplay : MonoBehaviour
{
    TMP_Text healthText;
    Player1 player1;
    int currentHealth;


    // Start is called before the first frame update
    void Start()
    {
        healthText = GetComponent<TMP_Text>();
        player1 = FindObjectOfType<Player1>();
    }

    // Update is called once per frame
    void Update()
    {
        currentHealth = player1.GetHealth();
        if(currentHealth > 0)
        {
            healthText.text = player1.GetHealth().ToString();
        }
        else
        {
            healthText.text = "0";
        }
        
    }
}
