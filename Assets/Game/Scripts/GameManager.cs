using UnityEngine;

namespace Game
{
    public class GameManager : MonoBehaviour
    {
        private void OnQuit()
        {
            Debug.Log("Quit");
            QuitGame();
        }
        
        private void QuitGame()
        {
            Application.Quit();
        }
    }
}