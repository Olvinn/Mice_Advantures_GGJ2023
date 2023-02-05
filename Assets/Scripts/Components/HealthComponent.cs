using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Components
{
    public class HealthComponent : MonoBehaviour
    {
        public bool isShielded;
        
        [SerializeField] private int _healthValue;
        [SerializeField] private int _recoveryEverySec = 0;

        [SerializeField] private UnityEvent<float> OnHealed;
        [SerializeField] private UnityEvent<float> OnDamaged;
        [SerializeField] private UnityEvent OnDie;
        [SerializeField] private UnityEvent OnBlock;
        
        private int _maxHP;

        private void Awake()
        {
            _maxHP = _healthValue;
        }

        private void Start()
        {
            StartCoroutine(Regen());
        }

        public void ModifyHealth(int value)
        {
            if (!isShielded)
                _healthValue += value;
            
            if (value >= 0)
                OnHealed?.Invoke((float) _healthValue / _maxHP);
            else if (isShielded)
                OnBlock?.Invoke();
            else
                OnDamaged?.Invoke((float) _healthValue / _maxHP);
            if (_healthValue <= 0)
                OnDie?.Invoke();
        }

        IEnumerator Regen()
        {
            while (true)
            {
                yield return new WaitForSeconds(1);
                if (_healthValue < _maxHP)
                    ModifyHealth(_recoveryEverySec);
            }
        }
    }
}