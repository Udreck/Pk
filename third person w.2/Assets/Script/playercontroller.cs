using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playercontroller : MonoBehaviour
{
    
    private CharacterController characterController;
    [SerializeField] private float playerspeed = 5f;
    [SerializeField] private Camera followCamera;
    
    [SerializeField] private float rotationSpeed = 10f;

    private Vector3 playerVelocity;
    [SerializeField] private float jumpHeight = 2.5f;

    public bool groundedPlayer;
    [SerializeField] private float gravityValue = -13f;

    public Animator animator;

    public static playercontroller instance;

    private void Awake()
    {
         instance = this; 
    }
    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (CheckWinner.instance.isWinner)
        {
             case true:
                animator.SetBool("victory", CheckWinner.instance.isWinner);
                  break;
             case false:
                Movement();
                break;
        }
        Movement();
    }
    void Movement()
    {
        groundedPlayer = characterController.isGrounded;

        if (characterController.isGrounded && playerVelocity.y < -2)
        {
            playerVelocity.y = -1f;
        }

        float horzontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 movementInput = Quaternion.Euler(0, followCamera.transform.eulerAngles.y, 0) * new Vector3 (horzontalInput,0,verticalInput);

        Vector3 movementDirection = movementInput.normalized;

        characterController.Move(movementDirection * playerspeed*Time.deltaTime);

        if (movementDirection != Vector3.zero)
        {
            Quaternion desiredRotation = Quaternion.LookRotation(movementDirection,Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, desiredRotation, Time.deltaTime);
        }
        if(Input.GetButtonDown("Jump") && groundedPlayer)
        {
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.6f * gravityValue);
        }

        playerVelocity.y += gravityValue * Time.deltaTime;
        characterController.Move(playerVelocity*Time.deltaTime);

        animator.SetFloat("speed", Mathf.Abs(movementDirection.x) + Mathf.Abs(movementDirection.z));
        animator.SetBool("ground", characterController.isGrounded);

    }
}
