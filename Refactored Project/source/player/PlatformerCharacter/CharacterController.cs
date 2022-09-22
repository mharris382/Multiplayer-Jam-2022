using Game.blocks.gas;
using Game.core;
using Game.player.Animation;
using Godot;

namespace Game.player.PlatformerCharacter
{
    public class CharacterController : PlatformerController
    {
        [Export()] public NodePath animatorPath = "Animator";

        private CharacterAnimator _animator;
        [Export()] public int suckRate = 16;
        [Export()] public Vector2 suckSize = new Vector2(2, 2);
        [Export()] public int dumpRate = 2;
        [Export()] public Vector2 dumpSize = new Vector2(2, 2);
        [Export()] public Vector2 pullSize = new Vector2(15, 15);
        private CharacterSink sink;
        private CharacterSource source;
        
        public override async void _Ready()
        {
            sink = new CharacterSink(this, 6);
            source = new CharacterSource(this, 6);
            
            _animator = GetNode<CharacterAnimator>(animatorPath);
            await GasStuff.WaitForAssignments();
            this.AssertNotNull(_animator);
            base._Ready();
        }


        public override void _Input(InputEvent @event)
        {
            var pressedSink = Input.IsActionPressed($"sink_p{playerNumber}");
            var pressedSource = Input.IsActionPressed($"source_p{playerNumber}");
            if (pressedSink)
            {
                Debug.Log("Sink Pressed");
                SetSinkActive(true);
                SetSourceActive(false);
            }
            else if (pressedSource)
            {
                SetSinkActive(false);
                SetSourceActive(true);
            }
            else
            {
                SetSinkActive(false);
                SetSourceActive(false);
            }
            // SetSinkActive();
            //
            // SetSourceActive(;

            base._Input(@event);
        }

        private void SetSinkActive(bool isActionPressed)
        {
            
            if (isActionPressed)
            {
                GasStuff.AddSink(sink);
                if (_sinkSourceState != SinkSourceState.Sink)
                {
                    _sinkSourceState = SinkSourceState.Sink;
                } 
               
            }
            else
            {
                GasStuff.RemoveSink(sink);
                if (_sinkSourceState == SinkSourceState.Sink)
                {
                    // GasStuff.AddSink(sink);
                    
                    _sinkSourceState = SinkSourceState.None;
                } 
            }

        }

        private void SetSourceActive(bool isActionPressed)
        {
            if (isActionPressed)
            {
                GasStuff.AddSource(source);
                if (_sinkSourceState != SinkSourceState.Source)
                {
                    _sinkSourceState = SinkSourceState.Source;
                } 
               
            }
            else
            {
                GasStuff.RemoveSource(source);
                if (_sinkSourceState == SinkSourceState.Source)
                {
                    _sinkSourceState = SinkSourceState.None;
                } 
            }
        }

        private SinkSourceState _sinkSourceState = SinkSourceState.None;
        public override void _Process(float delta)
        {
            base._Process(delta);
            _animator.UpdateAnimState(MoveInput, Velocity, IsGrounded);
        }

        enum SinkSourceState
        {
            None,
            Sink,
            Source
        }


        private class CharacterSink : ISteamSink
        {
            private int _demand;
            public CharacterSink(CharacterController controller, int demand)
            {
                _demand = demand;
                this.controller = controller;
            }

            public int Demand
            {
                get => controller.suckRate;
                set { }
            }

            public Vector2 size => controller.suckSize;
            public Vector2 pullSize => size + controller.pullSize;
            public Vector2 Position
            {
                get => GetWorldSpacePosition();
                set { }
            }

            public Vector2 GetWorldSpacePosition()
            {
                return controller.GlobalPosition;
            }

            public GridDirections GetPullDirections()
            {
                return GridDirections.ALL;
            }
            private CharacterController controller;
        }

        private class CharacterSource : ISteamSource
        {
            private int _output;

            public int Output
            {
                get => controller.dumpRate;
                set { }
            }

            public bool Enabled
            {
                get => true;
                set { }
            }

            public Vector2 Position
            {
                get => GetWorldSpacePosition();
                set { }
            }

            public CharacterSource(CharacterController controller, int output)
            {
                this.controller = controller;
                this._output = output;
            }

            private CharacterController controller;

            public Vector2 size => controller.dumpSize;

            public void BroadcastSourceStateChanged()
            {
                
            }

            public Vector2 GetWorldSpacePosition()
            {
                return controller.GlobalPosition;
            }
        }
    }
}