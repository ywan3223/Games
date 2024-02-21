using System;
using System.Collections;
using System.Collections.Generic;

using System.Numerics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    public float speed;
    public float rotationSpeed;

    private CharacterController characterController;
    private Animator animator;

    public float keynumber;
    public OpenDoor door1;
    public OpenDoor door2;
    public OpenDoor door3;

    void Start()
    {
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
    }
    void Update()
    {
        if (keynumber == 1)
        {
            door1.move();
        }
        if (keynumber == 2)
        {
            door2.move();
        }
        if (keynumber == 3)
        {
            door3.move();
        }
    }
 
    void FixedUpdate()
    {
        
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        UnityEngine.Vector3 movementDirection = new UnityEngine.Vector3(horizontalInput, 0, verticalInput);
        float magnitude = Mathf.Clamp01(movementDirection.magnitude) * speed;
        movementDirection.Normalize();

        characterController.SimpleMove(movementDirection * magnitude);

        if (movementDirection != UnityEngine.Vector3.zero)
        {
            animator.SetBool("IsMoving", true);
            UnityEngine.Quaternion toRotation = UnityEngine.Quaternion.LookRotation(movementDirection, UnityEngine.Vector3.up);

            transform.rotation = UnityEngine.Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
        }
        else
        {
            animator.SetBool("IsMoving", false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Finish")
        {
            Debug.Log("Finish!");
            GameObject.Find("boy").SendMessage("Finish");
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
        }
    }


}