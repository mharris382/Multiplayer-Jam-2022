extends Node2D

signal aim_direction_changed(aim_direction)
signal aiming_state_changed(is_aiming)

var _player = null
var is_aiming = false setget is_aiming_set, is_aiming_get
var aim_angle = 0.0 setget aim_angle_set, aim_angle_get



func _ready():
	_player = get_parent()

func _process(delta):
	_on_aiming_state_changed(Input.is_action_just_pressed(_player.get_player_specific_input_action("ability_mode")))

func _on_aim_changed(aim_direction : Vector2):
	if is_aiming and abs(aim_direction.length_squared()) > 0.01:
		aim_angle_set(atan2(aim_direction.y, aim_direction.x))

func _on_aiming_state_changed(aiming):
	if is_aiming != aiming:
		is_aiming = aiming
		emit_signal("aiming_state_changed", is_aiming)

func get_aim_direction()-> Vector2:
	return Vector2(cos(aim_angle), sin(aim_angle))

#-------------------------------------------------
#Getters/Setters

func aim_angle_get():
	return aim_angle
	
func aim_angle_set(angle):
	aim_angle = angle
	
func is_aiming_get():
	return is_aiming
	
func is_aiming_set(aiming : bool):
	_on_aiming_state_changed(aiming)


func _on_Player_aim_angle_changed(aim_direction, aim_angle):
	aim_angle_set(aim_angle)
