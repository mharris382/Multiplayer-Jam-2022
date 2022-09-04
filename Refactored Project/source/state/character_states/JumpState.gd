extends State


export var extra_jump_time = 0.4
export var jump_pow = .5

var _jump_timed_out = false

onready var jump_timer = $JumpTimer


func handle_input(_event: InputEvent):
	var player_num = state_machine.character.player_number
	if Input.is_action_just_released("jump_p%d"%player_num):
		_jump_timed_out = true
	

func physics_update(_delta: float):
	var move_input = state_machine.character._move_direction
	var speed = state_machine.character.speed
	var velocity = state_machine.character._velocity
	
	var t = (jump_timer.time_left / extra_jump_time)
	var direction = Vector2(move_input.x, -1 * pow(t, jump_pow))
	
	velocity = state_machine.character.calculate_move_velocity(velocity,direction,speed,false)
	state_machine.character.apply_velocity(velocity)
	
	var player_num = state_machine.character.player_number
	if Input.is_action_just_released("jump_p%d"%player_num) or state_machine.character.is_on_ceiling():
		_jump_timed_out = true
		state_machine.transition_to("Falling")
		return
	
	elif state_machine.character.is_on_floor():
		state_machine.transition_to("Grounded")
		return

func enter(_msg := {}) -> void:
	assert(is_owned_by_player())
	print("Enter: ", name)
	_jump_timed_out = false
	jump_timer.connect("timeout", self, "_on_jump_timer_timeout")
	jump_timer.start(extra_jump_time)

func exit():
	jump_timer.disconnect("timeout", self, "_on_jump_timer_timeout")
	jump_timer.stop()

func _on_jump_timer_timeout():
	_jump_timed_out = true
	state_machine.transition_to("Falling")

