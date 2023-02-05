using System;
using UnityEngine;

namespace Widgets
{
    public class PausePanel : MonoBehaviour
    {
        [SerializeField] private Animator _animator;
        private static readonly int HideKey = Animator.StringToHash("Hide");

        public event Action<bool> OnActivate;
        
        private float _defaultTimeScale;

        private void OnEnable()
        {
            OnActivate?.Invoke(true);

            _defaultTimeScale = Time.timeScale;
            Time.timeScale = 0f;
        }

        public void Hide()
        {
            _animator.SetTrigger(HideKey);
        }

        public void Deactivate()
        {
            gameObject.SetActive(false);
        }

        private void OnDisable()
        {
            Time.timeScale = _defaultTimeScale;
            OnActivate?.Invoke(false);
        }
    }
}