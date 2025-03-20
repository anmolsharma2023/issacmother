using Cysharp.Threading.Tasks;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;
public class Netmanageui : MonoBehaviour
{
    [SerializeField] private GameObject cli;
    [SerializeField] private GameObject ho;
    [SerializeField] private GameObject ima;
    [SerializeField] private GameObject cam;
    [SerializeField] private GameObject joiho;
    [SerializeField] private GameObject scroll;
    [SerializeField] private GameObject createe;

    [SerializeField] private GameObject inpuu;
    [SerializeField] private GameObject rella;
    [SerializeField] private GameObject loader;
    [SerializeField] private GameObject sounder;


    private void Tester()
    {
        Debug.Log("FUK ITT");
    }
        public void Hostme()
    {
        AudioManager aa=sounder.GetComponent<AudioManager>();
        aa.PlayOneShot("button",gameObject);
       // SessionManager.Instance.StartSessionAsHost();
       Lobbymanage lobbymanage=rella.GetComponent<Lobbymanage>();
        loader.SetActive(true);
        lobbymanage.CreateRelay();
      
        Removeit();
    }
    public void Clientme()
    {
        AudioManager aa = sounder.GetComponent<AudioManager>();
        aa.PlayOneShot("button", gameObject);
        TMP_InputField tt=inpuu.GetComponent<TMP_InputField>();
        string code = tt.text;
        Lobbymanage lobbymanage = rella.GetComponent<Lobbymanage>();
        loader.SetActive(true);

        lobbymanage.JoinRelay(code);
        //    var sessions = SessionManager.Instance.QuerySessions().Forget();
      
        Removeit();

    }
    
private void Removeit()
    {
        createe.SetActive(false);
        cli.SetActive(false);
        ho.SetActive(false);
        ima.SetActive(false);
        cam.SetActive(false);
        joiho.SetActive(false);
        scroll.SetActive(false);
        inpuu.SetActive(false);
    }

}
