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
    [SerializeField] private GameObject buttonforlobby;
    public GameObject parentObject; // Assign the parent GameObject in the Inspector
    private void SpawnButtonAsync(string id)
    {
        // Instantiate the button prefab
        GameObject buttonObject = Instantiate(buttonforlobby, parentObject.transform);

        // Set the parent
        buttonObject.transform.SetParent(parentObject.transform);

        // Get the Button and Text components
        Button button = buttonObject.GetComponent<Button>();
        Text buttonText = buttonObject.GetComponentInChildren<Text>();

        // Change the button text
        buttonText.text = id;

        // Add an OnClick listener
        button.onClick.AddListener(() => { Tester(); });
    }
    private void Tester()
    {
        Debug.Log("FUK ITT");
    }
        public void Hostme()
    {
        SessionManager.Instance.StartSessionAsHost();
      //  NetworkManager.Singleton.StartHost();
        Removeit();
    }
    public void Clientme()
    {
        var sessions = SessionManager.Instance.QuerySessions().Forget();
        //NetworkManager.Singleton.StartClient();
        Removeit();

    }
    
private void Removeit()
    {
        cli.SetActive(false);
        ho.SetActive(false);
        ima.SetActive(false);
        cam.SetActive(false);
        joiho.SetActive(false);
        scroll.SetActive(false);
    }

}
