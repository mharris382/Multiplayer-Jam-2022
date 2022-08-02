class_name DynamicBlock
extends RigidBody2D

var should_teleport: bool = false
var node_pos = Vector2(0,0)

func _integrate_forces(state):
	if should_teleport:
		state.transform.origin.x = node_pos.x
		state.transform.origin.y = node_pos.y
		should_teleport = false

func get_trajectory_point(step, start_pos, velocity: Vector2, gravity: Vector2, time = 1/60.0) -> Vector2:
	var t = time
	var velocity_t = t * velocity
	var gravity_t = t * t * gravity
	return start_pos + step * velocity_t + 0.5 * (step * step + step) * gravity_t

func notify_dropped():
	print("dropped block")
	pass
func notify_thrown(throw_force):
	print ("threw block")
	pass
