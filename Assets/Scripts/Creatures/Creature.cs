using UnityEngine;

namespace Creatures
{
    public abstract class Creature : MonoBehaviour
    {
        public abstract void Attack();
    }

    public enum CreatureState
    {
        Move,
        Attack,
        Fall,
        Block
    }
}