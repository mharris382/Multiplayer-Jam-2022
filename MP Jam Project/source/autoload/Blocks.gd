extends Node


onready var tile_set :TileSet= preload("res://assets/Blocks_Final/TileSet_Blocks.tres")
onready var block_library  = preload("res://assets/Blocks_Final/Blocks_Final.tres")
onready var dynamic_block_null_object = preload("res://scenes/objects/DynamicBlock.tscn")
onready var universal_dynamic_block = preload("res://scenes/dynamic_blocks/DynamicBlock_UniversalBlock.tscn")

var remapped_suffixes = [
	"_On", "_Off"
]

static func get_null_object_block_data():
	return Blocks.block_library.blocks[0]
#block can be either block name, block id, or an array or ids or strings
static func get_block_data(block) -> BlockData:
	if block is BlockData or (block == null):
		return block
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

static func get_block_tile_name(block):
	assert(block !=null)
	if block is BlockData:
		if block.resource_name != block.tile_name:
			print("ERROR The block: ", block.resource_name, " does not match the tile name: ", block.tile_name)	
		return block.resource_name
	elif block is String:
		
		pass
		
static func remap_block_name(block_name:String)->String:
	for suffix in Blocks.remapped_suffixes:
			if block_name.ends_with(suffix):
				return block_name.left(block_name.length() - suffix.length())
	return block_name



static func build_static_block(block, grid_position, target_tile_map):
	if target_tile_map == null:
		target_tile_map = Blocks.get_tree().get_nodes_in_group("Tilemap")

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


func instance_dynamic_block_at_location(block, location, parent_node):
	if parent_node == null:
		parent_node = self
	if not Blocks.has_block(block):
		print("ERROR at Blocks.instance_dynamic_block_at_location")
		return
		
	var instance = Blocks.universal_dynamic_block.instance()
	instance.name = block
	instance.block_name = block
	instance.position = location
	print("Created Dynamic Block(", instance.block_name, ") at location: ", location)
	if parent_node !=null:
		parent_node.add_child(instance)
	return instance

func instance_dynamic_block(block):
	return instance_dynamic_block_at_location(block, Vector2.ZERO, self)
