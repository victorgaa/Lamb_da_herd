using PixPlays.ElementalVFX;
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

    [SerializeField] Animator _Anim;
    [SerializeField] BindingPoints _BindingPoints;
    [SerializeField] Transform _Target;

    private AnimatorOverrideController _overrideController;
    public BindingPoints BindingPoints => _BindingPoints;

    private float rotateToFaceMovementSpeed = 5f;
    private float rotateToFaceAwayFromCameraSpeed = 5f;

    private float speed = 9.0f;         // XZ movement speed
    private float rotationSpeed = 720f; // rotation sensitivity

    private float gravity = -9.81f;     // default gravity (this will change)
    private float yVelocity = 0f;       // current y Velocity
    private float yVelocityWhenGrounded = -4f;  // this ensures cc.isGrounded will work 


    private void Start()
    {
        if (_Anim.runtimeAnimatorController != null)
        {
            _overrideController = new AnimatorOverrideController(_Anim.runtimeAnimatorController);
            _Anim.runtimeAnimatorController = _overrideController;
        }
    }

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

    public void PlayAnimation(string clipId, AnimationClip clip)
    {
        if (_overrideController != null)
        {
            _overrideController[clipId] = clip;
            _Anim.SetTrigger("Play");
        }
    }

    public Vector3 GetTarget()
    {
        Vector3 direction = (_Target.position - transform.position).normalized;
        Ray ray = new Ray(transform.position, direction);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100))
        {
            return hit.point;
        }
        return _Target.position;
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
