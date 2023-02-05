using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;
using UnityEngine.SceneManagement;

namespace Scenes
{
    public class Game : MonoBehaviour
    {
        [SerializeField] private Volume vol;
        
        private void Start()
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }

        public void UpdatePostProc(float val)
        {
            Vignette vig;
            if (vol.profile.TryGet(out vig))
                vig.intensity.value = 1 - val;
        }

        public void RestartGame()
        {
            StartCoroutine(Restart());
        }
        
        IEnumerator Restart()
        {
            yield return new WaitForSeconds(3);
            SceneManager.LoadScene(1);
        }
    }
}
