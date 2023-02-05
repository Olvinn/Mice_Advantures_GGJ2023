using System.Collections;
using UnityEngine;

namespace Components
{
    public class DestroyComponent : MonoBehaviour
    {
        [SerializeField] private Behaviour[] toDestroy;
        [SerializeField] private Transform toDigDown;
        [SerializeField] private Animator animator;
        
        public void Destroy()
        {
            Destroy(gameObject, 10f);
            foreach (var m in toDestroy)
            {
                Destroy(m);
            }
            
            animator.SetTrigger("Dead");

            StartCoroutine(Falling());
        }

        IEnumerator Falling()
        {
            yield return new WaitForSeconds(2);
            while(true)
            {
                toDigDown.transform.position += Vector3.down * Time.deltaTime;
                yield return null;
            }
        }
    }
}