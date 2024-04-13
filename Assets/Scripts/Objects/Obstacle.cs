using UnityEngine;
using Random = UnityEngine.Random;

namespace Objects
{
    public class Obstacle : MonoBehaviour
    {
        public Color[] baseColors;

        private Vector3 _randomAxis;

        private bool _hasTarget;
        private Vector3 _currentTarget;
        private Vector3 _direction;
        private float _speed;

        private Material _mat;
        private static readonly int Color = Shader.PropertyToID("_Color");

        private Collider _cl;
        private GameObject _comet;
        private ParticleSystem _ps;

        private void Awake()
        {
            _randomAxis = Random.insideUnitSphere;

            _mat = GetComponentInChildren<Renderer>().material;
            _ps = GetComponentInChildren<ParticleSystem>();

            _comet = transform.Find("Model").gameObject;

            _cl = GetComponent<Collider>();
        }

        private void OnEnable()
        {
            _hasTarget = true;
            _direction = Vector3.forward;
            _ps.Pause();
            _comet.SetActive(true);
            
            Randomize();
        }

        private void Randomize()
        {
            var color = baseColors[Random.Range(0, baseColors.Length)];
            var mainModule = _ps.main;
            mainModule.startColor = color;
            _mat.SetColor(Color, color);

            transform.localScale = Random.Range(.2f, 3f) * new Vector3(1, 1, 1);

            _speed = Random.Range(40, 60);
        }

        private void FixedUpdate()
        {
            transform.Rotate(_randomAxis, .1f * Time.timeSinceLevelLoad, Space.Self);

            if (_hasTarget)
                transform.Translate(_direction * (_speed * Time.deltaTime), Space.World);
        }

        private void Die()
        {
            _hasTarget = false;
            gameObject.SetActive(false);
        }

        private void OnTriggerEnter(Collider other)
        {
            if ( other.CompareTag("Player"))
            {
                HitPlayer();
                return;
            }

            Die();
        }

        private void HitPlayer()
        {
            _cl.enabled = false;
            _comet.SetActive(false);
            _hasTarget = false;
            _ps.Play();
        }
    }
}