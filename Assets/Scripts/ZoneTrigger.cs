using UnityEngine;

/// <summary>
/// This script should be attached to the trigger zones in the scene. 
/// It notifies the TutorialManager when a zone is triggered and disables the trigger to prevent multiple
/// </summary>
public class ZoneTrigger : MonoBehaviour
{
    public TutorialManager manager;
    public int zoneNumber; // Should be 1, 2, or 3
    
    /// <summary>
    /// When the player enters the trigger, notify the manager which zone was triggered and disable this
    /// trigger to prevent multiple activations. The manager will handle the logic of checking if it's the correct zone in order and activating the next zone or teleport area as needed.
    /// The trigger will remain visible but non-interactive to indicate it's completed.
    /// </summary>
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;
        
        // Notify manager which zone was triggered
        if (manager != null)
        {
            manager.ZoneTriggered(zoneNumber);
        }
        
        // Disable this trigger (but keep it visible with completed material)
        GetComponent<Collider>().enabled = false;
    }
}