class_name Player
extends KinematicBody2D





signal move_changed(move_direction)

#difference between the two of these signals is that the latter signal will not fire if there is no aim input (i.e. direction = Vector.ZERO), whereas the other fires regardless
signal aim_input(aim_direction)
signal aim_angle_changed(aim_direction, aim_angle)

const JUMP = "jump_p%d"

const INTERACT = "interact_p%d"

const MOVE_LEFT = "move_left_p%d"
const MOVE_RIGHT = "move_right_p%d"
const MOVE_UP = "move_up_p%d"
const MOVE_DOWN = "move_down_p%d"

const USE = "ability_p%d"
const AIM = "ability_mode_p%d"

const AIM_LEFT = "aim_left_p%d"
const AIM_RIGHT = "aim_right_p%d"
const AIM_UP = "aim_up_p%d"
const AIM_DOWN = "aim_down_p%d"

const SELECT_NEXT_BLOCK = "next_block_p%d"
const SELECT_PREV_BLOCK = "previous_block_p%d"

const SELECT_NEXT_ABILITY = "tool_right_p%d"
const SELECT_PREV_ABILITY = "tool_left_p%d"


export var player_number = 1


var _move_input: Vector2
var _aim_angle : float



onready var gravity = ProjectSettings.get("physics/2d/default_gravity")


func _process(delta):
	_process_movement()
	_process_jump()
	_process_interact()
	_process_block_select()
	_process_ability_select()
	_process_ability_aim()
	_process_ability()

func _process_movement():
	var move_input = Input.get_vector(MOVE_LEFT % player_number, MOVE_RIGHT % player_number, MOVE_UP % player_number, MOVE_DOWN % player_number)
	emit_signal("move_changed", move_input)
	
func _process_jump():
	pass
func _process_interact():
	pass

func _process_ability_select():
	pass
	
func _process_block_select():
	pass
	
func _process_ability_aim():
	var aim  = Input.get_vector(AIM_LEFT % player_number, AIM_RIGHT % player_number, AIM_UP % player_number, AIM_DOWN % player_number)
	emit_signal("aim_input", aim)
	if abs(aim.length_squared()) < 0.1:
		return
	emit_signal("aim_angle_changed", aim, _aim_angle)
	_aim_angle = atan2(aim.y, aim.x)
	
func _process_ability():
	pass


func _aim_vector_get():
	return Vector2(cos(_aim_angle), sin(_aim_angle))
	
func _aim_vector_set(aim_vector:Vector2):
	if abs(aim_vector.length_squared()) < 0.2:
		return
	_aim_angle = atan2(aim_vector.y, aim_vector.x)
	emit_signal("aim_angle_changed", _aim_vector_get(), _aim_angle)

#Even though I made consts for all the actions, it's likely the actions will need to be polled elsewhere so instead of injecting the player_number into all I opted to inject the player itself and expose this method
func get_player_specific_input_action(action):
	return "%s_p%d"%[action, player_number]


