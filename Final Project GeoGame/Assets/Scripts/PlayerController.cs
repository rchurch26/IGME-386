using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]

public class PlayerController : MonoBehaviour
{
    public Camera playerCam;
    public float walkSpeed = 6.0f;
    public float runSpeed = 12.0f;
    public float jumpPower = 7.0f;
    public float gravity = 9.81f;
    public float lookSpeed = 2.0f;
    public float lookXLimit = 45.0f;

    Vector3 moveDir = Vector3.zero;
    float rotationX = 0;

    public bool canMove = true;

    CharacterController control;
    // Start is called before the first frame update
    void Start()
    {
        control = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        //Movement
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);
        bool isRunning = Input.GetKey(KeyCode.LeftShift);
        float curSpeedX = canMove ? (isRunning ? runSpeed : walkSpeed) * Input.GetAxis("Vertical") : 0;
        float curSpeedY = canMove ? (isRunning ? runSpeed : walkSpeed) * Input.GetAxis("Horizontal") : 0;
        float movementDirY = moveDir.y;
        moveDir = (forward * curSpeedX) + (right * curSpeedY);

        //Jump
        if(Input.GetButton("Jump") && canMove && control.isGrounded)
        {
            moveDir.y = jumpPower;
        }
        else
        {
            moveDir.y = movementDirY;
        }

        if (!control.isGrounded)
        {
            moveDir.y -= gravity * Time.deltaTime;
        }

        //Cam Movement
        control.Move(moveDir * Time.deltaTime);
        if(canMove)
        {
            rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
            rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
            playerCam.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
            transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);
        }
    }
}
