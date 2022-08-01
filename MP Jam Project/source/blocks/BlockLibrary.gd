class_name BlockLibrary
extends Resource

export(Array, Resource) var blocks = []
var block_tile_set : TileSet = preload("res://assets/blocks_tileset.tres")
var tile_lookup = {}


func tile_get_static_functionality(tile_name : String):
	pass

func tile_get_dynamic_block_prefab(tile_name : String) -> PackedScene:
	return null

func get_tile_id(tile_name : String):
	if(!tile_lookup.has(tile_name)):
		return -1
	return tile_lookup[tile_name]

func _ready():
	assert(block_tile_set != null)
	var tiles = block_tile_set.get_tiles_ids()
	for tile_id in tiles:
		var tile_name = block_tile_set.tile_get_name(tile_id)
		#tile_lookup[tile_name] = BlockData.new(tile_id)

