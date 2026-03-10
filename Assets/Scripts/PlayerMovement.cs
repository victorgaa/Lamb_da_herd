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

    private float rotateToFaceMovementSpeed = 5f;
    private float rotateToFaceAwayFromCameraSpeed = 5f;
    private float speed = 9.0f;         // XZ movement speed

    void Update()
    {
        // determine XZ movement vector
        float horizInput = Input.GetAxis("Horizontal");
        float vertInput = Input.GetAxis("Vertical");
        Vector3 movement = new Vector3(horizInput, 0, vertInput);

        // ensure diagonal movement doesn't exceed horiz/vert movement speed
        movement = Vector3.ClampMagnitude(movement, 1.0f);

        anim.SetFloat("velocity", movement.magnitude);

        // convert from local to global coordinates
        movement = transform.TransformDirection(movement);

        if (movement.magnitude > 0)
        {
            RotateModelToFaceMovement(movement);
            RotatePlayerToFaceAwayFromCamera();
        }

        movement *= speed;
        movement *= Time.deltaTime; // make all movement processor independent
        cc.Move(movement);
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
