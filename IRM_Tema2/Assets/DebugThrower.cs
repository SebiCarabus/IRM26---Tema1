using UnityEngine;

using UnityEngine.InputSystem; 

[RequireComponent(typeof(Rigidbody))]
public class DebugThrower : MonoBehaviour
{
    [Header("Dummy Throw")]
    public float throwForce = 15f; 
    public float spinForce = 50f;

    private Rigidbody rb;
    private UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable grabInteractable;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        grabInteractable = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();
    }

    void Update()
    {
        #if UNITY_EDITOR
        if (Keyboard.current != null && Keyboard.current.tKey.wasPressedThisFrame)
        {
            if (grabInteractable != null && grabInteractable.isSelected)
            {
                var interactor = grabInteractable.firstInteractorSelecting;
                grabInteractable.interactionManager.SelectCancel(interactor, grabInteractable);

                rb.AddForce(Camera.main.transform.forward * throwForce, ForceMode.Impulse);
                rb.AddTorque(Camera.main.transform.right * spinForce, ForceMode.Impulse);
            }

            
        }
        #endif
    }
}