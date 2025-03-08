using UnityEngine;

public class Issacsound : MonoBehaviour
{
    private Rigidbody rb;
    private GameObject soundmanage;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        soundmanage = GameObject.FindGameObjectWithTag("Sound");
    }
    private void Update()
    {
        if (rb.linearVelocity.magnitude > 1f)
        {
            AudioManager aa = soundmanage.GetComponent<AudioManager>();
            aa.PlayOneShot("walk", gameObject);
        }
    }
}
