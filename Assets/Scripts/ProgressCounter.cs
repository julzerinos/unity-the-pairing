using System;
using System.Collections;
using System.Collections.Generic;
using Controllers;
using Objects;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public class ProgressCounter : MonoBehaviour
{
    private Text _text;
    private bool _died;

    private void Awake()
    {
        _text = GetComponentInChildren<Text>();

        Tower.OnHeightChange += UpdateWithRatio;
        Spaceship.OnDeath += UpdateAfterDeath;
    }

    private void OnDestroy()
    {
        Tower.OnHeightChange -= UpdateWithRatio;
        Spaceship.OnDeath -= UpdateAfterDeath;
    }

    private void UpdateAfterDeath()
    {
        _died = true;
        UpdateText("x.x");
    }

    private void UpdateWithRatio(float ratio)
    {
        if (_died) return;
        UpdateText($"{ratio*100:F0}%");
    }

    private void UpdateText(string text)
    {
        _text.text = text;
    }
}