using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private CharacterController cc;
    [SerializeField] private Animator anim;
    [SerializeField] private GameObject model;
    [SerializeField] private Camera cam;
    [SerializeField] private UIManager uiManager;
    [SerializeField] private InputType inputType = InputType.KeyboardMouse;

    private float rotateToFaceMovementSpeed = 5f;
    private float rotateToFaceAwayFromCameraSpeed = 5f;
    private float speed = 9.0f;         // XZ movement speed
    private float verticalVelocity = 0f;
    private float gravity = -9.81f;
    void Update()
    {
        if (!uiManager.IsGameActive) 
        { 
            return;
        }

        float horizInput = 0f;
        float vertInput = 0f;

        if (inputType == InputType.KeyboardMouse)
        {
            horizInput = Input.GetAxis("Horizontal");
            vertInput = Input.GetAxis("Vertical");
        }
        else if (inputType == InputType.Gamepad)
        {
            horizInput = Input.GetAxis("Gamepad_Horizontal");
            vertInput = Input.GetAxis("Gamepad_Vertical");
        }
        Vector3 movement = new Vector3(horizInput, 0, vertInput);

        movement = Vector3.ClampMagnitude(movement, 1.0f);

        anim.SetFloat("velocity", movement.magnitude);

        movement = transform.TransformDirection(movement);

        if (movement.magnitude > 0)
        {
            RotateModelToFaceMovement(movement);
            RotatePlayerToFaceAwayFromCamera();
        }
        else if(Input.GetMouseButton(0))
        {
            RotatePlayerToFaceAwayFromCamera();
        }

        // Apply speed
        movement *= speed;

        // Apply gravity
        if (cc.isGrounded)
        {
            verticalVelocity = -0.5f; // small downward force to keep grounded
        }
        else
        {
            verticalVelocity += gravity * Time.deltaTime;
        }

        movement.y = verticalVelocity;

        cc.Move(movement * Time.deltaTime);
    }


    // Set the rotation of the model to match the direction of the movement vector
    private void RotateModelToFaceMovement(Vector3 moveDirection)
    {
        Quaternion newRotation = Quaternion.LookRotation(moveDirection);
        model.transform.rotation = Quaternion.Slerp(model.transform.rotation, newRotation, rotateToFaceMovementSpeed * Time.deltaTime);
    }

    // set the player's Y rotation (yaw) to be aligned with the camera's Y rotation
    private void RotatePlayerToFaceAwayFromCamera()
    {
        Quaternion camRotation = Quaternion.Euler(0, cam.transform.rotation.eulerAngles.y, 0);
        transform.rotation = Quaternion.Slerp(transform.rotation, camRotation, rotateToFaceAwayFromCameraSpeed * Time.deltaTime);
    }

    public enum InputType
    {
        KeyboardMouse,
        Gamepad
    }
    private void OnDrawGizmos()
    {
        // make the source 1.5 units up from the player's pivot point (at their chest instead of their feet)
        Vector3 source = transform.position + Vector3.up * 1.5f;

        // visualize what direction the model is facing (blue line)
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(source, source + (model.transform.forward * 3f));

        // visualize what direction the Player is facing (red line)
        Gizmos.color = Color.red;
        Gizmos.DrawLine(source, source + (transform.forward * 3f));
    }
}
