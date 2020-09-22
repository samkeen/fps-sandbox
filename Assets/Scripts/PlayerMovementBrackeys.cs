using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This is from Brakeys: https://www.youtube.com/watch?v=_QajrabyTJc
/// There are [*change*]'s marked below
///
/// Issues
///   - Falls far too quickly
///   - Jump rockets you up (you do come back down)
/// </summary>
public class PlayerMovementBrackeys : MonoBehaviour
{
    private CharacterController _controller;
    [SerializeField] private float speed = 10f;
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private float jumpHeight = 3.0f;

    /// <summary>
    /// We introduce a var to track velocity so we can apply gravity to it
    /// </summary>
    private Vector3 _velocity;
    private bool groundedPlayer;
        
    
    void Start()
    {
        this._controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        groundedPlayer = this._controller.isGrounded;
        if (groundedPlayer && this._velocity.y < 0)
        {
            this._velocity.y = -2f;
        }
        
        var x = Input.GetAxis("Horizontal");
        var z = Input.GetAxis("Vertical");

        // if we did this: Vector3 move = new Vector3(x, 0, z); it would be world space
        // rather this takes into account the direction the player is facing
        var move = this.transform.right * x + this.transform.forward * z;

        this._controller.Move(move * this.speed * Time.deltaTime);

        if (Input.GetButtonDown("Jump") && this.groundedPlayer)
        {
            //
            //  v = sqrt(h * -2 * g)
            //
            this._velocity.y = Mathf.Sqrt(this.jumpHeight * -2f * this.gravity);
        }
        //              1
        //   (delta)y = - g * t^2
        //              2
        // It calculates the motion of an object falling from a higher point to a lower point through
        // a certain distance(or height) h within a certain period of time t, plus the influence of
        // gravity g exerted from the earth.
        //
        // thus here we multiply by Time again to get Time^2
        this._velocity.y += gravity * Time.deltaTime;

        this._controller.Move(this._velocity);
        
        
        
    }
}
