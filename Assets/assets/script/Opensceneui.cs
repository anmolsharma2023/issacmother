using UnityEngine;
using UnityEngine.SceneManagement;

public class Opensceneui : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
   public void Startbutton()
    {
        SceneManager.LoadScene(1);
    }
    public void Quitbutton()
    {
        Application.Quit();
    }
}
