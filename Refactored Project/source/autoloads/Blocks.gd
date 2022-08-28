extends Node

const MISSING_BLOCK_NAME = "Block_Base"
const MISSING_BLOCK_RECT = Rect2(0, 0, 128, 128)
#
onready var block_library = preload("res://assets/Blocks_Final/Blocks_Final.tres")
onready var universal_dynamic_block = preload("res://scenes/dynamic_blocks/DynamicBlock_UniversalBlock.tscn")

var steam_tilemap
var block_tilemap
var steam_neighbor_total = {}
var dirty_tiles = {}

static func mark_dirty(cell, dirty):
	if not dirty:
		Blocks.dirty_tiles[cell] = true
	else:
		Blocks.dirty_tiles[cell] = false
		
func get_block_data(block):
	if block == null:
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
	return block

func instance_static_block(block) -> Node2D:
	if !Blocks.block_has_static_scene(block):
		return null
	var block_data = get_block_data(block).static_block.instance()
	return block_data
#
func has_block(block) -> bool:
	var block_data = get_block_data(block)
	return block_data != null

func instance_dynamic_block_at_location(block, location, parent_node):
	if parent_node == null:
		parent_node = self
	if not Blocks.has_block(block):
		print("ERROR at Blocks.instance_dynamic_block_at_location")
		return

	var instance = Blocks.universal_dynamic_block.instance()
	instance.name = block
	instance.block_name = block
	instance.node_pos = location
	instance.should_teleport = true
	print("Created Dynamic Block(", instance.block_name, ") at location: ", location)
	if parent_node !=null:
		parent_node.add_child(instance)
	return instance
#
func instance_dynamic_block(block):
	return instance_dynamic_block_at_location(block, Vector2.ZERO, self)

func block_get_texture_rect(block_name):
	if not has_block(block_name):
		return MISSING_BLOCK_RECT
	var data = Blocks.get_block_data(block_name)

	var result = Blocks.block_library.get_block_tile_rect(block_name) as Rect2
	if result.size.x == 0 or result.size.y == 0:
		return MISSING_BLOCK_RECT
	return result

func block_get_texture(block_name):
	if not has_block(block_name):
		assert(has_block(MISSING_BLOCK_NAME))
		return Blocks.block_library.get_block_tile_texture(MISSING_BLOCK_NAME)
	return Blocks.block_library.get_block_tile_texture(block_name)


#
#static func block_has_static_scene(block) -> bool:
#	if !has_block(block):
#		return false
#	return get_block_data(block).static_block != null
#
#
#
#
#
#var remapped_suffixes = [
#	"_On", "_Off"
#]
##
#static func get_null_object_block_data():
#
#	return Blocks.block_library.blocks[0]
##block can be either block name, block id, or an array or ids or strings
#static func get_block_data(block):
#	if block is BlockData or (block == null):
#		return block
#	match typeof(block):
#		TYPE_INT:
#			return Blocks.block_library.blocks[block]
#		TYPE_STRING:
#			return Blocks.block_library.get_block_data(block)
#		TYPE_STRING_ARRAY:
#			var arr = []
#			for s in block:
#				arr.append(Blocks.block_library.get_block_data(s))
#			return arr
#		TYPE_ARRAY:
#			var arr = []
#			for s in block:
#				assert(s is int)
#				arr.append(Blocks.block_library[s])
#			return arr
#	print("ERROR: invalid block id: ", typeof(block))
#	return null
#
#static func get_block_tile_name(block):
#	assert(block !=null)
#	if block is BlockData:
#		if block.resource_name != block.tile_name:
#			print("ERROR The block: ", block.resource_name, " does not match the tile name: ", block.tile_name)	
#		return block.resource_name
#	elif block is String:
#
#		pass
#
#static func remap_block_name(block_name:String)->String:
#	for suffix in Blocks.remapped_suffixes:
#			if block_name.ends_with(suffix):
#				return block_name.left(block_name.length() - suffix.length())
#	return block_name
#
#
#
#static func build_static_block(block, grid_position, target_tile_map):
#	if target_tile_map == null:
#		target_tile_map = Blocks.get_tree().get_nodes_in_group("Tilemap")
#
