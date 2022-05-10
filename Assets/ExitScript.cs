using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitScript : MonoBehaviour
{
    public void ExitToMain()
    {
        SceneManager.LoadScene("Menu");
    }
}
