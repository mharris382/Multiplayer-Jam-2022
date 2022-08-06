extends Node


onready var tile_set :TileSet= preload("res://assets/Blocks_Final/TileSet_Blocks.tres")
onready var block_library  = preload("res://assets/Blocks_Final/Blocks_Final.tres")
onready var dynamic_block_null_object = preload("res://scenes/objects/DynamicBlock.tscn")

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

static func instance_dynamic_block(block):
	
	var block_data = get_block_data(block)
	if block_data == null or !block_data.dynamic_block !=null:
		
		var null_block= Blocks.dynamic_block_null_object
		var sprite = null_block.get_node("Sprite") as Sprite
		
		if sprite == null:
			print("Null Dynamic Block missing sprite")
			return null_block
	
		var tile_id = Blocks.tile_set.find_tile_by_name(block_data.resource_name)
		
		if tile_id == -1:
			Blocks.tile_set.find_tile_by_name(block_data.tile_name)
			if tile_id == -1:
				return null_block
		
		sprite.region_enabled = true
		sprite.region_rect = Blocks.tile_set.tile_get_region(tile_id)
		return null_block
	return Blocks.get_block_data(block).dynamic_block



static func instance_static_block(block) -> Node2D:
	if !Blocks.block_has_static_scene(block):
		return null
	var block_data = get_block_data(block).static_block.instance()
	return block_data

static func has_block(block) -> bool:
	var block_data = get_block_data(block)
	return block_data != null
	
static func block_has_static_scene(block) -> bool:
	if !has_block(block):
		return false
	return get_block_data(block).static_block != null
