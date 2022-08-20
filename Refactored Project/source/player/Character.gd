class_name Character
extends Player

signal facing_direction_changed(is_facing_right)
signal character_state_changed(state, velocity)
signal velocity_changed(velocity)


enum States {
	IDLE, 
	RUNNING, 
	IN_AIR, 
	JUMPING, 
	FALLING 
}

const FLOOR_DETECT_DISTANCE = 20.0
const TERMINAL_Y_VELOCITY = 10000

export var speed = Vector2(150.0, 350.0)
export var extra_jump_time = 0.4
export var jump_pow = .5
var _move_direction : Vector2 = Vector2.ZERO
var _velocity : Vector2 = Vector2.ZERO
var _facing_right : bool = true
var _state = States.IDLE setget _state_set, _state_get
var _jump_timed_out = true

onready var platform_detector = $PlatformDetector
onready var jump_timer = $"JumpTimer"

func _ready():
#	connect("aim_angle_changed", self, "_on_aim_angle_changed")
	connect("move_changed", self, "_on_move_changed")
	jump_timer.connect("timeout", self, "_on_jump_timer_timeout")
	jump_timer.stop()

func _physics_process(delta):
	
	_velocity.y += gravity * delta
	_velocity.y = min(_velocity.y, TERMINAL_Y_VELOCITY)
	
	var direction = _move_direction
	
	if is_on_floor():
		direction.y = -1 if Input.is_action_just_pressed(JUMP % player_number) else 0
		if direction.y == -1:
			_jump_timed_out = false
			jump_timer.start(extra_jump_time)
	else:
		direction.y = -1 if not _jump_timed_out else 0
		if not _jump_timed_out and not Input.is_action_just_released(JUMP % player_number):
			var t = (jump_timer.time_left / extra_jump_time)
			direction.y = -1 * pow(t, jump_pow)
		else:
			direction.y = 0
			_jump_timed_out = true
			jump_timer.stop()
	
	
	var is_jump_interrupted = _jump_timed_out and _velocity.y < 0.0
	_velocity = calculate_move_velocity(_velocity, direction, speed, is_jump_interrupted)
	
	var snap_vector = Vector2.ZERO
	if direction.y == 0.0:
		snap_vector = Vector2.DOWN * FLOOR_DETECT_DISTANCE
		
	var is_on_platform = platform_detector.is_colliding()
	_velocity = move_and_slide_with_snap(_velocity, snap_vector, Vector2.UP, not is_on_platform, 4, 0.9, false)
	
	if direction.x != 0:
		var is_facing_right =direction.x > 0
		_set_facing_direction(is_facing_right)
	
	emit_signal("velocity_changed", _velocity)
	_update_state(_velocity)

func _on_move_changed(move_input):
	_move_direction.x = move_input.x

func ended_jump() -> bool:
	return true
	
func calculate_move_velocity(
		linear_velocity,
		direction,
		speed,
		is_jump_interrupted
	):
	var velocity = linear_velocity
	velocity.x = speed.x * direction.x
	if direction.y != 0.0:
		velocity.y = speed.y * direction.y
	if is_jump_interrupted:
		# Decrease the Y velocity by multiplying it, but don't set it to 0
		# as to not be too abrupt.
		jump_timer.stop()
		velocity.y *= 0.6
		
	return velocity

func _set_facing_direction(facing_right):
	if _facing_right != facing_right:
		_facing_right = facing_right
		emit_signal("facing_direction_changed", 1 if _facing_right else -1)

func _update_state(new_vel : Vector2):
	
	if not is_on_floor():
		if new_vel.y < -0.1:
			_state_set(States.JUMPING)
		elif new_vel.y > 0.1:
			_state_set(States.FALLING)
		else:
			_state_set(States.IN_AIR)
		print("in air")
	else:
		if abs(new_vel.x) > 0.1:
			emit_signal("character_state_changed", States.RUNNING)
#			emit_signal("character_state_changed", _state)
			print("run")
		else:
			emit_signal("character_state_changed", States.IDLE)
#			emit_signal("character_state_changed", _state)
			print("Idle")

func _state_set(state):
	if _state != state:
		_state = state
		emit_signal("character_state_changed", _state)
func _state_get():
	return _state
	
func _on_jump_timer_timeout():
	_jump_timed_out = true
