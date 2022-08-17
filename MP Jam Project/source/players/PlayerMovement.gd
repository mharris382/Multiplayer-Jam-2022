class_name PlayerMovement
extends KinematicBody2D

signal move_input_changed(new_move_input)
signal look_input_changed(new_look_input)
signal facing_right_changed(is_facing_right)
signal jumping_changed(is_jumping)

export var player_speed = 500 #200
export var jump_force = -300 #-100
export var gravity = 1200 #800
export var push_force = 50

var velocity = Vector2.ZERO
var jump_input : bool
var jump_just_pressed : bool
var jump_pressed : bool

var facing = "right"


var look_input : Vector2 = Vector2.ZERO setget look_input_set, look_input_get
var move_input : Vector2 = Vector2.ZERO setget move_input_set, move_input_get
var is_facing_right : bool = false setget is_facing_right_set, is_facing_right_get
var is_jumping = false setget is_jumping_set, is_jumping_get

func can_jump():
	return is_jumping or is_on_floor()

func is_jumping_set(should_be_jumping):
	if is_jumping != should_be_jumping:
		if should_be_jumping and not can_jump(): 
			return
		
		is_jumping = should_be_jumping
		emit_signal("jumping_changed",  is_jumping)

func is_facing_right_set(face_right):
	if is_facing_right != face_right:
		is_facing_right = face_right
		emit_signal("facing_right_changed", is_facing_right)

func move_input_set(move):
	if move != move_input:
		move_input = move
		emit_signal("move_input_changed", move_input)
#		if sign(move_input.x) != sign(move.x) and move.x != 0:
#			is_facing_right_set(move.x > 0)

func look_input_set(look):
	if look != look_input:
		look_input = look
		emit_signal("look_input_changed", look_input)

func look_input_get():
	return look_input

func is_jumping_get():
	return is_jumping

func is_facing_right_get():
	return is_facing_right

func move_input_get():
	return move_input

onready var jump_detection = $JumpDetection #this is also a timer? not sure what it is used for

func _physics_process(delta):
	player_input()
	velocity.y += delta * gravity
	velocity = move_and_slide(velocity, Vector2.UP, false, 4, PI/4, false)
	apply_push_to_block()


func flip():
	var sprite = $Sprite as Sprite
	sprite.flip_h = !sprite.flip_h
	
	$"ControlPoints/Front".position.x *=-1
	if has_node("ControlPoints/Aim"):
		$"ControlPoints/Aim".position.x *=-1

func player_input():
	velocity.x = 0

	if move_input.x < -0.1:
		velocity.x -= player_speed		
		if(facing == "right" ):			
			flip()
			facing="left"
			
	elif move_input.x > 0.1:
		velocity.x += player_speed
		if(facing == "left" ):			
			flip()
			facing="right"
			
	#if Input.is_action_pressed("move_left_%s" % player_id):
	#if Input.is_action_pressed("move_right_%s" % player_id):
	if is_on_floor() and just_jumped():
		velocity.y = jump_force
		jump_detection.start(0.5)
	if !jump_detection.is_stopped():
		if jump_pressed:
			velocity.y += jump_force/8
		else:
			jump_detection.stop()
			
func just_jumped():
	if jump_just_pressed:
		jump_just_pressed = false
		return true
	return false

func apply_push_to_block():
	for num in get_slide_count():
		var collision = get_slide_collision(num)
		if collision.collider.is_in_group("dynamic_block"):
			collision.collider.apply_central_impulse(-collision.normal * push_force)

#on contact with jumper block
func block_jumper():
	velocity.y = jump_force*3
func _change_anim():
	pass

func on_jump_start():
	pass

func on_jump_stopped():
	pass
	
