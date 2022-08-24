extends Node2D

signal start_aiming_tool()
signal aim_tool(delta, aim_direction, aim_angle)
signal stop_aiming_tool()

export var rotate_tool_while_aiming = false
export var snap_rotation = false
export var rotate_speed = 3.15
var _is_aiming = false
var _aim_direction = Vector2.RIGHT
var _angle = 0.0
var _player = null

func _ready():
	_player = get_parent().get_parent()
	if rotate_tool_while_aiming:
		connect("aim_tool", self, "_rotate_tool_to_aim")
	pass
	
func _process(delta):
	if Input.is_action_pressed(_player.get_player_specific_input_action("ability_mode")):
		emit_signal("aim_tool", delta, _aim_direction.normalized(), _angle)
		if rotate_tool_while_aiming:
			var new_transform = Transform2D.IDENTITY.rotated(_player._aim_angle)
			transform = transform.interpolate_with(new_transform, delta * rotate_speed) if not snap_rotation else new_transform
	
func _on_AimController_aiming_state_changed(is_aiming):
	_is_aiming = is_aiming
	if is_aiming:
		emit_signal("start_aiming_tool")
	else:
		emit_signal("stop_aiming_tool")


func _on_AimController_aim_direction_changed(aim_direction):
	_aim_direction = aim_direction
	_angle = atan2(aim_direction.y, aim_direction.x)
	emit_signal("aim_tool", aim_direction, _angle)
	

func _rotate_tool_to_aim(delta, direction, angle):
	return
	var new_transform = Transform2D.IDENTITY.rotated(angle)
	transform = transform.interpolate_with(new_transform, delta * rotate_speed) if not snap_rotation else new_transform
