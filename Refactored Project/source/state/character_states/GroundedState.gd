extends State

func enter(_msg := {}) -> void:
	print("Enter: ", name)
	assert(is_owned_by_player())

func physics_update(_delta: float):
	
	var move_input = state_machine.character._move_direction
	var velocity = state_machine.character._velocity
	var speed = state_machine.character.speed
	var direction = Vector2(move_input.x, 0)
	
	velocity = state_machine.character.calculate_move_velocity(velocity,direction,speed,false)
	state_machine.character.apply_velocity(velocity)
	
	var player_num = state_machine.character.player_number
	if Input.is_action_just_pressed("jump_p%d"%player_num):
		if move_input.y > 0:
			var pos =state_machine.character.position
			state_machine.character.position = Vector2(pos.x, pos.y + 1)
		else:
			state_machine.transition_to("Jumping")
		return

	if not state_machine.character.is_on_floor():
		state_machine.transition_to("Falling")
		return


	
