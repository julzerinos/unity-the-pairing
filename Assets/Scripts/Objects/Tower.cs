using System;
using System.Collections;
using Controllers;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Objects
{
    public class Tower : MonoBehaviour
    {
        public static event Action<float> OnHeightChange;

        public Transform hero;

        public int levels;

        public float waitTime;

        public GameObject[] sidesPrefabs;
        public GameObject basePrefab;
        public GameObject topPrefab;

        private GameObject[] _levels;

        private WaitForSeconds _rotateWait;

        private Rigidbody _rg;

        private bool _inControl = true;

        private void Awake()
        {
            CreateLevel();

            _rotateWait = new WaitForSeconds(waitTime);

            _rg = GetComponent<Rigidbody>();

            Hero.OnEndInitiated += OnInitiateEnd;
            Spaceship.OnDeath += OnDeath;
        }

        private void OnDestroy()
        {
            Hero.OnEndInitiated -= OnInitiateEnd;
            Spaceship.OnDeath -= OnDeath;
        }

        private void OnDeath()
        {
            _inControl = false;
        }
        
        private void CreateLevel()
        {
            _levels = new GameObject[levels + 2];

            _levels[0] = Instantiate(basePrefab, transform);
            var l = 1;
            for (; l < _levels.Length - 1; l++)
            {
                var level = new GameObject {name = $"Level {l}"};
                level.transform.parent = transform;

                var sides = new GameObject[8];
                for (var i = 0; i < 8; i++)
                {
                    var x = Mathf.Sin(Mathf.Deg2Rad * i * 45) * .5f;
                    var y = Mathf.Cos(Mathf.Deg2Rad * i * 45) * .5f;

                    var g = Instantiate(sidesPrefabs[Random.Range(0, sidesPrefabs.Length)], level.transform);
                    g.transform.position = transform.position + new Vector3(x, l, y);
                    g.transform.rotation = Quaternion.Euler(0, i * 45, 0);

                    sides[i] = g;
                }

                _levels[l] = level;
            }

            var top = Instantiate(topPrefab, transform);
            top.transform.Translate(new Vector3(0, l, 0));
            _levels[_levels.Length - 1] = top;
        }

        private void Update()
        {
            OnHeightChange?.Invoke((hero.position.y + .5f) / (levels + 1f));
        }

        private void FixedUpdate()
        {
            if (!_inControl)
                return;
            
            _rg.AddTorque(new Vector3(0, -5f * Input.GetAxisRaw("Horizontal"), 0));
        }

        private IEnumerator Rotate(Quaternion startRot, Quaternion targetRot)
        {
            for (var i = 0f; i <= 1; i += 0.01f)
            {
                transform.rotation = Quaternion.Lerp(startRot, targetRot, i);

                yield return _rotateWait;
            }
        }

        private void OnInitiateEnd()
        {
            StartCoroutine(Rotate(transform.rotation, Quaternion.identity * Quaternion.Euler(0, 6, 0)));
            _inControl = false;
        }
    }
}