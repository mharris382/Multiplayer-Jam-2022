class_name BlockLauncher
extends Node

#magnitude of force to launch blocks
export var launch_force = 100

onready var launch_direction :RayCast2D = $"Launch Direction"

var facing_right : bool = true
var held_block : DynamicBlock setget held_block_set, held_block_get

func get_launch_force():
	return launch_direction.transform.xform(Vector2.RIGHT) * launch_force

func drop_held_block():
	if held_block != null:
		held_block.mode = RigidBody2D.MODE_RIGID
		held_block.notify_dropped() #not sure if we'll need this 
		#see YNGNI principle
		
func grab_held_block():
	if held_block == null:
		return
	held_block.mode = RigidBody2D.MODE_KINEMATIC
	launch_direction.add_child(held_block)
	
func throw_held_block():
	if held_block == null:
		return
	var released_block = held_block
	drop_held_block()
	#NOTE: we can disable this check if we don't like it
	if launch_direction.is_colliding():
		return
	released_block.add_central_force(launch_force)
	released_block.notify_thrown(launch_force)
	
# Property getter/setters
func held_block_set(new_block):
	drop_held_block()
	held_block = new_block
	if held_block != null:
		grab_held_block()
	
func held_block_get():
	return held_block


	

