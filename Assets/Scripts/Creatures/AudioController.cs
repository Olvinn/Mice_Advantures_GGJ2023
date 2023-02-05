using System.Collections;
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
        [SerializeField] private AudioClip[] _slash;
        [SerializeField] private AudioClip[] _getDamage;
        [SerializeField] private AudioClip _dead;
        [SerializeField] private AudioClip _block;
        
        [SerializeField] private AudioSource _source;
        [SerializeField] private AudioSource _stepsSource;
        
        public void PlaySteps()
        {
            if (_stepsSource.clip != null)
                return;
            _stepsSource.clip = _steps;
            _stepsSource.loop = true;
            _stepsSource.time = 0.5f;
            _stepsSource.volume = volume;
            _stepsSource.Play();
        }

        public void PlayIdle()
        {
            _source.loop = false;
            _source.volume = volume * .75f;
            _source.PlayOneShot(_idle[Random.Range(0, _idle.Length)]);
        }

        public void PlayDying()
        {
            _source.loop = false;
            _source.volume = volume * .4f;
            _source.PlayOneShot(_dead);
        }

        public void PlayJump()
        {
            _source.loop = false;
            _source.time = 0.5f;
            _source.volume = volume;
            _source.clip = _jump;
            _source.Play();
        }

        public void PlayGrounded()
        {
            _source.loop = false;
            _source.time = 0.7f;
            _source.volume = volume;
            _source.clip = _grounded;
            _source.Play();
        }

        public void PlaySwordSlash()
        {
            _source.loop = false;
            // _source.time = 0.2f;
            _source.volume = .4f * volume;
            _source.PlayOneShot(_slash[Random.Range(0, _slash.Length)]);
        }

        public void PlayGetDamage()
        {
            _source.loop = false;
            // _source.time = 0.2f;
            _source.volume = volume* .6f;;
            _source.PlayOneShot(_getDamage[Random.Range(0, _getDamage.Length)]);
        }

        public void StopPlayingSteps()
        {
            if (_stepsSource.clip == null)
                return;
            _stepsSource.clip = null;
            _stepsSource.Stop();
        }

        public void PlayBlock()
        {
            _source.loop = false;
            _source.time = 0.5f;
            _source.volume = volume* .9f;;
            _source.PlayOneShot(_block);
        }
    }
}
