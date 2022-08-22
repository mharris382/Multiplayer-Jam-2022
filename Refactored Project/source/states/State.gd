extends Node

# warning-ignore:unused_signal
signal finished(next_state_name)

#basically this enum is saying when this state is on the state state, should the next state in the stack still be allowed to run or should this state block all lower states
#BLOCK will prevent states lower in the stack from running
#COMBINE will allow the next state in the stack to run
#COMPLETED will allow the next state in the stack to run AND remove this state from the stack
#NOTE: each function that returns an int, is specified separately and Block is the default if not specified by the child class
enum StateStackBehaviour { BLOCK = 0, COMBINE = 1, COMPLETED = 1  }

var player = null

var move_input : Vector2
var current_aim_input : Vector2 
var last_aim_direction : Vector2
var input_jump : int #this value is StateMachine.ButtonInputState

func get_action(action_name):
	return player.get_player_specific_action(action_name)


# Initialize the state. E.g. change the animation.
func enter():
	pass


# Clean up the state. Reinitialize values like a timer.
func exit():
	pass


func handle_input(_event) -> int:
	
	return StateStackBehaviour.BLOCK

func update_physics(delta) -> int:
	
	return StateStackBehaviour.BLOCK


func update(_delta) -> int:
	
	return StateStackBehaviour.BLOCK


func _on_animation_finished(_anim_name):
	pass

func override_interact_input(interact_button_state):
	return interact_button_state

func override_jump_input(jump_button_state):
	return jump_button_state
	
func  override_ability_input(ability_button_state):
	return ability_button_state
