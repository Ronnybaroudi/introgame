using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class PlayerController : MonoBehaviour {
    public Vector2 moveValue;
    public float speed;
    private int count;
    private int numPickups = 8;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI winText;
    public TextMeshProUGUI playerPosition;
    public TextMeshProUGUI playerVelocity;
    private Vector3 oldPosition;

    void Start() {
        count = 0;
        winText.text = "";
        SetCountText();
        oldPosition = transform.position;
    }

    void OnMove(InputValue value) {
        moveValue = value.Get<Vector2>();
    }

    private void Update() {
        Vector3 velocity = (transform.position - oldPosition) / Time.deltaTime;

        if (playerPosition != null)
            playerPosition.text = "Position: " + transform.position.ToString("0.00");

        if (playerVelocity != null)
            playerVelocity.text = "Velocity: " + velocity.ToString("0.00") +
                                  " Speed " + velocity.magnitude.ToString("0.00");

        oldPosition = transform.position;
    }


    void FixedUpdate() {
        Vector3 movement = new Vector3(moveValue.x, 0.0f, moveValue.y);
        GetComponent<Rigidbody>().AddForce(movement * speed * Time.fixedDeltaTime);
    }

    void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "Pickup") {
            other.gameObject.SetActive(false);
            count++;
            SetCountText();
        }
    }

    private void SetCountText() {
        scoreText.text = "Score: " + count.ToString();
        if (count >= numPickups) {
            winText.text = "You Win!";
        }
    }
}