extends Node


onready var block_library : BlockLibrary = preload("res://assets/Blocks_Final/Blocks_Final.tres")

#block can be either block name, block id, or an array or ids or strings
static func get_block_data(block) -> BlockData:
	match typeof(block):
		TYPE_INT:
			return Blocks.block_library.blocks[block]
		TYPE_STRING:
			return Blocks.block_library.get_block_data(block)
		TYPE_STRING_ARRAY:
			var arr = []
			for s in block:
				arr.append(Blocks.block_library.get_block_data(s))
			return arr
		TYPE_ARRAY:
			var arr = []
			for s in block:
				assert(s is int)
				arr.append(Blocks.block_library[s])
			return arr
	print("ERROR: invalid block id: ", typeof(block))
	return null

static func instance_static_block(block) -> Node2D:
	if !Blocks.block_has_static_scene(block):
		return null
	var block_data = get_block_data(block).static_block.instance()
	
	return null

static func has_block(block) -> bool:
	var block_data = get_block_data(block)
	return block_data != null
	
static func block_has_static_scene(block) -> bool:
	if !has_block(block):
		return false
	return get_block_data(block).static_block != null
