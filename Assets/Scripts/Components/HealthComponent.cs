using UnityEngine;
using UnityEngine.Events;

namespace Components
{
    public class HealthComponent : MonoBehaviour
    {
        [SerializeField] private int _healthValue;

        [SerializeField] private UnityEvent OnHealed;
        [SerializeField] private UnityEvent OnDamaged;
        [SerializeField] private UnityEvent OnDie;
        
        public void ModifyHealth(int value)
        {
            Debug.Log($"Health was modified for {value}");

            _healthValue += value;
            
            switch (value)
            {
                case 0:
                    return;
                case > 0:
                    OnHealed?.Invoke();
                    break;
                default:
                    OnDamaged?.Invoke();
                    break;
            }

            if(_healthValue <= 0) OnDie?.Invoke();
        }
    }
}