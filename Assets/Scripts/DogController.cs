using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogController : MonoBehaviour
{
        public KeyCode forward = KeyCode.W; // Move forward
    public KeyCode backward = KeyCode.S; // Move backward
    public KeyCode left = KeyCode.A; // Move left
    public KeyCode right = KeyCode.D; // Move right
    public KeyCode run = KeyCode.LeftShift; // Run
    public KeyCode jump = KeyCode.Space; // Jump
    public KeyCode attack = KeyCode.Mouse0; // Attack
    public KeyCode bark = KeyCode.B; // Bark
    public KeyCode sniff = KeyCode.N; // Sniff

    private Animator dogAnim;
    private float currentSpeed;
    private float maxWalk = 0.5f;
    private float maxRun = 1.0f;
    private float acceleration = 1.0f;
    private float deceleration = 1.0f;
    private float w_movement = 0.0f;

    void Start()
    {
        dogAnim = GetComponent<Animator>(); // Get the animation component
        currentSpeed = 0.0f;
    }

    void Update()
    {
        HandleMovement();
        HandleRotation();
        HandleActions();
    }

    void HandleMovement()
    {
        bool walkPressed = Input.GetKey(forward);
        bool backwardPressed = Input.GetKey(backward);
        bool runPressed = Input.GetKey(run);

        if (runPressed)
        {
            currentSpeed = maxRun;
        }
        else
        {
            currentSpeed = maxWalk;
        }

        if (walkPressed && (w_movement < currentSpeed)) // If walking forward
        {
            w_movement += Time.deltaTime * acceleration;
        }
        else if (backwardPressed && (w_movement > -currentSpeed)) // If walking backward
        {
            w_movement -= Time.deltaTime * acceleration;
        }
        else if (!walkPressed && !backwardPressed) // Slow down to stop
        {
            w_movement = Mathf.Lerp(w_movement, 0.0f, Time.deltaTime * deceleration);
        }

        dogAnim.SetFloat("Movement_f", Mathf.Abs(w_movement)); // Set movement speed for all required parameters

        Vector3 forwardMovement = transform.forward * w_movement * Time.deltaTime;
        transform.position += forwardMovement;
    }

    void HandleRotation()
    {
        bool leftTurn = Input.GetKey(left);
        bool rightTurn = Input.GetKey(right);

        if (leftTurn)
        {
            transform.Rotate(Vector3.up * Time.deltaTime * -45, Space.Self);
        }
        else if (rightTurn)
        {
            transform.Rotate(Vector3.up * Time.deltaTime * 45, Space.Self);
        }
    }

    void HandleActions()
    {
        if (Input.GetKeyDown(attack))
        {
            dogAnim.SetTrigger("Attack_tr");
        }

        if (Input.GetKeyDown(bark))
        {
            dogAnim.SetTrigger("Bark_tr");
        }

        if (Input.GetKeyDown(sniff))
        {
            dogAnim.SetTrigger("Sniff_tr");
        }

        if (Input.GetKeyDown(jump))
        {
            dogAnim.SetTrigger("Jump_tr");
        }
    }
}
