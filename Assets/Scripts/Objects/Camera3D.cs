using UnityEngine;

namespace Objects
{
    public class Camera3D : MonoBehaviour
    {
        private Camera _camera;

        private void Awake()
        {
            _camera = GetComponent<Camera>();
        }

        private void LateUpdate()
        {
            Color.RGBToHSV(_camera.backgroundColor, out var h, out var s, out var v);
            _camera.backgroundColor = Color.HSVToRGB(h + 0.0001f % 1, s, v);
        }
    }
}