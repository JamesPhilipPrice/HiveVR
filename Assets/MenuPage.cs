using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuPage : MonoBehaviour
{
    public ScreenFader faderManager;

    public void StartTraining()
    {
        float timeLeft = faderManager.fadeDuration;
        faderManager.FadeOut();
        while(timeLeft > 0)
        {
            timeLeft -= Time.deltaTime;
        }
        SceneManager.LoadScene("BeeField");
    }

    public void OpenOptionsPage()
    {

    }

    public void QuitApplication()
    {
        Application.Quit();
    }
}
