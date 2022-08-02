class_name BlockLibrary, "res://assets/ui/editor/icon-block.png"
extends Resource

export(Array, Resource) var blocks = []
var block_tile_set : TileSet = preload("res://assets/blocks/Blocks_TileSet(placeholder).tres")
var tile_lookup = {}


func get_tile_id(tile_name : String):
	if(!tile_lookup.has(tile_name)):
		return -1
	return tile_lookup[tile_name]
	
	
func get_block_data(tile_name):
	if(!tile_lookup.has(tile_name)):
		return -1
	return tile_lookup[tile_name]

func _ready():
	assert(block_tile_set != null)
	var tiles = block_tile_set.get_tiles_ids()
	for tile_id in tiles:
		var tile_name = block_tile_set.tile_get_name(tile_id)
		#tile_lookup[tile_name] = BlockData.new(tile_id)
