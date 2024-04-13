using UnityEngine;

public class Platform : MonoBehaviour
{
    public Texture[] textures;

    private Collider2D _cl;
    private PlatformEffector2D _ef;

    private void Awake()
    {
        GetComponent<Renderer>().material.SetTexture("_MainTex", textures[Random.Range(0, textures.Length)]);

        transform.position += (Random.Range(-.13f, .13f)) * Vector3.right;
        var s = transform.localScale;
        s.x += Random.Range(0, .1f);
        transform.localScale = s;

        _cl = GetComponent<Collider2D>();
        _ef = GetComponent<PlatformEffector2D>();

        _ef.enabled = false;
        _cl.enabled = false;
    }

    private void OnBecameInvisible()
    {
        _ef.enabled = false;
        _cl.enabled = false;
    }

    private void OnBecameVisible()
    {
        _ef.enabled = true;
        _cl.enabled = true;
    }
}