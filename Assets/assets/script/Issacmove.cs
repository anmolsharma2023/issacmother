using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class Issacmove : NetworkBehaviour
{
    //public Camera playerCamera; // Reference to your camera
    public string doorTag = "Door"; // Tag for doors
    public float detectionRange = 3f; // Range to detect doors
    public Transform referencePoint; // Reference point from which to measure distance
    private GameObject doorbtn;
  //  private List<GameObject> doorsInRange = new List<GameObject>();
    public float speed = 5f; // Movement speed
    private Rigidbody rb; // Reference to the Rigidbody component
    private Joystick joystick;
    
    public override void OnNetworkSpawn()
    {

        if (IsOwner)
        {
            Issacprops issacprops = GetComponent<Issacprops>();
            if (issacprops != null)
            {
                issacprops.iamowner = true;
            }
            // Define the tag you want to search for
            string tagToFind = "Camera";

            // Find all GameObjects with the specified tag
            GameObject[] objectsWithTag = GameObject.FindGameObjectsWithTag(tagToFind);
            Debug.Log(objectsWithTag.Length);
            // Do something with the found objects
            foreach (GameObject obj in objectsWithTag)
            {
                Camfollowplayer camfollowplayer = obj.GetComponent<Camfollowplayer>();
                if (camfollowplayer != null)
                {
                    camfollowplayer.player=gameObject;
                }
            }
        }
        // Find the GameObject with the tag "joysticke"
        GameObject joystickObject = GameObject.FindWithTag("Joysticke");
        GameObject sppo = GameObject.FindWithTag("Spa1");
        gameObject.transform.position=sppo.transform.position;
        if (joystickObject != null)
        {
            // If your Joystick variable should hold a reference to the GameObject itself:
            joystick = joystickObject.GetComponent<Joystick>(); // Ensure there's a Joystick component attached

            if (joystick == null)
            {
                Debug.LogError("No Joystick component found on the object tagged 'joysticke'.");
            }
        }
        else
        {
            Debug.LogError("No object found with tag 'joysticke'.");
        }
 
        rb = GetComponent<Rigidbody>(); // Get the Rigidbody component attached to this GameObject
        referencePoint = gameObject.transform;
    }
    
    void FixedUpdate()
    {
        if (IsOwner)
         { MovePlayer();

            //if (Input.GetKeyDown(KeyCode.Space))
            //{
               
                // Now doorsInRange contains all doors within the specified range
                // Debug.Log($"Doors in range: {doorsInRange.Count}");
               // Debug.Log("calling to server");
              //  DoorServerRpc();
            //}
        
        }
        else
            return;
    }
 
    private void MovePlayer()
    {
        // Get input from the player
        float moveHorizontal = joystick.Horizontal;
        float moveVertical = joystick.Vertical; // W/S or Up/Down arrows
        Vector3 movementInput = new Vector3(moveHorizontal, 0.0f, moveVertical).normalized;
        // Create a movement vector based on input
        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical).normalized;

        // Set the Rigidbody's velocity for constant movement
        rb.linearVelocity = movement * speed;
        MoveServerRpc(movementInput);

    }
    [ServerRpc]
    private void MoveServerRpc(Vector3 input)
    {
        // Apply movement on server side by setting linear velocity.
        rb.linearVelocity = new Vector3(input.x * speed, 0f, input.z * speed);

        // Optionally update all clients with new position via ClientRpc.
        UpdateAllClientRpc(transform.position);
    }

    [ClientRpc]
    void UpdateAllClientRpc(Vector3 newPosition)
    {

        transform.position = newPosition;

    }
}
