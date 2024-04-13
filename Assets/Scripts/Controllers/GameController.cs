using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Controllers
{
    public class GameController : MonoBehaviour
    {
        public bool spaceGameStandalone;
        public bool platformGameStandalone;

        private GameObject _spaceGame;
        private GameObject _platformGame;

        private bool _restartAble;

        private void Awake()
        {
            _spaceGame = GameObject.FindWithTag("Space_Game");
            _platformGame = GameObject.FindWithTag("Platform_Game");

            _platformGame.SetActive(false);
            _spaceGame.SetActive(false);

            SceneManager.LoadScene("Platform game", LoadSceneMode.Additive);
            SceneManager.LoadScene("Space game", LoadSceneMode.Additive);

            StartCoroutine(Opening());

            Spaceship.OnDeath += OnEnd;
            Hero.OnEndInitiated += OnEnd;
        }

        private void OnDestroy()
        {
            Spaceship.OnDeath -= OnEnd;
            Hero.OnEndInitiated -= OnEnd;
        }

        private void OnEnd()
        {
            _restartAble = true;
        }

        private IEnumerator Restart()
        {
            yield return new WaitForSeconds(20f);
            RestartScene();
        }

        private void Update()
        {
            if (_restartAble && Input.GetKeyDown(KeyCode.Space))
                RestartScene();
        }

        private void RestartScene()
        {
            SceneManager.LoadScene("Master");
        }

        private IEnumerator Opening()
        {
            var wait = new WaitForSeconds(3.0f);
            yield return wait;
            _platformGame.SetActive(true);
            yield return wait;
            _spaceGame.SetActive(true);
        }
    }
}