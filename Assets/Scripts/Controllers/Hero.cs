using System;
using System.Collections;
using UnityEngine;

namespace Controllers
{
    public class Hero : MonoBehaviour
    {
        public static event Action OnEndInitiated;

        public float speed;
        public float jumpForce;
        public float flipDuration;

        private Rigidbody2D _rg;

        private Animator _am;
        private static readonly int Running = Animator.StringToHash("Running");

        private bool _isGrounded;
        private bool _falling;
        private bool _jumped;
        private Transform _groundedDetector1;
        private Transform _groundedDetector2;

        private float _move;
        private bool _isRunning;
        private static readonly int IsGrounded = Animator.StringToHash("isGrounded");

        private bool _facingRight = true;
        private Coroutine _flipCoroutine;
        private WaitForSeconds _flipWait;

        private bool _inControl = true;
        private static readonly int Die = Animator.StringToHash("Die");
        private static readonly int Falling = Animator.StringToHash("Falling");

        private void Awake()
        {
            _rg = GetComponent<Rigidbody2D>();
            _am = GetComponentInChildren<Animator>();

            _flipWait = new WaitForSeconds(flipDuration);

            _groundedDetector1 = transform.Find("Grounded detect 1");
            _groundedDetector2 = transform.Find("Grounded detect 2");

            Spaceship.OnDeath += OnDeath;
        }

        private void OnDestroy()
        {
            Spaceship.OnDeath -= OnDeath;
        }

        private void OnDeath()
        {
            _inControl = false;
            _am.SetTrigger(Die);
        }

        private void Update()
        {
            _move = Input.GetAxisRaw("Horizontal");
            _jumped = _jumped || Input.GetButtonDown("Vertical");

            _am.SetBool(Running, _isRunning);
            _am.SetBool(IsGrounded, _isGrounded);
            _am.SetBool(Falling, _rg.velocity.y < -0.1f);
        }

        private void FixedUpdate()
        {
            if (!_inControl)
                return;

            _isGrounded = Physics2D.Raycast(_groundedDetector1.position, Vector2.down, .01f) ||
                          Physics2D.Raycast(_groundedDetector2.position, Vector2.down, .01f);

            if (_isGrounded && _jumped)
            {
                _rg.AddForce(jumpForce * Vector2.up, ForceMode2D.Impulse);
                _jumped = false;
            }

            if (!(_isRunning = Mathf.Abs(_move) > 0))
                return;

            Flip(_move > 0);
        }

        private void Flip(bool right)
        {
            if (_facingRight == right)
                return;

            _facingRight = right;

            if (_flipCoroutine != null)
                StopCoroutine(_flipCoroutine);
            _flipCoroutine = StartCoroutine(FlipAnim());
        }

        private IEnumerator FlipAnim()
        {
            var t = transform;
            var scale = t.localScale;
            for (float i = 0; i <= 1; i += .05f)
            {
                scale.x = Mathf.Lerp(
                    scale.x,
                    _facingRight ? 1 : -1,
                    i);

                t.localScale = scale;

                yield return _flipWait;
            }

            yield return null;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("pg_End") && _isGrounded)
                InitiateEnd();
        }

        private void InitiateEnd()
        {
            if (!_inControl) return;
            
            _inControl = false;
            _isRunning = false;
            Flip(true);
            OnEndInitiated?.Invoke();
        }
    }
}