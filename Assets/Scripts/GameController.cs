using UnityEngine;
using TMPro;

public class GameController : MonoBehaviour
{
    public GameObject player;
    public TextMeshProUGUI distanceText;

    private GameObject[] pickups;
    private LineRenderer lineRenderer;

    private enum DebugMode
    {
        Normal,
        Distance,
        Vision
    }

    private DebugMode currentMode = DebugMode.Normal;

    void Start()
    {
        pickups = GameObject.FindGameObjectsWithTag("Pickup");

        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.positionCount = 2;
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;
        lineRenderer.enabled = false;
    }

    void Update()
    {
        // Change debug mode when Space is pressed
        if (Input.GetKeyDown(KeyCode.Space))
        {
            currentMode++;

            if ((int)currentMode > 2)
            {
                currentMode = DebugMode.Normal;
            }
        }

        GameObject closestPickup = null;
        GameObject visionPickup = null;

        float closestDistance = Mathf.Infinity;
        float bestDirection = -1f;

        Vector3 playerPosition = player.transform.position;
        Vector3 playerVelocity =
            player.GetComponent<Rigidbody>().velocity;

        // Reset all remaining pickups to white
        foreach (GameObject pickup in pickups)
        {
            if (pickup.activeSelf)
            {
                pickup.GetComponent<Renderer>().material.color = Color.white;

                Vector3 toPickup =
                    pickup.transform.position - playerPosition;

                float distance = toPickup.magnitude;

                // Find closest pickup
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestPickup = pickup;
                }

                // Find pickup most directly in player's movement direction
                if (playerVelocity.magnitude > 0.01f)
                {
                    float direction = Vector3.Dot(
                        playerVelocity.normalized,
                        toPickup.normalized
                    );

                    if (direction > bestDirection)
                    {
                        bestDirection = direction;
                        visionPickup = pickup;
                    }
                }
            }
        }

        // NORMAL MODE
        if (currentMode == DebugMode.Normal)
        {
            lineRenderer.enabled = false;

            if (distanceText != null)
            {
                distanceText.text = "";
            }
        }

        // DISTANCE MODE
        else if (currentMode == DebugMode.Distance)
        {
            if (closestPickup != null)
            {
                closestPickup.GetComponent<Renderer>()
                    .material.color = Color.blue;

                lineRenderer.enabled = true;

                lineRenderer.SetPosition(
                    0,
                    playerPosition
                );

                lineRenderer.SetPosition(
                    1,
                    closestPickup.transform.position
                );

                if (distanceText != null)
                {
                    distanceText.text =
                        "Distance: " +
                        closestDistance.ToString("0.00");
                }
            }
        }

        // VISION MODE
        else if (currentMode == DebugMode.Vision)
        {
            lineRenderer.enabled = true;

            // Draw player's velocity
            lineRenderer.SetPosition(
                0,
                playerPosition
            );

            lineRenderer.SetPosition(
                1,
                playerPosition + playerVelocity
            );

            if (distanceText != null)
            {
                distanceText.text = "";
            }

            if (visionPickup != null)
            {
                // Pickup player is approaching most directly = green
                visionPickup.GetComponent<Renderer>()
                    .material.color = Color.green;

                // Green pickup faces the player
                visionPickup.transform.LookAt(
                    player.transform
                );
            }
        }
    }
}