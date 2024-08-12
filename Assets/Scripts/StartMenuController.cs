﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartMenuController : MonoBehaviour
{

    Level level;
    // Update is called once per frame

    private void Start()
    {
        level = FindObjectOfType<Level>();   
    }

    void Update()
    {
        if (Input.GetButtonDown("Fire2") || Input.GetButtonDown("Fire1"))
        {
            level.LoadGame();
        }
    }
}
