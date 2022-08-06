extends Area2D

signal block_picked_up(block_data)



var enable_pickups = true

func _on_Block_Area2D_body_entered(body):
	if enable_pickups and body is DynamicBlock:
		var db = body as DynamicBlock
		var data = db.block_data
		if data == null:
			data = Blocks.get_null_object_block_data()
			assert(data != null)
		emit_signal("block_picked_up", data)
		body.queue_free()


func _on_Interact_Pickup_Timer_timeout():
	enable_pickups = false


func _on_Builder_try_pickup_blocks():
	enable_pickups = true
