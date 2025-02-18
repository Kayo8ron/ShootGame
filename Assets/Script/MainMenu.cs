using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Método para cargar la escena del juego
    public void LoadGameScene()
    {
        SceneManager.LoadScene(0); // Cambia 0 por el nombre de la escena si es necesario
    }

    // Método para salir del juego
    public void QuitGame()
    {
        Application.Quit();
    }
}
