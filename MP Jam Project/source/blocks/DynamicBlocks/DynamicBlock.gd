class_name DynamicBlock
extends RigidBody2D

var impulse_vel = Vector2(100,-100)
var prev_impulse = impulse_vel
var gravity = ProjectSettings.get("physics/2d/default_gravity")
var array : PoolVector2Array = []

func _input(event):
	if event is InputEventKey:
		if event.scancode == KEY_LEFT and event.pressed:
			impulse_vel.x -= 5
		if event.scancode == KEY_RIGHT and event.pressed:
			impulse_vel.x += 5
		if event.scancode == KEY_DOWN and event.pressed:
			impulse_vel.y += 5
		if event.scancode == KEY_UP and event.pressed:
			impulse_vel.y -= 5
		if event.scancode == KEY_SPACE and event.pressed:
			apply_impulse(Vector2(), impulse_vel)
	

func get_trajectory_point(step, start_pos, velocity: Vector2, gravity: Vector2, time = 1/60.0) -> Vector2:
	var t = time
	var velocity_t = t * velocity
	var gravity_t = t * t * gravity
	return start_pos + step * velocity_t + 0.5 * (step * step + step) * gravity_t


func _process(delta):
	if impulse_vel != prev_impulse:
		prev_impulse = impulse_vel
		add_drawing_points(delta)
		update()
	

func add_drawing_points(delta: float):
	array.resize(0)
	for i in range(300):
		array.append(get_trajectory_point(i, Vector2(0,0), impulse_vel, Vector2(0, gravity*gravity_scale), delta))	


func _draw():
	for result in array:
		draw_circle(result, 2, Color(255,255,255))

func notify_dropped():
	print("dropped block")
	pass
func notify_thrown(throw_force):
	print ("threw block")
	pass
