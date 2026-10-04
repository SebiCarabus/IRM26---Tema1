using UnityEngine;

public class Attack_retreat : MonoBehaviour
{
    public Transform other;
    public float attackDistance = 0.25f;

    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (other != null)
        {
            float distance = Vector3.Distance(transform.position, other.position);

            if (distance <= attackDistance)
            {
                animator.SetBool("Attacking", true);
            }
            else
            {
                animator.SetBool("Attacking", false);
            }
        }
    }
}
