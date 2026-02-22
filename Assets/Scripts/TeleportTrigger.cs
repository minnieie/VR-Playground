using UnityEngine;

/// <summary>
/// This script should be attached to the teleport trigger areas in the scene. 
/// It handles teleporting the player to the specified destination and notifying the TutorialManager when a teleport area is triggered.
/// </summary>
public class TeleportTrigger : MonoBehaviour
{
    public TutorialManager manager;
    public int teleportNumber; // Should be 1 or 2
    public Transform teleportDestination;
    
    /// <summary>
    /// When the player enters the trigger, teleport them to the destination and notify the manager.
    ///  Then disable this trigger to prevent multiple activations.
    /// </summary>
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;
        
        // Teleport the player
        if (teleportDestination != null)
        {
            // Find the XR Origin (the real root that needs to move)
            GameObject xrOrigin = GameObject.Find("XR Origin");
            if (xrOrigin == null)
                xrOrigin = GameObject.Find("XR Origin (XR Rig)");
            
            if (xrOrigin != null)
            {
                // Get the camera and its local offset
                Transform cameraTransform = Camera.main.transform;
                Vector3 cameraLocalPos = cameraTransform.localPosition;
                
                // Calculate the new position for the XR Origin based on the teleport destination and camera offset
                Vector3 newOriginPos = teleportDestination.position - cameraLocalPos;
                
                // Teleport the XR Origin
                xrOrigin.transform.position = newOriginPos;
                xrOrigin.transform.rotation = teleportDestination.rotation;
                
            }
            else
            {
                // if we can't find the XR Origin, just move the player directly (may cause issues with VR rigs)
                other.transform.position = teleportDestination.position;
                other.transform.rotation = teleportDestination.rotation;
                
            }
        }
        
        // Notify manager
        if (manager != null)
        {
            manager.TeleportAreaTriggered(teleportNumber);
        }
        
        // Disable this trigger
        GetComponent<Collider>().enabled = false;
    }
}