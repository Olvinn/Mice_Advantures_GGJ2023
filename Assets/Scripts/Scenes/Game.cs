using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;
using UnityEngine.SceneManagement;
using Widgets;

namespace Scenes
{
    public class Game : MonoBehaviour
    {
        [SerializeField] private Volume vol;

        [SerializeField]
        private PausePanel _pausePanel; //Да какая-то херня, ну курсора в игре не будет=> включать тут будем

        private void Start()
        {
            ActivateCursor(false);
            
            _pausePanel.OnActivate += ActivateCursor;
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

        private void ActivateCursor(bool value)
        {
            Cursor.visible = value;
            Cursor.lockState = value ? CursorLockMode.None : CursorLockMode.Locked;
        }

        private IEnumerator Restart()
        {
            yield return new WaitForSeconds(3);
            SceneManager.LoadScene(1);
        }

        private void Update()
        {
            if (Input.GetKey(KeyCode.Escape))
            {
                _pausePanel.gameObject.SetActive(true);
            }
        }

        private void OnDestroy()
        {
            _pausePanel.OnActivate -= ActivateCursor;
        }
    }
}