using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public float moveSpeed = 4.0f;

    void FixedUpdate()
    {
        KeyboardMovement();
    }

    void KeyboardMovement ()
    {
        Vector3 movement = new Vector3(0, 0, 0);
        float inputHorizontal = Input.GetAxis("Horizontal");
        float inputVertical = Input.GetAxis("Vertical");
        Vector3 cameraForward = Camera.main.transform.forward;
        Vector3 cameraRight = Camera.main.transform.right;
        cameraForward = inputVertical * cameraForward;
        cameraRight = inputHorizontal * cameraRight;
        movement = cameraForward + cameraRight;
        movement.y = 0;
        transform.Translate(movement * moveSpeed * Time.deltaTime);
    }
}
