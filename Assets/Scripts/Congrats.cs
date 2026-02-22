using UnityEngine;

public class Congrats : MonoBehaviour
{
    [Header("Socket References")]
    [Tooltip("First socket that needs to be filled")]
    public UnityEngine.XR.Interaction.Toolkit.Interactors.XRSocketInteractor firstSocket;
    
    [Tooltip("Second socket that needs to be filled")]
    public UnityEngine.XR.Interaction.Toolkit.Interactors.XRSocketInteractor secondSocket;
    
    [Tooltip("Third socket that needs to be filled")]
    public UnityEngine.XR.Interaction.Toolkit.Interactors.XRSocketInteractor thirdSocket;

    [Header("UI References")]
    [Tooltip("The congratulations UI panel to display when puzzle is solved")]
    public GameObject completionUIPanel;

    /// <summary>
    /// Tracks whether the puzzle has been completed to prevent repeated execution
    /// </summary>
    private bool puzzleCompleted = false;

    /// <summary>
    /// Initializes the component by ensuring the completion UI is hidden at start
    /// </summary>
    void Start()
    {
        if (completionUIPanel != null)
            completionUIPanel.SetActive(false);
    }

    /// <summary>
    /// Checks every frame if all sockets are filled and triggers puzzle completion
    /// Only executes if the puzzle hasn't been solved yet
    /// </summary>
    void Update()
    {
        if (puzzleCompleted) return;

        if (IsSocketOccupied(firstSocket) &&
            IsSocketOccupied(secondSocket) &&
            IsSocketOccupied(thirdSocket))
        {
            HandlePuzzleCompletion();
        }
    }

    /// <summary>
    /// Determines whether a specific socket currently has an object inserted
    /// </summary>
    /// <param name="socket">The socket interator to check</param>
    /// <returns>True if the socket exists and has a selection, false otherwise</returns>
    bool IsSocketOccupied(UnityEngine.XR.Interaction.Toolkit.Interactors.XRSocketInteractor socket)
    {
        return socket != null && socket.hasSelection;
    }

    /// <summary>
    /// Handles the logic when the puzzle is successfully completed
    /// Marks the puzzle as solved and displays the congratulations UI
    /// </summary>
    void HandlePuzzleCompletion()
    {
        puzzleCompleted = true;

        if (completionUIPanel != null)
            completionUIPanel.SetActive(true);
    }
}