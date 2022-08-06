extends AnimatedSprite

enum {IDLE, RUN, JUMP, SPECIAL, FALL, LAND}
enum {LEFT, RIGHT}

var _d_anim_names = {
	IDLE : "idle",
	RUN : "run",
	JUMP : "jump-rise",
	SPECIAL : "idle",
	FALL : "jump-fall",
	LAND : "idle"
}


var _current_anim = IDLE
var _speed_scale = 1 setget _set_speed_scale
var _facing_dir : int = RIGHT




func _ready():
	_current_anim = IDLE
	play(_current_anim)
	pass

func _process(delta):	
	pass
	
	
	
func _switch_animation(animation) :
	if _current_anim != animation :
		stop()
		play(_d_anim_names[animation])
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

func _set_speed_scale(scale):
	_speed_scale = abs(scale)



## test function
func jump_up():
	if Input.is_action_just_pressed("ui_right"):
		play("jump-rise")
	elif Input.is_action_just_released("ui_right"):
		play("idle")
		


