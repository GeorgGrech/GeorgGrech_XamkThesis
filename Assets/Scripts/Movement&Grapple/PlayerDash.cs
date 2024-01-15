using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class PlayerDash : MonoBehaviour
{
    [SerializeField] PlayerGrapple playerGrapple; //Link to player's grapple ability. Used to check that player is not grappling when dash is attempted.

    [SerializeField] private float dashCooldown; //Minimum value between dashes
    private float dashTimer; //Constantly updated time to check if dash is available

    [SerializeField] private float doubleTapInterval;
    private float doubleTapTimer;

    [SerializeField] private float dashForce;
    [SerializeField] Rigidbody playerRB;
    [SerializeField] Transform playerRotation;
    
    enum ButtonPressed
    {
        Forward, Back, Left, Right, Null //Including "Null" value if nothing has been pressed in a particular frame
    }

    [SerializeField] ButtonPressed lastPressed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        dashTimer += Time.deltaTime;
        doubleTapTimer += Time.deltaTime;

        UserInput();
    }

    private void Dash(ButtonPressed direction)
    {
        dashTimer = 0; //Reset timer
        Debug.Log("Dashing");

        playerRB.AddForce(new Vector3(0,0, dashForce));
        
    }

    void UserInput()
    {

        ButtonPressed nowPressed = ButtonPressed.Null;
        if (Input.GetButtonDown("Vertical") && Input.GetAxisRaw("Vertical") > 0)
        {
            // forward
            nowPressed = ButtonPressed.Forward;
        }
        if (Input.GetButtonDown("Vertical") && Input.GetAxisRaw("Vertical") < 0)
        {
            // back
            nowPressed = ButtonPressed.Back;
        }
        if (Input.GetButtonDown("Horizontal") && Input.GetAxisRaw("Horizontal") > 0)
        {
            // right
            nowPressed = ButtonPressed.Right;
        }
        if (Input.GetButtonDown("Horizontal") && Input.GetAxisRaw("Horizontal") < 0)
        {
            // left
            nowPressed = ButtonPressed.Left;
        }

        if(nowPressed == lastPressed)
        {
            if (doubleTapTimer <= doubleTapInterval) //If within dashing time window
            {
                if (dashTimer >= dashCooldown) //If dash has cooled down
                {
                    Dash(nowPressed);
                }
                else
                    Debug.Log("Cooling down - " + (dashCooldown - dashTimer) + "s left");
            }
        }

        if(nowPressed != ButtonPressed.Null)
        {
            //If a key has been pressed
            lastPressed = nowPressed; //Save direction pressed
            doubleTapTimer = 0; //Reset timer
        }
    }
}
