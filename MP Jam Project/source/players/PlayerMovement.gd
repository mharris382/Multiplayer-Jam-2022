class_name PlayerMovement
extends KinematicBody2D

export var player_speed = 500 #200
export var jump_force = -300 #-100
export var gravity = 1200 #800
export var player_id = "p1"

var velocity = Vector2.ZERO
var is_jumping = false

onready var jump_detection = $JumpDetection


func _change_anim():
	pass


func player_input():
	velocity.x = 0
	if Input.is_action_pressed("move_left_%s" % player_id):
		velocity.x -= player_speed
	if Input.is_action_pressed("move_right_%s" % player_id):
		velocity.x += player_speed
	if is_on_floor() and Input.is_action_just_pressed("jump_%s" % player_id):
		velocity.y = jump_force
		jump_detection.start(0.5)
	if !jump_detection.is_stopped():
		if Input.is_action_pressed("jump_%s" % player_id):
			velocity.y += jump_force/8
		else:
			jump_detection.stop()


func _physics_process(delta):
	player_input()
	velocity.y += delta * gravity
	velocity = move_and_slide(velocity, Vector2.UP)


#on contact with jumper block
func block_jumper():
	velocity.y = jump_force*3