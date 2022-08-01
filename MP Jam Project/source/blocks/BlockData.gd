class_name BlockData
extends Resource

export var block_name : String
export var dynamic_block : PackedScene



func spawn_dynamic_block(transform : Transform2D) -> DynamicBlock:
	var block = dynamic_block.instance() as DynamicBlock
	assert(block != null)
	block.position = transform.get_origin()
	block.rotation = transform.get_rotation()
	block.scale = transform.get_scale()
	return block
