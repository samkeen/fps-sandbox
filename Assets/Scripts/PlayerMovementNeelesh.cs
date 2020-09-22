using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Example from https://neelesh.io/simple-character-controller-unity-3d/
/// There are [*change*]'s marked below
/// </summary>
public class PlayerMovementNeelesh : MonoBehaviour
{
    private CharacterController controller;
    [SerializeField] private float speed;
    [SerializeField] private float gravityScale;
    [SerializeField] private float jumpForce;
    
    private Vector3 direction;
    
    // Start is called before the first frame update
    void Start()
    {
        this.controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        // X and Z movement (WASD)
        direction = new Vector3(
            Input.GetAxis("Horizontal") * speed, 
            direction.y,
            Input.GetAxis("Vertical") * speed
        );
        
        // Y movement
        if (controller.isGrounded)
        {
            // If we're grounded, and we press Space, we apply jumpForce to our direction.y
            if (Input.GetButtonDown("Jump"))
            {
                direction.y = jumpForce;
            }
            else
            {
                // give a small negative value to direction.y when the we're on the ground, so we are always touching
                // what we're standing on.
                direction.y = -1;
            }
        }
        else
        {
            // we are not on the ground, we wish to increase this gravitational force.
            direction.y = direction.y + (Physics.gravity.y * gravityScale * Time.deltaTime);
        }
        // [*change*] added this line as done in Brakeys to have player move in
        // direction of camera
        // also added this.transform.up * direction.y (brackey ex had y accounted for in
        // another controller.move() call )
        direction = 
            this.transform.right * direction.x + this.transform.up * direction.y + this.transform.forward * direction.z;
        // var move = this.transform.right * x + this.transform.forward * z;
        this.controller.Move(direction * Time.deltaTime);
    }
}
