class_name BlockLibrary, "res://assets/ui/editor/icon-block.png"
extends Resource

export var bugfixes2 = 1
export var bugfixes = 1
export(Array, Resource) var blocks = []

var block_tile_set : TileSet = preload("res://assets/Blocks_Final/auto-tiles/Block_Base_autotile.tres")
var tile_lookup = {}


func get_tile_id(tile_name : String):
	if(!tile_lookup.has(tile_name)):
		return -1
	return tile_lookup[tile_name]
	
func get_block_data(tile_name):
	tile_name = _remap_tile_name(tile_name)
	for item in blocks:
		if item.tile_name == tile_name:
			return item
	return null

func _remap_tile_name(tile_name)->String:
	if tile_name.ends_with("_On"):
		print("Found Special Tile: ", tile_name)
		return tile_name.left(tile_name.length()-3)
	elif tile_name.ends_with("_Off"):
		print("Found Special Tile: ", tile_name)
		return tile_name.left(tile_name.length()-4)
	else:
		return tile_name
		
func _ready():
	assert(block_tile_set != null)
	var tiles = block_tile_set.get_tiles_ids()
	for tile_id in tiles:
		var tile_name = block_tile_set.tile_get_name(tile_id)
		#tile_lookup[tile_name] = BlockData.new(tile_id)
