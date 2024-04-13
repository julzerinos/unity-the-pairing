using System;
using System.Collections;
using UnityEngine;

namespace Controllers
{
    [RequireComponent(typeof(Rigidbody))]
    public class Spaceship : MonoBehaviour
    {
        public static event Action<float> OnHealthChange;
        public static event Action OnDeath;

        public float maxTurnAngle = 10f;
        public float turnForce = 3f;
        public float moveForce = 2f;

        public int maxHealth;
        public int healRate;

        public Transform boundary;

        private Vector2 _move;
        private Quaternion _initialRotation;
        private Quaternion _lerpQuaternion = Quaternion.identity;
        private Coroutine _rotate;
        private readonly WaitForSeconds _rotateWait = new WaitForSeconds(.01f);

        private Rigidbody _rg;

        private ParticleSystem _ps;
        private int _health;


        private int Health
        {
            get => _health;
            set
            {
                _health = value;
                OnHealthChange?.Invoke((float) _health / maxHealth);
            }
        }

        private readonly WaitForSeconds _healTick = new WaitForSeconds(1.0f);

        private TrailRenderer _trail;

        private bool _inControl = true;
        private bool _died;

        private void Awake()
        {
            _rg = GetComponent<Rigidbody>();
            _initialRotation = _rg.rotation;
            _rg.interpolation = RigidbodyInterpolation.Interpolate;

            _health = maxHealth;
            StartCoroutine(HealTick());

            _trail = GetComponentInChildren<TrailRenderer>();

            _ps = GetComponentInChildren<ParticleSystem>();

            Hero.OnEndInitiated += OnEndInitiated;
        }

        private void OnDestroy()
        {
            Hero.OnEndInitiated -= OnEndInitiated;
        }

        private void OnEndInitiated()
        {
            _inControl = false;
        }

        private IEnumerator HealTick()
        {
            while (_health > 0)
            {
                if (_health < maxHealth)
                    Health++;
                yield return _healTick;
            }

            Die();
        }

        private void Die()
        {
            _inControl = false;
            OnDeath?.Invoke();
            _ps.Play();
            _died = true;
        }

        private void Update()
        {
            _move = new Vector2(
                Input.GetAxisRaw("Horizontal"),
                Input.GetAxisRaw("Vertical")
            );
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!_inControl)
                return;

            if (other.gameObject.CompareTag("Obstacle"))
                Health -= 10;

            if (other.gameObject.CompareTag("sg_FlyBoundary"))
            {
                var shipTransform = transform;
                var boundaryPos = boundary.position;

                var pos = boundaryPos - shipTransform.position;
                var moveTo = boundaryPos + pos - pos.normalized;
                moveTo.z = 0;

                shipTransform.position = moveTo;
                _trail.Clear();
            }
        }

        private void FixedUpdate()
        {
            if (!_inControl)
            {
                if (_died) _rg.AddTorque(.3f * Vector3.forward);
                else _rg.AddForce(-Vector3.forward, ForceMode.Impulse);
                return;
            }

            if (_move.sqrMagnitude > .0f)
            {
                if (_rotate != null)
                    StopCoroutine(_rotate);
                _rotate = null;

                _rg.AddForce(moveForce * new Vector3(-_move.x, _move.y, 0), ForceMode.Force);

                var angles = new Vector3(5f * _move.x, _move.x, -_move.y) * maxTurnAngle;
                _lerpQuaternion =
                    Quaternion.Lerp(_lerpQuaternion, Quaternion.Euler(angles), turnForce * Time.deltaTime);
                _rg.rotation = _initialRotation * _lerpQuaternion;

                return;
            }

            _rotate ??= StartCoroutine(Rotate(_rg.rotation, _initialRotation));
        }

        private IEnumerator Rotate(Quaternion from, Quaternion to)
        {
            for (float i = 0; i <= 1; i += .01f)
            {
                _rg.rotation = Quaternion.Slerp(@from, to, Mathf.SmoothStep(0, 1, i));
                _lerpQuaternion = Quaternion.Inverse(_initialRotation) * _rg.rotation;

                yield return _rotateWait;
            }
        }
    }
}