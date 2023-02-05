using System;
using UnityEngine;

namespace Creatures
{
    public class AttackAnimationEventCatcher : MonoBehaviour
    {
        public Action onHit, onEndAttack, onSound;

        public void Hit()
        {
            onHit?.Invoke();
        }

        public void EndAttack()
        {
            onEndAttack?.Invoke();
        }

        public void Sound()
        {
            onSound?.Invoke();
        }
    }
}
