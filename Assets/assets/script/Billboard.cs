using UnityEngine;
using Unity.Netcode;
public class Billboard : NetworkBehaviour
{
    // The tag for the sprites to billboard
    public string spriteTag = "pp";

    // Update is called once per frame
    void LateUpdate()
    {
        if (!IsOwner)
        {
            return;
        }
        // Find all sprites with the tag "pp"
        GameObject[] sprites = GameObject.FindGameObjectsWithTag(spriteTag);

        // Loop through each sprite
        foreach (GameObject sprite in sprites)
        {
            // Ensure the sprite is a child of a GameObject
            if (sprite.transform.parent != null && IsOwner)
            {
                Vector3 meme = Camera.main.transform.position;

                meme.z = -1f * (sprite.transform.position.z);

                // Calculate the direction from the sprite to the camera
                Vector3 direction = (meme - sprite.transform.position).normalized;

                // Calculate the angle needed to rotate the sprite around the X-axis
                float angle = Mathf.Atan2(direction.y, direction.z);

                // Apply the rotation while keeping Y and Z rotations unchanged
                sprite.transform.rotation = Quaternion.Euler(angle * Mathf.Rad2Deg, sprite.transform.eulerAngles.y, sprite.transform.eulerAngles.z);
            }
        }
    }
}
