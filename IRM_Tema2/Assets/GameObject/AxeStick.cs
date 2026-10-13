using UnityEngine;

public class AxeStick : MonoBehaviour
{
    private Rigidbody body;
    private bool isStuck = false;

    private void Awake()
    {
        body = GetComponent<Rigidbody>();
    }

    public void Stick()
    {
        if (isStuck)
        {
            return;
        }

        body.linearVelocity = Vector3.zero;
        body.angularVelocity = Vector3.zero;
        body.constraints = RigidbodyConstraints.FreezeAll;
        isStuck = true;
    }

    public void Release()
    {
        body.constraints = RigidbodyConstraints.None;
        isStuck = false;
    }
}