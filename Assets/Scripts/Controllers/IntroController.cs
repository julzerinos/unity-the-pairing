using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class IntroController : MonoBehaviour
{
    public string[] texts;

    private Text _text;
    private Queue<string> _texts;

    private bool _skip;

    private void Awake()
    {
        _text = GameObject.Find("Text").GetComponent<Text>();
        _texts = new Queue<string>(texts);
    }

    private void Start()
    {
        StartCoroutine(Type());
    }

    private IEnumerator Type()
    {
        _skip = false;

        if (_texts.Count <= 0)
        {
            OpenMaster();
            yield return null;
        }

        var wait = new WaitForSeconds(.08f);
        var skip = new WaitForSeconds(0.004f);
        _text.text = "";
        foreach (var c in _texts.Dequeue())
        {
            _text.text += c;
            yield return _skip ? skip : wait;
        }

        yield return new WaitForSeconds(3.0f);

        StartCoroutine(Type());
    }

    private void Update()
    {
        if (Input.anyKeyDown)
            _skip = true;
    }

    private static void OpenMaster()
    {
        SceneManager.LoadScene("Master");
    }
}