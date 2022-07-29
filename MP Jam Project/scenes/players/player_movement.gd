extends KinematicBody2D

export var player_speed = 200
export var jump_force = -100
export var gravity = 800
export var player_id = "p1"
var velocity = Vector2.ZERO


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


func _physics_process(delta):
	player_input()
	velocity.y += delta * gravity
	velocity = move_and_slide(velocity, Vector2.UP)
