extends AnimatedSprite

enum {IDLE, RUN, JUMP, SPECIAL, FALL, LAND}
enum Direction {LEFT = -1, RIGHT = 1}

var _d_anim_names = {
	IDLE : "idle",
	RUN : "run",
	JUMP : "jump-rise",
	SPECIAL : "idle",
	FALL : "jump-fall",
	LAND : "idle"
}

var _default_anim = IDLE
var _current_anim = IDLE
var _speed_scale = 1 setget set_speed_scale
var _facing_dir = Direction.RIGHT
var in_air = false
var last_frame_position


func _ready():
	_current_anim = IDLE
	flip_h = false
	play(_d_anim_names[_current_anim])
	last_frame_position = get_transform().get_origin()
	pass
#
#func _process(delta):
#	var position = get_transform().get_origin()
#	var air_direction = position.y - last_frame_position.y
#	last_frame_position = position
#	if in_air:
#		if air_direction > 0:
#			on_jump_start()
#			pass
#		elif air_direction < 0:
#			on_fall()
#			pass
#		else:
#			#at peak
#			pass
#
#	if Input.is_action_just_pressed("ui_up") :
#		on_jump_start()
#
#	elif Input.is_action_pressed("ui_right") :
#		on_run_start()
#
#	elif Input.is_action_just_pressed("ui_down") :
#		on_fall()
#
#	elif Input.is_action_just_pressed("ui_left"):
#		on_stop()
#
#	else:
#		pass
#
#	pass
	
	
	

	
func on_jump_start():
	_switch_animation(JUMP)
	pass
	
	
func on_run_start(run_scale = 0):
	if run_scale != 0 :
		set("_speed_scale", run_scale)
	_switch_animation(RUN)
	pass
  
	
func on_stop():
	_switch_animation((IDLE))
	pass
	
	
func on_fall() :
	_switch_animation(FALL)
	pass
	
	
func on_land() :
	_switch_animation(LAND)
	pass


func face_direction(direction):
	flip_h = direction is Direction.LEFT
	pass

func _switch_animation(animation) :
	if _current_anim != animation :
		_current_anim = animation
		stop()
		play(_d_anim_names[animation])
	pass
	
	
func set_speed_scale(scale):
	_speed_scale = abs(scale)



## test function
func jump_up():
	if Input.is_action_just_pressed("ui_right"):
		play("jump-rise")
	elif Input.is_action_just_released("ui_right"):
		play("idle")
		




func _on_character_state_changed(prev_state, next_state, move_input):
	#print("CharacterAnimator. on character state changed from: ", prev_state, " to ", next_state)
	if prev_state == CharacterBase.PlayerState.IN_AIR:
		#just landed
		on_land()
		pass
	match next_state:
		CharacterBase.PlayerState.IDLE:
			on_stop()
			pass
		
		CharacterBase.PlayerState.RUNNING:
			on_run_start()
			pass
		
		CharacterBase.PlayerState.IN_AIR:
			if move_input.y > 0:
				on_jump_start()
			else:
				on_fall()
			pass
	pass



func _on_Animator_animation_finished():
	pass # Replace with function body.



func _on_character_changed_direction(direction):
	if direction == 1:
		flip_h = false
	else:
		flip_h = true
