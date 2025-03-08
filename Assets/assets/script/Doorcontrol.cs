using UnityEngine;
using System.Collections;

public class Doorcontrol : MonoBehaviour

{
    public GameObject prefab; // The prefab to rotate
    public string playerTag = "Player"; // Tag for the player object
    public float distanceThreshold = 5f; // Distance threshold for interaction
    public float rotationDuration = 0.1f; // Duration of the rotation
    private bool open = false; // State of the door (open or closed)
    private bool isRotating = false; // Flag to prevent concurrent rotations

  
    public void DoorActionServerRp()
    {
        // Check if the player is at the specified distance on the server
        if (IsPlayerAtDistance() && !isRotating)
        {
            // Notify clients to rotate the prefab
            DoorActionClientRp();
        }
    }

    
    public void DoorActionClientRp()
    {
        Debug.Log("Calling clients to rotate prefab");
        StartCoroutine(RotatePrefab90Degrees());
    }

    // Check if the player is at the specified distance
    bool IsPlayerAtDistance()
    {
        // Find all GameObjects with the specified tag.  This returns an array.
        GameObject[] playerObjects = GameObject.FindGameObjectsWithTag(playerTag);

        // If no player objects are found, return false immediately.
        if (playerObjects.Length == 0)
        {
            return false; // No player found
        }

        // Iterate through each player object found.
        foreach (GameObject playerObject in playerObjects)
        {
            // Calculate the distance between this GameObject and the current player GameObject.
            float distance = Vector3.Distance(transform.position, playerObject.transform.position);

            // If the distance is within the threshold, return true immediately.
            if (distance <= distanceThreshold)
            {
                return true; // At least one player is within the distance
            }

        }
        return false;
    }
        // Rotate the prefab 90 degrees
        IEnumerator RotatePrefab90Degrees()
    {
        isRotating = true;
        Quaternion startRotation = prefab.transform.rotation;
        Quaternion endRotation;
        if (!open)
        {
            endRotation = startRotation * Quaternion.Euler(0, 90, 0); // Rotate 90 degrees around Y-axis
        }
        else
        {
            endRotation = startRotation * Quaternion.Euler(0, -90, 0);
        }
        float elapsedTime = 0;
        while (elapsedTime < rotationDuration)
        {
            prefab.transform.rotation = Quaternion.Slerp(startRotation, endRotation, elapsedTime / rotationDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        open = !open;
        prefab.transform.rotation = endRotation; // Ensure the rotation is exact
        isRotating = false;
    }

    void OnDrawGizmos()
    {
        // Draw a gizmo to represent the interaction area
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, distanceThreshold);

        // Draw arrows to indicate the rotation direction
        Gizmos.color = Color.red;
        if (!open)
        {
            // Draw an arrow indicating rotation to open
            Vector3 start = prefab.transform.position;
            Vector3 end = prefab.transform.position + prefab.transform.right * 0.5f;
            Gizmos.DrawLine(start, end);
            Gizmos.DrawSphere(end, 0.05f);
        }
        else
        {
            // Draw an arrow indicating rotation to close
            Vector3 start = prefab.transform.position;
            Vector3 end = prefab.transform.position - prefab.transform.right * 0.5f;
            Gizmos.DrawLine(start, end);
            Gizmos.DrawSphere(end, 0.05f);
        }
    }


}
