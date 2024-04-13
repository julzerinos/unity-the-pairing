using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.SceneManagement;

public class Standalone : MonoBehaviour
{
    public string scene;
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            RestartScene();
    }

    private void RestartScene()
    {
        SceneManager.LoadScene(scene);
    }
}