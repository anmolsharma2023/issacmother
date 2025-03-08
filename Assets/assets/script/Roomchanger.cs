using UnityEngine;

public class Roomchanger : MonoBehaviour
{
    public Transform[] transforms; // Array of transforms to check

    // Function to handle collision
    void OnCollisionEnter(Collision collision)
    {
        // Check if the collision is with the desired GameObject
        if (collision.gameObject.CompareTag("Player"))
        {
            // Run your function here
            RunFunctionWithGizmos(collision.gameObject);
        }
    }

    // Example function that uses gizmos
    void RunFunctionWithGizmos(GameObject gg)
    {
        // Example gizmo drawing
        Debug.DrawRay(transform.position, Vector3.up * 5, Color.red, 1f);

        // Loop through each transform in the array
        foreach (Transform t in transforms)
        {
            // Check if there is no GameObject at the transform's position
            if (!IsObjectAtPosition(t.position))
            {
                // Select this transform if no object is present
                Vector3 newpos = t.position;
                newpos.y=gg.transform.position.y;
                gg.transform.position = newpos;
            }
        }
    }
    // Function to check if a GameObject exists at a given position
    bool IsObjectAtPosition(Vector3 position)
    {
        // Use a sphere cast to detect objects at the position
        Collider[] hits = Physics.OverlapSphere(position, 0.1f); // Adjust the radius as needed
        return hits.Length > 0;
    }

    // Function to select a transform (e.g., for further processing or visualization)
  

    // Optional: Draw gizmos in the editor for debugging
  
    // Optional: Draw gizmos in the editor for debugging
    void OnDrawGizmos()
    {
        foreach (Transform t in transforms)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(t.position, 0.5f);
        }
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, 1f);
    }
}
