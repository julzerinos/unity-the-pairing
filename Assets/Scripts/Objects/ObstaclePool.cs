using System;
using System.Collections;
using System.Collections.Generic;
using Controllers;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Objects
{
    public class ObstaclePool : MonoBehaviour
    {
        public bool standalone;
        public GameObject obstacle;
        public int spawnCount = 35;

        private List<GameObject> _obstacles;

        private bool _shouldSpawn = true;
        private WaitForSeconds[] _waits;

        private void Awake()
        {
            _obstacles = new List<GameObject>();

            for (var i = 0; i < spawnCount; i++)
            {
                var ob = Instantiate(obstacle, transform);
                ob.SetActive(false);
                _obstacles.Add(ob);
            }

            _waits = new[]
            {
                new WaitForSeconds(Random.value),
                new WaitForSeconds(Random.value),
                new WaitForSeconds(Random.value),
                new WaitForSeconds(Random.value),
                new WaitForSeconds(Random.value),
                new WaitForSeconds(Random.value),
            };

            if (standalone)
                return;

            Hero.OnEndInitiated += OnEnd;
            Spaceship.OnDeath += OnEnd;
            Instructions.OnStart += OnStart;
        }

        private void OnDestroy()
        {
            if (standalone)
                return;

            Hero.OnEndInitiated -= OnEnd;
            Spaceship.OnDeath -= OnEnd;
            Instructions.OnStart -= OnStart;
        }

        private void Start()
        {
            if (!standalone)
                return;

            OnStart();
        }

        private void OnStart()
        {
            _shouldSpawn = true;
            StartCoroutine(SpawnLoop());
        }

        private void OnEnd()
        {
            _shouldSpawn = false;
        }

        private IEnumerator SpawnLoop()
        {
            while (_shouldSpawn)
            {
                var o = GetObstacle();
                o.SetActive(true);
                o.transform.position = transform.position + 8f * Random.insideUnitSphere;

                yield return _waits[Random.Range(0, _waits.Length)];
            }
        }

        private GameObject GetObstacle()
        {
            foreach (var o in _obstacles)
                if (!o.activeInHierarchy)
                    return o;

            var ob = Instantiate(obstacle, transform);
            _obstacles.Add(ob);
            return ob;
        }
    }
}