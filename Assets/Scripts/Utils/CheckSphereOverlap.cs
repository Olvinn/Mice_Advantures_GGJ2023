using System;
using UnityEngine;
using UnityEngine.Events;

namespace Utils
{
    public class CheckSphereOverlap : LayerCheck
    {
        [SerializeField] private OnOverlapEvent _onOverlapEvent;

        private readonly Collider[] _interactResult = new Collider[10];

        public void Check()
        {
            var size = Physics.OverlapSphereNonAlloc(transform.position, _collider.radius,
                _interactResult,_layerMask);

            for (var i = 0; i < size; i++)
            {
                _onOverlapEvent?.Invoke(_interactResult[i].gameObject);
            }
        }

        [Serializable]
        public class OnOverlapEvent : UnityEvent<GameObject> { }
    }
}