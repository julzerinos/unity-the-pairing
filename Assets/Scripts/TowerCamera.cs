using UnityEngine;

public class TowerCamera : MonoBehaviour
{
    public Transform hero;

    private void LateUpdate()
    {
        var pos = transform.position;
        pos.y = Mathf.Max(Mathf.Lerp(pos.y, hero.position.y, .1f), 0f);
        transform.position = pos;
    }
}