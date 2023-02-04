using UnityEngine;

namespace Components
{
    public class DestroyComponent : MonoBehaviour
    {
        public void Destroy()
        {
            Destroy(gameObject);
        }
    }
}