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

var desired_velocity : Vector2 = Vector2.ZERO setget desired_velocity_set, desired_velocity_get


func desired_velocity_get():
	return desired_velocity

func desired_velocity_set(vel):
	if desired_velocity != vel:
		desired_velocity = vel

onready var platform_detector = $PlatformDetector
onready var jump_timer = $"JumpTimer"

func _ready():
#	connect("aim_angle_changed", self, "_on_aim_angle_changed")
	connect("move_changed", self, "_on_move_changed")
	#jump_timer.connect("timeout", self, "_on_jump_timer_timeout")
	#jump_timer.stop()
	
func process_not_on_floor(direction):
	direction.y = -1 if not _jump_timed_out else 0
	if not _jump_timed_out and not Input.is_action_just_released(JUMP % player_number):
		var t = (jump_timer.time_left / extra_jump_time)
		direction.y = -1 * pow(t, jump_pow)
	else:
		direction.y = 0
		_jump_timed_out = true
		jump_timer.stop()
	return direction

func process_on_floor(direction):
		direction.y = -1 if Input.is_action_just_pressed(JUMP % player_number) else 0
		if direction.y == -1:
			_jump_timed_out = false
			jump_timer.start(extra_jump_time)
		return direction

func get_snap_vector(direction):
	var snap_vector = Vector2.ZERO
	if direction.y == 0.0:
		snap_vector = Vector2.DOWN * FLOOR_DETECT_DISTANCE
	return snap_vector

func is_on_platform() -> bool:
	return  platform_detector.is_colliding()
	
onready var state_machine = $CharacterStateMachine

func _physics_process(delta):
	_velocity.y += gravity * delta
	_velocity.y = min(_velocity.y, TERMINAL_Y_VELOCITY)



func _update_facing_direction(direction):
	if direction.x != 0:
		var is_facing_right =direction.x > 0
		_set_facing_direction(is_facing_right)

func _on_move_changed(move_input):
	_move_direction= move_input
	

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
		#jump_timer.stop()
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
	else:
		
		if abs(new_vel.x) > 0.1:
			emit_signal("character_state_changed", States.RUNNING)
		else:
			emit_signal("character_state_changed", States.IDLE)

func _state_set(state):
	if _state != state:
		_state = state
		emit_signal("character_state_changed", _state)

func _state_get():
	return _state

func input_get_move_direction():
	return _move_direction
	
func _on_aim_changed(aim_direction):
	pass
	
func _on_aim_angle_changed(aim_direction, aim_angle):
	pass


func apply_velocity(new_velocity : Vector2):
	var direction = new_velocity.normalized()
	var snap_vector = get_snap_vector(direction)
	var stop_on_slope = not is_on_platform()
	_velocity = move_and_slide_with_snap(new_velocity, snap_vector, Vector2.UP, stop_on_slope, 4, 0.9, false)
	emit_signal("velocity_changed", _velocity)
	_update_facing_direction(direction)
	_update_state(_velocity)
