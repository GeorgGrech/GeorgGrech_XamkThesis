using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;

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

    [SerializeField] Slider dashSlider;
    
    enum ButtonPressed
    {
        Forward, Back, Left, Right, Null //Including "Null" value if nothing has been pressed in a particular frame
    }

    [SerializeField] ButtonPressed lastPressed;

    // Start is called before the first frame update
    void Start()
    {
        dashTimer = dashCooldown; //Make dash instantly available
    }

    // Update is called once per frame
    void Update()
    {
        dashTimer += Time.deltaTime;
        doubleTapTimer += Time.deltaTime;
        UpdateDashSlider();

        UserInput();
    }

    private void Dash(ButtonPressed direction)
    {
        dashTimer = 0; //Reset timer
        Debug.Log("Dashing");

        playerRB.velocity = Vector3.zero; //Stop player movement before assigning new force

        Vector3 forceDirection = Vector3.zero;

        switch (direction)
        {
            case ButtonPressed.Forward:
                forceDirection = Vector3.forward;
                break;
            case ButtonPressed.Back:
                forceDirection = Vector3.back;
                break;
            case ButtonPressed.Left:
                forceDirection = Vector3.left;
                break;
            case ButtonPressed.Right:
                forceDirection = Vector3.right;
                break;
        }

        playerRB.AddForce(playerRotation.rotation * forceDirection * dashForce);
        
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

    void UpdateDashSlider()
    {
        if (dashTimer >= dashCooldown)
        {
            dashSlider.gameObject.SetActive(false);
        }
        else
        {
            dashSlider.value = (1-(dashTimer/dashCooldown));
            dashSlider.gameObject.SetActive(true);
        }
    }
}
