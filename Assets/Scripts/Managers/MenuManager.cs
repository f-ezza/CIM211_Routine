using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
public class MenuManager : MonoBehaviour
{
    public void ChangeScene(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
