class_name PlayerMovement
extends KinematicBody2D

export var player_speed = 500 #200
export var jump_force = -300 #-100
export var gravity = 1200 #800
export var player_id = "p1"
export var push_force = 50
export var max_jump_time = 1

var velocity = Vector2.ZERO
var is_jumping = false
var move_input : Vector2
var jump_input : bool
var jump_just_pressed : bool
var jump_pressed : bool

var facing = "right"

onready var jump_detection = $JumpDetection #this is also a timer? not sure what it is used for
onready var jump_timer = $JumpTimer

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
	
