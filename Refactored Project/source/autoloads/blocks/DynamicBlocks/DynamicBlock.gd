class_name DynamicBlock
extends RigidBody2D

signal block_changed(block_name)
var should_teleport: bool = false
var node_pos = Vector2(0,0)
var block_name = "" setget block_name_set, block_name_get
func _ready():
	if Blocks.has_block(name):
		block_name_set(self.name)
		
func block_name_get():
	return block_name
	
func block_name_set(new_block_name):
	if new_block_name.length() > 0:
		print("dynamic block set to ", new_block_name)
		block_name = new_block_name
		emit_signal("block_changed", block_name)
#	if block_name != new_block_name and new_block_name.length() > 0:
#		if not Blocks.has_block(new_block_name):
#			print("Error (DynamicBlock.block_name_set): No block found named ", new_block_name)
#			return
#		block_name = new_block_name
#		emit_signal("block_changed", block_name)

func block_changed(_block_name):
	if not Blocks.has_block(_block_name):
		print("Error (DynamicBlock.block_changed: No block found named ", _block_name)
		return
	block_name_set(_block_name)


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


func get_block_texture():
	return $Sprite.texture

func notify_dropped():
	print("dropped block")
	pass
func notify_thrown(throw_force):
	print ("threw block")
	pass
