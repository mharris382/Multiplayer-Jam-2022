using Game.core;
using Game.player.Animation;
using Godot;
namespace Game.player.PlatformerCharacter
{
    public class CharacterController : PlatformerController
    {
        [Export()]
        public NodePath animatorPath = "Animator";
        
        private CharacterAnimator _animator;
        public override void _Ready()
        {
            _animator = GetNode<CharacterAnimator>(animatorPath);
            this.AssertNotNull(_animator);
            base._Ready();
        }

        public override void _Process(float delta)
        {
            base._Process(delta);
            _animator.UpdateAnimState(MoveInput, Velocity, IsGrounded);
        }
    }
}