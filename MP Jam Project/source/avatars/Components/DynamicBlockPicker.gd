extends Area2D

signal block_picked_up(block_data)

#responsibility: emits blocks node on enter and then destroys the block node 
#intention: for use by a pickup system (can be reused for any block destroying area, such as a furnace or recycle bin)

var enable_pickups = true

func _on_Block_Area2D_body_entered(body):
	if enable_pickups and body is DynamicBlock:
		var db = body as DynamicBlock
		emit_signal("block_picked_up", db.block_name)
		body.queue_free()


func _on_Interact_Pickup_Timer_timeout():
	enable_pickups = false


func _on_Builder_try_pickup_blocks():
	enable_pickups = true
