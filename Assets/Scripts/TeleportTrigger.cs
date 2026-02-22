using UnityEngine;

/// <summary>
/// This script should be attached to teleport areas in the scene.
/// It notifies the TutorialManager when a teleport area is triggered in the correct order.
/// </summary>
public class TeleportTrigger : MonoBehaviour
{
    public TutorialManager manager;
    public int teleportIndex; // Should be 1 or 2
    
    private bool completed = false;

    private void OnTriggerEnter(Collider other)
    {
        if (completed) return;
        if (!other.CompareTag("Player")) return;

        // Notify manager which teleport area was triggered
        if (manager != null)
        {
            manager.TeleportAreaTriggered(teleportIndex);
        }
        
        // Disable this trigger (but keep it visible with completed material)
        GetComponent<Collider>().enabled = false;
        completed = true;
    }
}