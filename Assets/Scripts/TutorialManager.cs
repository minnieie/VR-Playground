using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; // Added this namespace

/// <summary>
/// This script manages the tutorial flow, including activating trigger zones in order, enabling teleport areas, and showing a congratulations message at the end.
/// </summary>
public class TutorialManager : MonoBehaviour
{
    [Header("Trigger Zones")]
    public GameObject zone1;
    public GameObject zone2;
    public GameObject zone3;
    
    [Header("Teleport Areas")]
    public GameObject teleportArea1;
    public GameObject teleportArea2;
    
    [Header("UI")]
    public GameObject congratsCanvas;
    public GameObject closeButton;
    
    [Header("Zone Materials")]
    public Material activeMaterial;
    public Material inactiveMaterial;
    public Material completedMaterial;
    
    [Header("Scene Management")] // Added this header
    [SerializeField] private string nextSceneName = "CA5"; // Next scene name
    [SerializeField] private float loadSceneDelay = 5f; // Delay before loading next scene
    
    private int currentZoneIndex = 1; // Tracks which zone should be triggered next
    private bool triggersCompleted = false;
    
    /// <summary>
    /// Initializes the tutorial by deactivating all zones and teleport areas, then activating the first zone.
    ///  Also sets up the close button for the congratulations message.
    /// </summary>
    void Start()
    {
        // Ensure all zones start correctly
        DeactivateAllZones();
        
        // Activate first zone
        if (zone1 != null)
        {
            zone1.SetActive(true);
            UpdateZoneVisual(zone1, activeMaterial);
        }
        
        // Ensure teleport areas start inactive
        if (teleportArea1 != null) 
        {
            teleportArea1.SetActive(false);
            UpdateZoneVisual(teleportArea1, inactiveMaterial);
        }
        if (teleportArea2 != null) 
        {
            teleportArea2.SetActive(false);
            UpdateZoneVisual(teleportArea2, inactiveMaterial);
        }
        
        // Hide congratulations
        if (congratsCanvas != null)
            congratsCanvas.SetActive(false);
        
        Button closeBtn = GameObject.Find("CloseButton")?.GetComponent<Button>();
        if (closeBtn != null)
        {
            closeBtn.onClick.AddListener(CloseCongratsMessage);
        }   
    }
    
    /// <summary>
    /// Deactivates all trigger zones and teleport areas, setting their visuals to inactive. This is called at the start to ensure a clean state.
    /// </summary>
    void DeactivateAllZones()
    {
        if (zone1 != null) 
        {
            zone1.SetActive(false);
            UpdateZoneVisual(zone1, inactiveMaterial);
        }
        if (zone2 != null) 
        {
            zone2.SetActive(false);
            UpdateZoneVisual(zone2, inactiveMaterial);
        }
        if (zone3 != null) 
        {
            zone3.SetActive(false);
            UpdateZoneVisual(zone3, inactiveMaterial);
        }
    }
    
    /// <summary>
    /// Updates the visual material of a zone or teleport area to indicate its state (active, inactive, completed). This helps guide the player through the tutorial steps.
    /// </summary>
    void UpdateZoneVisual(GameObject zone, Material material)
    {
        Renderer renderer = zone.GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.material = material;
        }
    }
    
    /// <summary>
    /// Handles the logic when a trigger zone is entered. 
    /// It checks if the correct zone was triggered in order, updates visuals, and activates the next zone or teleport area as needed. 
    /// If all zones are completed, it enables the first teleport area.
    /// </summary>
    public void ZoneTriggered(int zoneNumber)
    {
        // Check if this is the correct next zone
        if (zoneNumber == currentZoneIndex)
        {
            // Mark current zone as completed
            GameObject currentZone = GetZoneByNumber(zoneNumber);
            if (currentZone != null)
            {
                UpdateZoneVisual(currentZone, completedMaterial);
            }
            
            // Increment to next zone
            currentZoneIndex++;
            
            // Activate next zone if available
            if (currentZoneIndex <= 3)
            {
                GameObject nextZone = GetZoneByNumber(currentZoneIndex);
                if (nextZone != null)
                {
                    nextZone.SetActive(true);
                    UpdateZoneVisual(nextZone, activeMaterial);
                }
            }
            
            // Check if all 3 trigger zones are completed
            if (currentZoneIndex > 3)
            {
                triggersCompleted = true;
                EnableTeleportArea1();
            }
        }
        else
        {
            Debug.Log("Wrong order! You need to complete zone " + currentZoneIndex + " first.");
        }
    }
    
    /// <summary>
    /// Returns the corresponding zone GameObject based on the zone number. This is used to update visuals and manage activation of zones in order.
    /// </summary>
    GameObject GetZoneByNumber(int number)
    {
        switch (number)
        {
            case 1: return zone1;
            case 2: return zone2;
            case 3: return zone3;
            default: return null;
        }
    }
    
    /// <summary>
    /// Enables the first teleport area after all trigger zones are completed. 
    /// This allows the player to progress to the next part of the tutorial.
    ///  It also updates the visual to indicate it's active.
    /// </summary>
    public void EnableTeleportArea1()
    {   
        // Enable teleport area 1
        if (teleportArea1 != null && triggersCompleted)
        {
            teleportArea1.SetActive(true);
            UpdateZoneVisual(teleportArea1, activeMaterial);
        }
    }
    
    /// <summary>
    /// Handles the logic when a teleport area is triggered. 
    /// It checks if the trigger zones are completed first, then updates visuals for the teleport areas and shows the congratulations message when the second teleport area is triggered.
    /// </summary>
    public void TeleportAreaTriggered(int areaNumber)
    {
        // Check if triggers are completed first
        if (!triggersCompleted)
        {
            Debug.Log("Complete all trigger zones first!");
            return;
        }
        
        // Update visuals based on which teleport area was triggered
        if (areaNumber == 1)
        {
            // Mark teleport 1 as completed
            if (teleportArea1 != null)
            {
                UpdateZoneVisual(teleportArea1, completedMaterial);
            }
            
            // Enable teleport area 2
            if (teleportArea2 != null)
            {
                teleportArea2.SetActive(true);
                UpdateZoneVisual(teleportArea2, activeMaterial);
            }
        }
        else if (areaNumber == 2)
        {
            // Mark teleport 2 as completed
            if (teleportArea2 != null)
            {
                UpdateZoneVisual(teleportArea2, completedMaterial);
            }
            
            // Show congratulations
            ShowCongrats();
            
            // Load next scene after delay
            Invoke(nameof(LoadNextScene), loadSceneDelay);
        }
    }
    
    /// <summary>
    /// Displays the congratulations message on the canvas when the player completes the tutorial by triggering the second teleport area.
    /// </summary>
    public void ShowCongrats()
    {
        if (congratsCanvas != null)
        {
            congratsCanvas.SetActive(true);
            
            // Make sure close button is active
            if (closeButton != null)
                closeButton.SetActive(true);
        }
    }
    
    /// <summary>
    /// Closes the congratulations message when the close button is pressed.
    ///  This allows the player to exit the tutorial completion message and continue with the rest of the experience.
    /// </summary>
    public void CloseCongratsMessage()
    {
        if (congratsCanvas != null)
        {
            congratsCanvas.SetActive(false);
        }
        
        // Cancel scene loading if user closes the message
        CancelInvoke(nameof(LoadNextScene));
    }
    
    /// <summary>
    /// Loads the next scene (CA5)
    /// </summary>
    private void LoadNextScene()
    {
        Debug.Log("Loading next scene: " + nextSceneName);
        SceneManager.LoadScene(nextSceneName);
    }
}