using UnityEngine;

public class Gamesoundmanage : MonoBehaviour
{
    [SerializeField] private GameObject soundmanage;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        AudioManager aa=soundmanage.GetComponent<AudioManager>();
        aa.Play("bg",gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
