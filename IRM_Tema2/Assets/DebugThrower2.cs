using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

[RequireComponent(typeof(Rigidbody))]
public class DebugThrower2 : MonoBehaviour
{
    [Header("Dummy Throw")]
    public float throwForce = 15f;
    public float spinForce = 50f;

    [Tooltip("Throw in the direction the controller points. If off, throws where the headset looks.")]
    public bool useControllerDirection = true;

    [Header("Input")]
    [Tooltip("Buttons that trigger the throw. You can add or change bindings here in the Inspector.")]
    [SerializeField] private InputAction throwAction = new InputAction("DebugThrow", InputActionType.Button);

    private Rigidbody rb;
    private XRGrabInteractable grabInteractable;

    private void Awake()
    {
        // Add default bindings only if none were set in the Inspector.
        if (throwAction.bindings.Count == 0)
        {
            throwAction.AddBinding("<Keyboard>/t");
            throwAction.AddBinding("<XRController>{RightHand}/{PrimaryButton}");
            throwAction.AddBinding("<XRController>{LeftHand}/{PrimaryButton}");
        }
    }

    private void OnEnable()
    {
        throwAction.performed += OnThrowPerformed;
        throwAction.Enable();
    }

    private void OnDisable()
    {
        throwAction.performed -= OnThrowPerformed;
        throwAction.Disable();
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        grabInteractable = GetComponent<XRGrabInteractable>();
    }

    private void OnThrowPerformed(InputAction.CallbackContext context)
    {
        if (grabInteractable == null)
        {
            return;
        }

        if (!grabInteractable.isSelected)
        {
            return;
        }

        var interactor = grabInteractable.firstInteractorSelecting;
        if (interactor == null)
        {
            return;
        }

        Vector3 forward;
        Vector3 right;

        if (useControllerDirection)
        {
            forward = interactor.transform.forward;
            right = interactor.transform.right;
        }
        else
        {
            forward = Camera.main.transform.forward;
            right = Camera.main.transform.right;
        }

        grabInteractable.interactionManager.SelectCancel(interactor, grabInteractable);

        rb.AddForce(forward * throwForce, ForceMode.Impulse);
        rb.AddTorque(right * spinForce, ForceMode.Impulse);
    }
}