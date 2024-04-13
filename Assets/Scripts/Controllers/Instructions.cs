using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Instructions : MonoBehaviour
{
    public static event Action OnStart;
    
    private void Update()
    {
        if (!Input.GetKeyDown(KeyCode.Space))
            return;
            
        OnStart?.Invoke();
        gameObject.SetActive(false);
    }
}
