using UnityEngine;
using UnityEngine.SceneManagement;

public class Opensceneui : MonoBehaviour
{

    [SerializeField] private GameObject sounder;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Startbutton()
    {
        AudioManager aa = sounder.GetComponent<AudioManager>();
        aa.PlayOneShot("button", gameObject);
        SceneManager.LoadScene(1);
    }
    public void Quitbutton()
    {
        AudioManager aa = sounder.GetComponent<AudioManager>();
        aa.PlayOneShot("button", gameObject);
        Application.Quit();
    }
}
