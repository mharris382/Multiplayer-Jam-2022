class_name Actor
extends KinematicBody2D


signal facing_right_changed(is_facing_right)
signal move_input_changed(new_move_input)
signal jumping_changed(is_jumping)

export var speed = Vector2(150.0, 350.0)
onready var gravity = ProjectSettings.get("physics/2d/default_gravity")

const FLOOR_NORMAL = Vector2.UP

var is_facing_right : bool = false setget is_facing_right_set, is_facing_right_get
var move_input : Vector2 = Vector2.ZERO setget move_input_set, move_input_get
var is_jumping = false setget is_jumping_set, is_jumping_get


var _velocity = Vector2.ZERO
var _jump_just_pressed = false
var _jump_just_released = false

onready var platform_detector = $PlatformDetector


func _ready():
	connect("jumping_changed", self, "_on_jump_changed")

func _process(delta):
	var start_jumping = _jump_just_pressed and is_on_floor()
	var direction = Vector2(move_input.x, -1 if start_jumping else 0)
	var is_jump_interupted = _jump_just_released and _velocity.y < 0.0
	
	_velocity = calclulate_move_velocity(_velocity, direction, speed, is_jump_interupted)
	_reset_frame_event_flags()


func _physics_process(delta):
	_velocity.y += gravity * delta

func get_direction():
	return Vector2(move_input.x, move_input.y)

func calclulate_move_velocity(linear_velocity, direction, speed, is_jump_interupted):
	var velocity = linear_velocity
	velocity.x = speed.x * direction.x
	if direction.y != 0.0:
		velocity.y = speed.y * direction.y
	if is_jump_interupted:
		velocity.y *= 0.6
	return velocity

func _on_jump_changed(jump):
		_jump_just_pressed = jump
		_jump_just_released = not jump

func _reset_frame_event_flags():
	_jump_just_pressed = false
	_jump_just_released = false


#------------------------------------
#Character facing right

func is_facing_right_get():
	return is_facing_right

func is_facing_right_set(face_right):
	if is_facing_right != face_right:
		is_facing_right = face_right
		emit_signal("facing_right_changed", is_facing_right)


#------------------------------------
#character is jumping

func can_jump():
	return is_jumping or is_on_floor()
	
func is_jumping_get():
	return is_jumping

func is_jumping_set(should_be_jumping):
	if is_jumping != should_be_jumping:
		if should_be_jumping and not can_jump(): 
			return
		
		is_jumping = should_be_jumping
		emit_signal("jumping_changed",  is_jumping)


#------------------------------------
#Move input

func move_input_set(move):
	if move != move_input:
		move_input = move
		emit_signal("move_input_changed", move_input)
#		if sign(move_input.x) != sign(move.x) and move.x != 0:
#			is_facing_right_set(move.x > 0)

func move_input_get():
	return move_input
