using System;
using UnityEngine;

namespace Utils
{
    [RequireComponent(typeof(SphereCollider))]
    public class LayerCheck : MonoBehaviour
    {
        [SerializeField] protected LayerMask _layerMask;

        protected SphereCollider _collider;
        public bool IsTouching { get; private set; }

        private void Awake()
        {
            if (_collider == null)
                _collider = GetComponent<SphereCollider>();
        }

        private void OnTriggerEnter(Collider other)
        {
            IsTouching = (_layerMask.value & (1 << other.gameObject.layer)) > 0;
        }

        private void OnTriggerExit(Collider other)
        {
            IsTouching = false;
        }

#if UNITY_EDITOR

        private void OnValidate()
        {
            if (_collider == null)
                _collider = GetComponent<SphereCollider>();
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = IsTouching ? Color.green : Color.red;
            Gizmos.DrawWireSphere(transform.position, _collider.radius * 2);
        }
#endif
    }
}