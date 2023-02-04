using UnityEngine;

namespace Components
{
    public class ModifyHealthComponent : MonoBehaviour
    {
        [SerializeField] private int _value;

        public void ModifyHealth(GameObject go)
        {
            var health = go.GetComponent<HealthComponent>();

            if (health != null)
            {
                health.ModifyHealth(_value);
            }
        }
    }
}