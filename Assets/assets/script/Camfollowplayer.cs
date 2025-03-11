using UnityEngine;

public class Camfollowplayer : MonoBehaviour
{
    public GameObject player; // Reference to the player object
    public Vector3 originalPosition; // Original position of the camera
    public float maxDistance = 5f; // Maximum distance from the original position
    public float smoothSpeed = 0.3f; // Speed of camera movement

    private Vector3 offset; // Offset from the player

    void Start()
    {// Store the original camera position
        //originalPosition = transform.position;

        if (player != null)
        {
            // Calculate the initial offset
            offset = transform.position - player.transform.position;
        }
        else
        {
            // Define the tag you want to search for
            string tagToFind = "Player";

            // Find all GameObjects with the specified tag
            GameObject[] objectsWithTag = GameObject.FindGameObjectsWithTag(tagToFind);
            Debug.Log(objectsWithTag.Length);
            // Do something with the found objects
            foreach (GameObject obj in objectsWithTag)
            {
               Issacprops issacprops = obj.GetComponent<Issacprops>();
                if (issacprops.iamowner)
                {
                    player=obj;
                }
            }
        }
    }

    void LateUpdate()
    {  
        if(player != null) 
        { // Calculate the target position for the camera
            Vector3 targetPosition = player.transform.position + offset;

            // Ensure the camera stays within the max distance from its original position
            Vector3 constrainedPosition = ConstrainDistance(targetPosition, originalPosition, maxDistance);

            // Smoothly move the camera to the constrained position
            transform.position = Vector3.Lerp(transform.position, new Vector3(constrainedPosition.x, constrainedPosition.y, transform.position.z), smoothSpeed * Time.deltaTime);

            // Keep the camera's Z position constant (only move in X-Y plane)
            transform.position = new Vector3(transform.position.x, transform.position.y, originalPosition.z);
        }
}
    // Function to constrain the camera's distance from its original position
    Vector3 ConstrainDistance(Vector3 target, Vector3 origin, float maxDistance)
    {
        Vector3 direction = target - origin;
        float distance = direction.magnitude;

        if (distance > maxDistance)
        {
            return origin + direction.normalized * maxDistance;
        }
        else
        {
            return target;
        }
    }
}
