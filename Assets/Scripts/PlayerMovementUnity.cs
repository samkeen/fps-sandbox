using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This is from the Unity example for CharacterController.Move():
///   https://docs.unity3d.com/ScriptReference/CharacterController.Move.html
/// There are [*change*]'s marked below
///
/// Issues
///   - Jump doesn't always trigger (Input.GetButtonDown("Jump"))
///   - Get stuck jumping when touching a wall 
/// </summary>
public class PlayerMovementUnity : MonoBehaviour
{
    private CharacterController controller;
    private Vector3 playerVelocity;
    private bool groundedPlayer;
    
    [SerializeField]
    private float playerSpeed = 2.0f;
    [SerializeField]
    private float jumpHeight = 1.0f;
    [SerializeField]
    private float gravityValue = -9.81f;
    
    // Start is called before the first frame update
    void Start()
    {
        this.controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        groundedPlayer = controller.isGrounded;
        // set velocity to zero if we are on the ground (it will go negative as we fall)
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }

        var x = Input.GetAxis("Horizontal");
        var z = Input.GetAxis("Vertical");

        // [*change*] original did this
        //   Vector3 move = new Vector3(x, 0, z); 
        // it would be world space
        // rather this takes into account the direction the player is facing
        var move = this.transform.right * x + this.transform.forward * z;
        
        
        controller.Move(move * Time.deltaTime * playerSpeed);

        if (move != Vector3.zero)
        {
            gameObject.transform.forward = move;
        }

        // Changes the height position of the player..
        if (Input.GetButtonDown("Jump") && groundedPlayer)
        {
            // [*change*] original had -3.0f, formula shown on Brackeys had -2.0f
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -2.0f * gravityValue);
        }

        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
    }
}
