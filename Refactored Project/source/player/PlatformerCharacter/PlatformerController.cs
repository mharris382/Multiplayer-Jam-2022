using Godot;

public class PlatformerController : KinematicBody2D
{
    public Vector2 acc;
    public int accelerationTime = 10;

    
    [Export] public bool canHoldJump = true;

    /// <summary>
    ///  You can still jump this many seconds after falling off a ledge
    /// </summary>
    [Export] public float coyoteTime = 0.1f;

    public Timer coyoteTimer = new Timer();


    // These will be calcualted automatically
    public float defaultGravity;

    /// <summary>
    ///   The height of your jump in the air
    /// </summary>
    [Export] public int doubleJumpHeight = 100; // {set{SetDoubleJumpHeight(value);}}

    public float doubleJumpVelocity;

    /// <summary>
    ///  Multiplies the gravity by this while falling
    /// </summary>
    [Export] public float fallingGravityMultiplier = 1.5f;
    
    public int friction
    {
        get => MoveInput.x != 0 || !IsGrounded ? movingFriction : idleFriction;
    }
    [Export]public int movingFriction = 0;
    [Export]public int idleFriction = 25;
    public bool holdingJump;

    /// <summary>
    /// the name of the jump Action  (in the Input Map) disregarding the "_p1" or "_p2"
    /// </summary>
    [Export] public string inputJump = "jump";
     /// <summary>
    /// the name of the move left Action (in the Input Map) disregarding the "_p1" or "_p2"
    /// </summary>
    [Export] public string inputLeft = "move_left";

     /// <summary>
     /// the name of the move right Action (in the Input Map) disregarding the "_p1" or "_p2"
     /// </summary>
    [Export] public string inputRight = "move_right";

     /// <summary>
     /// the name of the move down Action (in the Input Map) disregarding the "_p1" or "_p2"
     /// </summary>
     [Export] public string inputDown = "move_down";

     /// <summary>
    /// Only neccessary when canHoldJump is off
    /// Pressing jump this many seconds before hitting the ground will still make you jump
    /// </summary>
    [Export] 
     public float jumpBuffer = 0.1f;
     [Export] 
     public int maxSpeed = 128;
    public Timer jumpBufferTimer = new Timer();

    /// <summary>
    /// How long it takes to get to the peak of the jump in seconds
    /// </summary>
    //[Export(PropertyHint.ExpRange, "0.01,1,0.01")]
    [Export()]
    public float jumpDuration = 0.3f; // {set{SetJumpDuration(value);}}


    public int jumpsLeft;
    public float jumpVelocity;

    [Export()]
    //[Export(PropertyHint.ExpRange, "100,4000,0.01,or_greater")] 
    public int maxAcceleration = 4000;

    /// <summary>
    /// Set to 2 for double jump
    /// </summary>
    // [Export(PropertyHint.Range, "1, 2")]
    [Export()]
    public int maxJumpAmount = 1;
    
    
    /// <summary>
    /// The max jump height in Pixels (holding jump)
    /// </summary>
    [Export()]//PropertyHint.Range, "128,2048,64")] 
    public int maxJumpHeight = 150; 


    /// <summary>
    /// The minimum jump Height (tapping jump)
    /// </summary>
    [Export(PropertyHint.Range, "1,128")] 
    public int minJumpHeight = 128; 

    [Export(PropertyHint.Range, "1,2")] 
    public int playerNumber = 1;
    
    
    
    /// <summary>
    /// Multiplies the gravity by this when we release jump
    /// </summary>
    public float releaseGravityMultiplier;
    public Vector2 vel;
    public Vector2 input;
    private string _inputRight  = null;
    private string _inputLeft = null;
    private string _inputDown = null;
    private string _inputJump = null;

    bool _isGrounded;
    public bool IsGrounded
    {
        get => _isGrounded;
        private set => _isGrounded = value;
    }
    public Vector2 Velocity
    {
        get => vel;
        private set => vel = value;
    }
    public Vector2 MoveInput
    {
        get => input;
        private set => input = value;
    }
    
    public PlatformerController() : base()
    {
        _Init();
    }

    public int MaxAcceleration
    {
        get => maxAcceleration;
        set => maxAcceleration = value;
    }

    /// <summary>
    /// Set to 2 for double jump
    /// </summary>
    public int MaxJumpAmount
    {
        get => maxJumpAmount;
        set => maxJumpAmount = value;
    }

