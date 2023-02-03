using UnityEngine;

namespace Components
{
    public class QuitComponent: MonoBehaviour
    {
        public void Quit()
        {
            Application.Quit();
        }
    }
}