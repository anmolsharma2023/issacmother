using UnityEngine;
using Unity.Netcode;
public class Issacanim : NetworkBehaviour
{
    private Rigidbody rb; // Reference to Rigidbody
    private Animator anim; // Reference to Animator

    public override void OnNetworkSpawn()
    {
       
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
    }
    private void Update()
    {
        
        if (rb != null&&IsOwner)
        {
            // Update animation based on velocity
            UpdateAnimation(rb.linearVelocity);
        }
    }
    private void UpdateAnimation(Vector3 velocity)
    {
        // Calculate speed based on velocity magnitude
        float currentSpeed = new Vector2(velocity.x, velocity.z).magnitude;
        if (currentSpeed > 1f)
        {
            AnimServerRpc(true);
        }
        else
        {
            AnimServerRpc(false);


        }

    }
    [ServerRpc]
    private void AnimServerRpc(bool state)
    {
AnimClientRpc(state);
    }
    [ClientRpc]
    private void AnimClientRpc(bool state)
    {
        
            anim.SetBool("run", state);
        
    }
}
