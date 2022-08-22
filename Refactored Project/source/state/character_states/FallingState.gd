extends State

export var smoothing = 2.0

func enter(msg = {}):
	print("Enter: ", name)

func physics_update(_delta: float):
	assert(state_machine.character!=null)
	
	var move_input = state_machine.character._move_direction
	var speed = state_machine.character.speed
	var velocity : Vector2 = state_machine.character._velocity
	
	var direction = Vector2(move_input.x, 1)
	velocity = state_machine.character.calculate_move_velocity(velocity,direction,speed,true)
	state_machine.character.apply_velocity(velocity)
	
	if state_machine.character.is_on_floor():
		state_machine.transition_to("Grounded")
		return