    /// <summary>
    /// The max jump height in Pixels (holding jump)
    /// </summary>
    public int MaxJumpHeight
    {
        get => maxJumpHeight;
        set => SetMaxJumpHeight(value);
    }

    /// <summary>
    /// The min jump height in Pixels (holding jump)
    /// </summary>
    public int MinJumpHeight
    {
        get => minJumpHeight;
        set => SetMinJumpHeight(value);
    }

    /// <summary>
    /// How long it takes to get to the peak of the jump in seconds
    /// </summary>
    public float JumpDuration
    {
        get => jumpDuration;
        set => SetJumpDuration(value);
    }

    public int DoubleJumpHeight
    {
        get => doubleJumpHeight;
        set => SetDoubleJumpHeight(value);
    }

    // !used
    public string InputDown => _inputDown ?? (_inputDown = $"{inputDown}_p{playerNumber}");
    private string InputRight => _inputRight ?? (_inputRight = $"{inputRight}_p{playerNumber}");
    private string InputLeft => _inputLeft ?? (_inputLeft = $"{inputLeft}_p{playerNumber}");
    private string InputJump =>_inputJump ?? (_inputJump = $"{inputJump}_p{playerNumber}");

    
    
    public void _Init()
    {
        defaultGravity = CalculateGravity(maxJumpHeight, jumpDuration);
        jumpVelocity = CalculateJumpVelocity(maxJumpHeight, jumpDuration);
        doubleJumpVelocity = CalculateJumpVelocity2(doubleJumpHeight, defaultGravity);
        releaseGravityMultiplier = CalculateReleaseGravityMultiplier(
            jumpVelocity, minJumpHeight, defaultGravity);
    }

    public override void _Ready()
    {
        _Init();
        AddChild(coyoteTimer);
        coyoteTimer.WaitTime = coyoteTime;
        coyoteTimer.OneShot = true;

        AddChild(jumpBufferTimer);
        jumpBufferTimer.WaitTime = jumpBuffer;
        jumpBufferTimer.OneShot = true;
    }

    public override void _PhysicsProcess(float delta)
    {
        acc.x = 0;

        IsGrounded = IsOnFloor();
        if (IsGrounded) coyoteTimer.Start();
        if (!coyoteTimer.IsStopped()) jumpsLeft = maxJumpAmount;
        
        if (Input.IsActionPressed(InputLeft))
        {
            input.x = -1;
            acc.x = -maxAcceleration;
        }
        else if (Input.IsActionPressed(InputRight)) 
        {
            input.x = 1;
            acc.x = maxAcceleration;
        }
        else //no horizontal move input
        {
            input.x = 0;
        }

        if (Input.IsActionPressed(InputDown))
        {
            input.y = -1;
        }
        else
        {
            input.y = 0;
        }
        
        // Check for ground jumps when we can hold jump
        if (canHoldJump)
            // Dont use double jump when holding down
            if (Input.IsActionPressed(InputJump))
                if (IsOnFloor())
                    Jump();

        // Check for ground jumps when we cannot hold jump
        if (!canHoldJump)
            if (!jumpBufferTimer.IsStopped() && IsOnFloor())
                Jump();

        // Check for jumps in the air
        if (Input.IsActionJustPressed(InputJump))
        {
            if (Input.IsActionPressed(InputDown))
            {
                var pos = Position;
                Position = new Vector2(pos.x, pos.y + 1);
            }
            else
            {
                holdingJump = true;
                jumpBufferTimer.Start();

                // Only jump in the air when press the button down, code above already jumps when we are grounded
            }

            if (!IsOnFloor()) Jump();
        }

        if (Input.IsActionJustReleased(InputJump)) holdingJump = false;
        var gravity = defaultGravity;

        if (vel.y > 0) // If we are falling
            gravity *= fallingGravityMultiplier;
        if (!holdingJump && vel.y < 0) // if we released jump && are still rising
            if (!(jumpsLeft < maxJumpAmount - 1)) // Always jump to max height when we are using a double jump
                gravity *= releaseGravityMultiplier; // multiply the gravity so we have a lower jump

        acc.y = -gravity;
        vel.x *= 1 / (1 + delta * friction);

        vel += acc * delta;
        if (Mathf.Abs(vel.x) > maxSpeed)
        {
            vel.x = maxSpeed * Mathf.Sign(vel.x);
        }
        vel = MoveAndSlide(vel, Vector2.Up);
    }

