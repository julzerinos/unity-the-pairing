using System.Collections;
using Controllers;
using UnityEngine;
using UnityEngine.UI;

public class HullDamage : MonoBehaviour
{
    private Image _ri;

    private Coroutine _fill;
 
    private readonly WaitForSeconds _fillWait = new WaitForSeconds(.01f);
    
    private void Awake()
    {
        _ri = transform.Find("bar").GetComponent<Image>();
        _ri.fillAmount = 0;

        Spaceship.OnHealthChange += UpdateBar;
    }

    private void OnDestroy()
    {
        Spaceship.OnHealthChange -= UpdateBar;
    }

    private void Start()
    {
        _fill = StartCoroutine(SmoothFill(0, 1));
    }

    private IEnumerator SmoothFill(float a, float b)
    {
        for (var i = 0f; i <= 1; i += .01f)
        {
            _ri.fillAmount = Mathf.SmoothStep(a, b, i);
            yield return _fillWait;
        }
    }

    private void UpdateBar(float ratio)
    {
        StopCoroutine(_fill);
        _fill = StartCoroutine(SmoothFill(_ri.fillAmount, ratio));
    }
}