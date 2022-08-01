class_name BlockData
extends Resource

#Character Id: 1=Transporter, 2=Builder
signal character_collected_block(character_id, block_id, dynamic_block)

signal builder_built_block(builder, block_id, grid_position)

signal transporter_picked_up_block(transporter, dynamic_block)
signal transporter_dropped_block(transporter, idk_what_here)
signal transporter_disconnected_block(transporter, block_id, grid_position)
signal transporter_threw_block(transporter, dynamic_block, throw_force)

export var block_name : String
export var dynamic_block : PackedScene



func spawn_dynamic_block(transform : Transform2D) -> DynamicBlock:
	var block = dynamic_block.instance() as DynamicBlock
	assert(block != null)
	block.position = transform.get_origin()
	block.rotation = transform.get_rotation()
	block.scale = transform.get_scale()
	return block
