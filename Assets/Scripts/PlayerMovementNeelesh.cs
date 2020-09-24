using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Example from https://neelesh.io/simple-character-controller-unity-3d/
/// There are [*change*]'s marked below
///
/// Transform holds
///  - Position: vector
///  - rotation: A Quaternion that stores the rotation of the Transform in world space.
///  - scale: vector
/// 
///   we want to add to that a velocity vector to get our new position vector.
/// 
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
        _GetUserInput();
        _ProcessYMovement();
        _ProcessRotation();
        _MoveCharacter();
    }

    private void _MoveCharacter()
    {
        // CharacterController.Move(): moves the GameObject in the given direction
        // see https://docs.unity3d.com/ScriptReference/CharacterController.Move.html
        //
        this.controller.Move(this.direction * Time.deltaTime);
    }

    private void _ProcessRotation()
    {
        // you can still move but that will be in world space
        // here we convert this.direction to local space using transform
        //
        // [*change*] added this line as done in Brakeys to have player move in
        // direction of camera
        // also added this.transform.up * direction.y (brackey ex had y accounted for in
        // another controller.move() call )
        // This is (vector * scaler) + (vector * scaler) + (vector * scaler)
        // which is: (vector) + (vector) + (vector)
        // ex at rest: (0,0,0) + (0,-1,0) + (0,0.0)
        this.direction =
            // transform.right|up|forward are local space vs Vector3.right|up|forward which are world space
            // since the camera in a child transform of player (this) the camera rotates in the direction of the
            // player's movement.
            // unit vector (1,0,0)
            this.transform.right * this.direction.x
            // unit vector (0,1,0)
            + this.transform.up * this.direction.y
            // unit vector (0,0,1)
            + this.transform.forward * this.direction.z;
        // this._LogEvery100Frames($"this.transform.right: {this.transform.right}");
        // this._LogEvery100Frames($"this.transform.up: {this.transform.up}");
        // this._LogEvery100Frames($"this.transform.forward: {this.transform.forward}");
        
    }

    private void _ProcessYMovement()
    {
        // Y movement
        if (controller.isGrounded)
        {
            // If we're grounded, and we press Space, we apply jumpForce to our direction.y
            if (Input.GetButtonDown("Jump"))
            {
                this.direction.y = jumpForce;
                Debug.Log($"Direction vector after process Jump: {this.direction}");
            }
            else
            {
                // give a small negative value to direction.y when the we're on the ground, so we are always touching
                // what we're standing on.
                this.direction.y = -1;
                this._LogEvery100Frames($"Direction vector after no jump and on ground: {this.direction}");
            }
        }
        else
        {
            // we are not on the ground, we wish to increase this gravitational force.
            this.direction.y = this.direction.y + (Physics.gravity.y * gravityScale * Time.deltaTime);
            this._LogEvery100Frames($"Direction vector after process in air: {this.direction}");
        }
    }

    private void _GetUserInput()
    {
        this.direction = new Vector3(
            Input.GetAxis("Horizontal") * speed,
            this.direction.y,
            Input.GetAxis("Vertical") * speed
        );
        this._LogEvery100Frames($"Direction vector after GetAxis: {this.direction}");
    }

    private void _LogEvery100Frames(string message)
    {
        if (Time.frameCount % 100 == 0)
        {
            Debug.Log(message);
        }
    }
}