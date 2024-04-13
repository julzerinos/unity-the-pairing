using System.Collections;
using Controllers;
using UnityEngine;

public class FinalScene : MonoBehaviour
{
    private SpriteRenderer _cage;
    private Animator _am;
    private static readonly int Saved = Animator.StringToHash("Saved");
    private GameObject _hearts;
    
    private void Awake()
    {
        _cage = transform.GetChild(0).Find("Cage").GetComponent<SpriteRenderer>();
        _am = GetComponentInChildren<Animator>();
        _hearts = transform.GetChild(0).Find("Hearts").gameObject;
        
        Hero.OnEndInitiated += OnEnd;
    }

    private void OnDestroy()
    {
        Hero.OnEndInitiated -= OnEnd;
    }

    private void OnEnd()
    {
        StartCoroutine(FlashCage());
        _am.SetTrigger(Saved);
        _hearts.SetActive(true);
    }

    private IEnumerator FlashCage()
    {
        var cageColor = _cage.color;
        cageColor.a = 0;
        _cage.color = cageColor;

        var wait = new WaitForSeconds(.5f);
        yield return wait;
        cageColor.a = 1;
        _cage.color = cageColor;

        yield return wait;
        cageColor.a = 0;
        _cage.color = cageColor;

        yield return wait;
        cageColor.a = 1;
        _cage.color = cageColor;

        yield return wait;
        cageColor.a = 0;
        _cage.color = cageColor;
    }
}