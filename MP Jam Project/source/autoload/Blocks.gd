extends Node


onready var block_library : BlockLibrary = preload("res://assets/BlockLibrary.tres")

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
