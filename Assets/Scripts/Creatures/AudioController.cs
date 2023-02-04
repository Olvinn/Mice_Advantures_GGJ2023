using UnityEngine;
using Random = UnityEngine.Random;

namespace Creatures
{
    public class AudioController : MonoBehaviour
    {
        public float volume = 1f;
        
        [SerializeField] private AudioClip[] _idle;
        [SerializeField] private AudioClip _steps;
        [SerializeField] private AudioClip _jump;
        [SerializeField] private AudioClip _grounded;
        [SerializeField] private AudioClip _slash;
        
        [SerializeField] private AudioSource _source;

        public void PlaySteps()
        {
            if (_source.clip == _steps)
                return;
            _source.clip = _steps;
            _source.loop = true;
            _source.time = 0.5f;
            _source.volume = volume;
            _source.Play();
        }

        public void PlayIdle()
        {
            if (_source.clip == _grounded)
                return;
            _source.clip = _idle[Random.Range(0, _idle.Length)];
            _source.loop = false;
            _source.volume = volume;
            _source.Play();
        }

        public void PlayJump()
        {
            _source.clip = _jump;
            _source.loop = false;
            _source.time = 0.5f;
            _source.volume = volume;
            _source.Play();
        }

        public void PlayGrounded()
        {
            _source.clip = _grounded;
            _source.loop = false;
            _source.time = 0.7f;
            _source.volume = volume;
            _source.Play();
        }

        public void PlaySwordSlash()
        {
            _source.clip = _slash;
            _source.loop = false;
            // _source.time = 0.2f;
            _source.volume = .2f * volume;
            _source.Play();
        }

        public void StopLoops()
        {
            if (_source.clip != _steps)
                return;
            _source.clip = null;
            _source.loop = true;
            _source.Stop();
        }
    }
}
