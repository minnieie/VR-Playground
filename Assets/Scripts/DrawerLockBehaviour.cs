using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

/// <summary>
/// Manages drawer unlocking functionality when a key object is inserted into a socket.
/// Listens for socket insertion events and modifies the drawer's joint constraints accordingly.
/// </summary>
public class DrawerLockMechanism : MonoBehaviour
{
    [Header("Drawer Configuration")]
    [Tooltip("The ConfigurableJoint that controls drawer movement")]
    [SerializeField] private ConfigurableJoint drawerJoint;
    
    [Tooltip("Maximum drawer extension distance when unlocked (in meters)")]
    [SerializeField] private float unlockedExtensionLimit = 0.6f;

    /// <summary>
    /// Reference to the XR socket interactor component on this GameObject
    /// </summary>
    private XRSocketInteractor keySocket;

    /// <summary>
    /// Initializes component by caching the socket reference
    /// </summary>
    void Awake()
    {
        keySocket = GetComponent<XRSocketInteractor>();
    }

    /// <summary>
    /// Subscribes to socket insertion events when the component is enabled
    /// </summary>
    void OnEnable()
    {
        keySocket.selectEntered.AddListener(HandleKeyInserted);
    }

    /// <summary>
    /// Unsubscribes from socket events when the component is disabled
    /// </summary>
    void OnDisable()
    {
        keySocket.selectEntered.RemoveListener(HandleKeyInserted);
    }

    /// <summary>
    /// Callback triggered when an object is inserted into the socket
    /// </summary>
    /// <param name="eventArgs">Event data containing information about the selected object</param>
    private void HandleKeyInserted(SelectEnterEventArgs eventArgs)
    {
        ReleaseDrawerLock();
    }

    /// <summary>
    /// Releases the drawer lock by enabling limited motion and setting the extension limit
    /// </summary>
    private void ReleaseDrawerLock()
    {
        // Enable limited linear motion for the drawer
        drawerJoint.zMotion = ConfigurableJointMotion.Limited;

        // Update the linear limit to allow drawer extension
        SoftJointLimit currentLimit = drawerJoint.linearLimit;
        currentLimit.limit = unlockedExtensionLimit;
        drawerJoint.linearLimit = currentLimit;
    }
}