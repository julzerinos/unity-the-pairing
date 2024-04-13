using UnityEngine;

namespace Objects
{
    public class SpaceScene : MonoBehaviour
    {
        public bool standalone;

        public float offset;
        public float rotateSpeed;
        public float moveSpeed;

        private Transform _move;

        private float _length;

        private void Awake()
        {
            _move = transform.Find("Move boundary");
            _length = _move.GetComponentInChildren<Renderer>().bounds.size.z;

            if (!standalone)
                Tower.OnHeightChange += UpdateZ;
        }

        private void OnDestroy()
        {
            if (!standalone)
                Tower.OnHeightChange -= UpdateZ;
        }

        private void UpdateZ(float ratio)
        {
            _move.position = new Vector3(0, 0, ratio * _length - offset);
        }

        private void FixedUpdate()
        {
            _move.Rotate(Vector3.forward, rotateSpeed * Time.deltaTime);

            if (standalone)
                _move.transform.Translate(moveSpeed * Vector3.forward);
        }
    }
}