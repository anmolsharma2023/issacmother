using TMPro;
using Unity.Netcode;
using UnityEngine;

public class Netmanageui : MonoBehaviour
{
    [SerializeField] private GameObject cli;
    [SerializeField] private GameObject ho;
    [SerializeField] private GameObject ima;
    public void Hostme()
    {
        NetworkManager.Singleton.StartHost();
        Removeit();
    }
    public void Clientme()
    {
        NetworkManager.Singleton.StartClient();
        Removeit();

    }
private void Removeit()
    {
        cli.SetActive(false);
        ho.SetActive(false);
        ima.SetActive(false);
    }

}
