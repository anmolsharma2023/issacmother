using UnityEngine;

public class Openscene : MonoBehaviour
{
    [SerializeField] private GameObject sounder;
     // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       AudioManager mm=sounder.GetComponent<AudioManager>();
        mm.Play("bg1",gameObject);
        
    }
    public void Cleanchildren()
    {
        // Loop through all children and destroy them
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
    }
 
}
