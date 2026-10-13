using System.Collections.Generic;
using UnityEngine;

public class TargetScore : MonoBehaviour
{
    [Tooltip("Radius of the target face in world units (meters). Radius = Scale X / 2 for a default cylinder.")]
    [SerializeField] private float radius = 0.5f;

    [Tooltip("Number of scoring rings. A bullseye gives this many points.")]
    [SerializeField] private int rings = 10;

    [Tooltip("Tag of the object that has the axe's Rigidbody.")]
    [SerializeField] private string axeTag = "Axe";

    [Tooltip("Minimum impact speed so a gentle touch does not score.")]
    [SerializeField] private float minImpactSpeed = 1.0f;

    [Tooltip("Seconds before the same axe can score again.")]
    [SerializeField] private float rescoreCooldown = 1.5f;

    [Tooltip("Freeze the axe in place when it scores.")]
    [SerializeField] private bool stickAxes = true;

    private readonly Dictionary<Rigidbody, float> lastScoreTimes = new Dictionary<Rigidbody, float>();

    private void OnCollisionEnter(Collision collision)
    {
        Rigidbody axeBody = collision.rigidbody;
        if (axeBody == null)
        {
            return;
        }

        if (!axeBody.CompareTag(axeTag))
        {
            return;
        }

        if (collision.relativeVelocity.magnitude < minImpactSpeed)
        {
            return;
        }

        float lastTime;
        if (lastScoreTimes.TryGetValue(axeBody, out lastTime))
        {
            if (Time.time - lastTime < rescoreCooldown)
            {
                return;
            }
        }

        Vector3 hitPoint = collision.GetContact(0).point;
        int points = CalculatePoints(hitPoint);
        lastScoreTimes[axeBody] = Time.time;

        Debug.Log("Axe hit the target for " + points + " points");

        if (ScoreManager.Instance != null)
        {
            ScoreManager.Instance.AddScore(points);
        }
        else
        {
            Debug.LogWarning("No ScoreManager in the scene.");
        }

        if (stickAxes)
        {
            AxeStick stick = axeBody.GetComponent<AxeStick>();
            if (stick != null)
            {
                stick.Stick();
            }
        }
    }

    private int CalculatePoints(Vector3 hitPoint)
    {
        Vector3 offset = hitPoint - transform.position;
        Vector3 flatOffset = Vector3.ProjectOnPlane(offset, transform.up);
        float distance = flatOffset.magnitude;

        float ratio = distance / radius;
        if (ratio >= 1f)
        {
            return 0;
        }

        int points = rings - Mathf.FloorToInt(ratio * rings);
        return points;
    }

    public void ResetTarget()
    {
        lastScoreTimes.Clear();
    }
}