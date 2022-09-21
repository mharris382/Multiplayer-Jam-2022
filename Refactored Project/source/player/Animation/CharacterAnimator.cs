using Game.core;
using Godot;

namespace Game.player.Animation
{
    public class CharacterAnimator : AnimatedSprite
    {
        private const string IDLE_ANIM_NAME = "idle";
        private const string JUMP_ANIM_NAME = "jump-fall";
        private const string SPECIAL_ANIM_NAME = "idle";
        private const string FALL_ANIM_NAME = "jump-fall";
        private const string LAND_ANIM_NAME = "idle";
        
        private const string RUN_ANIM_NAME = "run";

        [Export(PropertyHint.Enum, "Left:-1,Right:1")]
        public int defaultDirection = 1;

        /// <summary>
        /// allows tweaking of the animation transition between falling and jumping
        /// </summary>
        [Export(PropertyHint.Range, "-0.5, 0.1")]
        public float fallThreshold = 0;

        private bool _facingLeft;
        private string _currentAnim = IDLE_ANIM_NAME;
        private bool _wasJumping = false;
        
        private bool FacingLeft
        {
            get => _facingLeft;
            set
            {
                if (_facingLeft != value)
                {
                    _facingLeft = value;
                    FlipH = _facingLeft;
                }
            }
        }
        public void UpdateAnimState(Vector2 moveInput, Vector2 velocity, bool isGrounded)
        {
           // Debug.Log($"moveInput={moveInput}, velocity={velocity}, isGrounded={isGrounded}");
            SetFacingDirection(moveInput.x);
            
            if (isGrounded)
            {
                _wasJumping = false;
                TrySwitchAnimation(moveInput.x != 0 ? RUN_ANIM_NAME : IDLE_ANIM_NAME);
            }
            else
            {
                if (velocity.y > 0)
                {
                    _wasJumping = true;
                    TrySwitchAnimation(JUMP_ANIM_NAME);
                }
                if ( velocity.y < 0)// || (_wasJumping && velocity.y < fallThreshold))
                {
                    _wasJumping = false;
                    TrySwitchAnimation(FALL_ANIM_NAME);
                }
            }
        }

        private void SetFacingDirection(float moveInputX)
        {
            if (moveInputX != 0)
            {
                FacingLeft = moveInputX < 0;
            }
        }

        private void TrySwitchAnimation(string anim)
        {
            if (_currentAnim != anim)
            {
                _currentAnim = anim;
                Stop();
                Play(anim);
            }
        }
    }
}