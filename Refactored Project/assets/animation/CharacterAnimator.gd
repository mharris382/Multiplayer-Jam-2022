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

var last_velocity

func _ready():
	_current_anim = IDLE
	flip_h = false
	play(_d_anim_names[_current_anim])
	last_frame_position = get_transform().get_origin()
	pass

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


func set_speed_scale(scale):
	_speed_scale = abs(scale)


func _switch_animation(animation) :
	if _current_anim != animation :
		_current_anim = animation
		stop()
		play(_d_anim_names[animation])
	pass

var prev_state = Character.States.IN_AIR

func _on_character_state_changed(next_state):
	#print("CharacterAnimator. on character state changed from: ", prev_state, " to ", next_state)
	if prev_state == next_state:
		return
	if prev_state == Character.States.IN_AIR :
		#just landed
		on_land()
	match next_state:
		Character.States.IDLE:
			on_stop()
		Character.States.RUNNING:
			on_run_start()
		Character.States.IN_AIR, Character.States.JUMPING, Character.States.FALLING:
			if last_velocity.y > 0:
				on_jump_start()
			else:
				on_fall()
	prev_state = next_state


func _on_character_changed_direction(direction):
	flip_h = direction == -1
	


func _on_velocity_changed(velocity):
	last_velocity = velocity
