using UnityEngine;
using UnityEngine.SceneManagement;

public class MainmenuPLay : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadSceneAsync(1);
    }

    public void Quit()
    {
        Application.Quit();
    }

}
