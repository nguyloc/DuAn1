using UnityEngine;

namespace Player.StateMachine
{
    public class Player : MonoBehaviour
    {
        #region State Variables
        public PlayerStateMachine StateMachine { get; private set; }

        public PlayerIdleState IdleState { get; private set; }
        public PlayerMoveState MoveState { get; private set; }
        public PlayerJumpState JumpState { get; private set; }
        public PlayerInAirState InAirState { get; private set; }
        public PlayerLandState LandState { get; private set; }
        public PlayerWallSlideState WallSlideState { get; private set; }
        public PlayerWallGrabState WallGrabState { get; private set; }
        public PlayerWallClimbState WallClimbState { get; private set; }

        public PlayerWallJumpState WallJumpState { get; private set; }
        public PlayerLegdeClimbState LegdeClimbState { get; private set; }
        public PlayerDashState DashState { get; private set; }

        #endregion

        #region Components
        public Animator Anim {  get; private set; }
        public PlayerInputHandler InputHandler { get; private set; }
        public Rigidbody2D Rb { get; private set; }
        public Transform DashDirectionIndicator { get; private set; }

        #endregion

        #region Check Transforms

        [SerializeField]
        private Transform groundCheck;

        [SerializeField]
        private Transform wallCheck;

        [SerializeField]
        private Transform ledgeCheck;

        #endregion

        #region Other Variables
        public Vector2 CurentVelocity { get; private set; }
        public int FacingDirection { get; private set; }

        [SerializeField]
        private PlayerData playerData;

        private Vector2 workspace;

        #endregion

        #region Unity Callback Functions
        private void Awake()
        {
            StateMachine = new PlayerStateMachine();

            IdleState = new PlayerIdleState(this, StateMachine, playerData, "idle");
            MoveState = new PlayerMoveState(this, StateMachine, playerData, "move");
            JumpState = new PlayerJumpState(this, StateMachine, playerData, "inAir");
            InAirState = new PlayerInAirState(this, StateMachine, playerData, "inAir");
            LandState = new PlayerLandState(this, StateMachine, playerData, "land");
            WallSlideState = new PlayerWallSlideState(this, StateMachine, playerData, "wallSlide");
            WallGrabState = new PlayerWallGrabState(this, StateMachine, playerData, "wallGrab");
            WallClimbState = new PlayerWallClimbState(this, StateMachine, playerData, "wallClimb");
            WallJumpState = new PlayerWallJumpState(this, StateMachine, playerData,"inAir" );
            LegdeClimbState = new PlayerLegdeClimbState(this, StateMachine, playerData, "ledgeClimbState");
            DashState = new PlayerDashState(this, StateMachine, playerData, "inAir");
        }

        private void Start()
        {
            Anim = GetComponent<Animator>();
            InputHandler = GetComponent<PlayerInputHandler>();
            Rb = GetComponent<Rigidbody2D>();
            DashDirectionIndicator = transform.Find("DashDirectionIndicator");

            FacingDirection = 1;

            StateMachine.Initialize(IdleState);
        }

        private void Update()
        {
            CurentVelocity = Rb.velocity;
            StateMachine.CurrentState.LogicUpdate();
        }

        private void FixedUpdate()
        {
            StateMachine.CurrentState.PhysicsUpdate();
        }
        #endregion

        #region Set Functions

        public void SetVelocityZero()
        {
            Rb.velocity = Vector2.zero;
            CurentVelocity = Vector2.zero ;
        }

        public void SetVelocity(float velocity, Vector2 angle, int direction)
        {
            angle.Normalize();
            workspace.Set(angle.x * velocity * direction, angle.y * velocity );
            Rb.velocity = workspace;
            CurentVelocity = workspace;
        }

        public void SetVelocity(float velocity, Vector2 direction)
        {
            workspace = direction * velocity;
            Rb.velocity = workspace;
            CurentVelocity = workspace;
        }

        public void SetVelocityX(float velocity)
        {
            workspace.Set(velocity, CurentVelocity.y);
            Rb.velocity = workspace;
            CurentVelocity = workspace;
        }

        public void SetVelocityY(float velocity)
        {
            workspace.Set(CurentVelocity.x, velocity);
            Rb.velocity = workspace;
            CurentVelocity = workspace;
        }

        #endregion

        #region Check Functions

        public bool CheckIfGrounded()
        {
            return Physics2D.OverlapCircle(groundCheck.position,playerData.groundCheckRadius,playerData.whatIsGround);
        }

        public bool CheckIfTouchingWall()
        {
            return Physics2D.Raycast(wallCheck.position,Vector2.right * FacingDirection, playerData.wallCheckDistance,playerData.whatIsGround);
        }

        public bool CheckIfTouchingLedge()
        {
            return Physics2D.Raycast(ledgeCheck.position, Vector2.right * FacingDirection, playerData.wallCheckDistance, playerData.whatIsGround);
        }

        public bool CheckIfTouchingWallBack()
        {
            return Physics2D.Raycast(wallCheck.position, Vector2.right * - FacingDirection, playerData.wallCheckDistance, playerData.whatIsGround);
        }

        public void CheckIfShouldFlip(int xInput)
        {
            if (xInput != 0 && xInput != FacingDirection)
            {
                Flip();
            }
        }
        #endregion

        #region Other Functions

        public Vector2 DetermineCornerPosition()
        {
            var position1 = wallCheck.position;
            RaycastHit2D xHit = Physics2D.Raycast(position1, Vector2.right * FacingDirection, playerData.wallCheckDistance, playerData.whatIsGround);
            float xDist = xHit.distance;
            workspace.Set(xDist * FacingDirection, 0f);

            var position = ledgeCheck.position;
            RaycastHit2D yHit = Physics2D.Raycast(position + (Vector3)(workspace), Vector2.down, position.y - position1.y, playerData.whatIsGround) ;
            float yDist = yHit.distance;
            workspace.Set(position1.x + (xDist * FacingDirection), position.y - yDist);

            return workspace;
        }

        private void AnimationTrigger() => StateMachine.CurrentState.AnimationTrigger();

        private void AnimationFinishTrigger() => StateMachine.CurrentState.AnimationFinishTrigger();
        private void Flip()
        {
            FacingDirection *= -1;
            transform.Rotate(0.0f, 180.0f, 0.0f);
        }
        #endregion

    }
}
