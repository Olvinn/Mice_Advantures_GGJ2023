using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Scenes
{
    public class Game : MonoBehaviour
    {
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
