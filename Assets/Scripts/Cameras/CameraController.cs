using UnityEngine;

namespace Cameras
{
    public class CameraController : MonoBehaviour
    {
        public float positionLerpSpeed = 10f;
        public float horizontalSpeed = 1.5f,
            verticalSpeed = .75f;
        
        public Vector3 positionOffset, targetOffset;
        
        [SerializeField] private Camera camera;
        [SerializeField] private Transform target;

        private Vector3 _savedAngle;
        private Vector3 _endPosition;

        private void Update()
        {
            if (camera == null)
                return;
            
            Rotate(new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y")));

            camera.transform.position = Vector3.Slerp(camera.transform.position, _endPosition, Time.deltaTime * positionLerpSpeed);
        }

        private void LateUpdate()
        {
            if (camera == null)
                return;

            Vector3 offset = camera.transform.localToWorldMatrix * targetOffset;
            camera.transform.LookAt(target.position + offset);
        }

        public void Rotate(Vector2 delta)
        {
            _savedAngle += new Vector3(-(delta.y * verticalSpeed) % 360, (delta.x * horizontalSpeed) % 360, 0);

            _endPosition = target.transform.position + Quaternion.Euler(_savedAngle) * positionOffset;
        }
    }
}
