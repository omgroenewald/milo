using UnityEngine;



public class motion : MonoBehaviour
{

    public float sprintmodifier;
    public float speed;
    public float CrouchSpeed;
    public bool CrouchBool;
    public float jumpforce;
    public float forwardjumpforce;
    public float holdjumpforce;
    public Camera normalcam;
    public Transform grounddetector;
    public Transform WallDetector;
    public LayerMask Wall;
    public LayerMask ground;
    


    private Rigidbody rig;
    private float basefov;
    private float sprintfovmodifier = 1.15f;
    float t_hmove;
    float t_vmove;
    float ExtraHmove;
    float ExtraVmove;


    bool sprint;
    bool jump;
    bool holdjump;
    bool doblejump;
    bool power;


    bool InvokePause = false;
    bool isgrounded;
    bool IsWalled;
    bool isjumping;
    bool isholdjumping;
    bool issprinting;
    bool isdoblejumping;
    bool doblejumping;
    bool iswalljumping;
    public bool doubleJumpUsed = false;
    int WallJumpsUsed;
    public bool AllJumpsNotUsed = false;
    bool Infinite = false;
    bool WallInfinite = false;
    


    private void Start()
    {
        //Camera.main.enabled = false;
        rig = GetComponent<Rigidbody>();
        basefov = normalcam.fieldOfView;

    }

    private void Update()
    {
        t_hmove = Input.GetAxis("Vertical");
        t_vmove = Input.GetAxis("Horizontal");
        ExtraVmove = forwardjumpforce;

        power = Input.GetKey(KeyCode.LeftControl);
        sprint = Input.GetKey(KeyCode.LeftShift);
        jump = Input.GetKeyDown(KeyCode.Space);


        //holdjump = Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.Space);
        //doblejump = isjumping && Input.GetKeyDown(KeyCode.Space);

        isgrounded = Physics.Raycast(grounddetector.position, Vector3.down, 0.1f, ground);
        IsWalled = Physics.Raycast(WallDetector.position, Vector3.right, 0.3f, Wall);
        isjumping = jump && isgrounded && !power;
        isholdjumping = jump && isgrounded && power;
        issprinting = sprint && isgrounded;
        if (isgrounded && !jump)
        {
            WallJumpsUsed = 3;
            print("grounded");
        }
        if (jump && !isgrounded && !doubleJumpUsed && !IsWalled)
        {
            isdoblejumping = true;
            doubleJumpUsed = true;

        }
        if (jump && !isgrounded && IsWalled)
        {
            iswalljumping = true;
            WallJumpsUsed =- 1;
            print("walljumpsmet");
        }
        if (Infinite)
        {
            doubleJumpUsed = false;
        }
        if (WallJumpsUsed == 0)
        {
            AllJumpsNotUsed = true;
        }
        if (IsWalled)
        {
            print("iswalled");
        }
  
    }
    public void DoubleJump()
    {
        Infinite = true;
    }
    public void Pause()
    {
        InvokePause = true;
    }
    public void CrouchT()
    {
        CrouchBool = true;
    }
    public void CrouchF()
    {
        CrouchBool = false;
    }
    public void Resume()
    {
        InvokePause = false;
    }
    public void FixedUpdate()
    {
        if (!InvokePause)
        {
            if (isjumping)
            {
                rig.AddForce(Vector3.up * jumpforce);
                print("Jumping");
                doubleJumpUsed = false;
                isjumping = false;

            }

            if (isholdjumping)
            {
                rig.AddForce(Vector3.up * (jumpforce + 100f));
                print("hold jump");
                doubleJumpUsed = false;
                isholdjumping = false;
            }
            if (isdoblejumping)
            {

                rig.AddForce(Vector3.up * jumpforce);

                print("second Jump");
                doubleJumpUsed = true;
                isdoblejumping = false;

            }if (iswalljumping)
            {
                rig.AddForce(Vector3.up * jumpforce);
                

                print("wallJump");
                WallJumpsUsed -= 1;
                iswalljumping = false;
            }



            Vector3 t_direction = new Vector3(t_vmove, 0, t_hmove);
            t_direction.Normalize();

            float t_adjustedspeed = speed;
            if (issprinting) t_adjustedspeed *= sprintmodifier;
            if (CrouchBool) t_adjustedspeed -= CrouchSpeed;


            Vector3 t_targetvelocty = transform.TransformDirection(t_direction * t_adjustedspeed * Time.deltaTime);
            t_targetvelocty.y = rig.velocity.y;

            rig.velocity = t_targetvelocty;

            

            if (issprinting) { normalcam.fieldOfView = Mathf.Lerp(normalcam.fieldOfView, basefov * sprintfovmodifier, Time.deltaTime * 8f); }
            else { normalcam.fieldOfView = basefov; }
        } 
    }
}