    public float CalculateGravity(float pMaxJumpHeight, float pJumpDuration)
    {
        // Calculates the desired gravity by looking at our jump height && jump duration
        // Formula is from this video https://www.youtube.com/watch?v=hG9SzQxaCm8;
        return -2f * pMaxJumpHeight / Mathf.Pow(pJumpDuration, 2f);
    }

    public float CalculateJumpVelocity(float pMaxJumpHeight, float pJumpDuration)
    {
        // Calculates the desired jump velocity by lookihg at our jump height && jump duration
        return 2 * pMaxJumpHeight / pJumpDuration;
    }

    public float CalculateJumpVelocity2(float pMaxJumpHeight, float pGravity)
    {
        // Calculates jump velocity from jump height && gravity
        // formula from 
        // https://sciencing.com/acceleration-velocity-distance-7779124.html#:~:text=in%20every%20step.-,Starting%20from%3A,-v%5E2%3Du
        return Mathf.Sqrt(-2f * pGravity * pMaxJumpHeight);
    }

    public float CalculateReleaseGravityMultiplier(float pJumpVelocity, float pMinJumpHeight, float pGravity)
    {
        // Calculates the gravity when the key is released based on the minimum jump height && jump velocity
        // Formula is from this website https://sciencing.com/acceleration-velocity-distance-7779124.html
        var releaseGravity = 0f - Mathf.Pow(pJumpVelocity, 2f) / (2f * pMinJumpHeight);
        return releaseGravity / pGravity;
    }

    public float CalculateFriction(float timeToMax)
    {
        // Formula from https://www.reddit.com/r/gamedev/comments/bdbery/comment/ekxw9g4/?utm_source=share&utm_medium=web2x&context=3;
        // this friction will hit 90% of max speed after the accel time
        return 1f - 2.30259f / timeToMax;
    }

    public float CalculateSpeed(float pMaxSpeed, float pFriction)
    {
        // Formula from https://www.reddit.com/r/gamedev/comments/bdbery/comment/ekxw9g4/?utm_source=share&utm_medium=web2x&context=3	;
        return pMaxSpeed / pFriction - pMaxSpeed;
    }

    public void Jump()
    {
        if (jumpsLeft == maxJumpAmount && coyoteTimer.IsStopped())
            // Your first jump must be used when on the ground
            // If you fall off the ground && then jump you will be using you second jump
            jumpsLeft -= 1;
        if (jumpsLeft > 0)
        {
            if (jumpsLeft < maxJumpAmount) // If we are double jumping
                vel.y = -doubleJumpVelocity;
            else
                vel.y = -jumpVelocity;
            jumpsLeft -= 1;
        }

        coyoteTimer.Stop();
    }

    public void SetMaxJumpHeight(int value)
    {
        maxJumpHeight = value;

        defaultGravity = CalculateGravity(maxJumpHeight, jumpDuration);
        jumpVelocity = CalculateJumpVelocity(maxJumpHeight, jumpDuration);
        doubleJumpVelocity = CalculateJumpVelocity2(doubleJumpHeight, defaultGravity);
        releaseGravityMultiplier = CalculateReleaseGravityMultiplier(
            jumpVelocity, minJumpHeight, defaultGravity);
    }

    public void SetJumpDuration(float value)
    {
        jumpDuration = value;

        defaultGravity = CalculateGravity(maxJumpHeight, jumpDuration);
        jumpVelocity = CalculateJumpVelocity(maxJumpHeight, jumpDuration);
        doubleJumpVelocity = CalculateJumpVelocity2(doubleJumpHeight, defaultGravity);
        releaseGravityMultiplier = CalculateReleaseGravityMultiplier(
            jumpVelocity, minJumpHeight, defaultGravity);
    }

    public void SetMinJumpHeight(int value)
    {
        minJumpHeight = value;
        releaseGravityMultiplier = CalculateReleaseGravityMultiplier(
            jumpVelocity, minJumpHeight, defaultGravity);
    }

    public void SetDoubleJumpHeight(int value)
    {
        doubleJumpHeight = value;
        doubleJumpVelocity = CalculateJumpVelocity2(doubleJumpHeight, defaultGravity);
    }
}