class_name GridTileMap
extends TileMap


var TileMapCellInfo = load("res://source/blocks/TileMapCellInfo.gd")

func _ready():
	assert(tile_set != null)
	print("GridTileMap - Ready.")

func get_cell_blockname_from_cell(location: Vector2):
	return tile_set.tile_get_name(get_cellv(location))

func extract_cell_info() -> Array:
	var used_tiles = get_used_cells()
	var used_cells = []
	for used_tile in used_tiles:
		var cell_info = TileMapCellInfo.new()
		cell_info.tilemap_location = used_tile
		cell_info.tile_id = get_cell(used_tile.x, used_tile.y)
		used_cells.append()
	return used_cells

func on_TileMap_tile_state_changed(tile_id : int, location : Vector2):
	print("Tie state changed callback: ", tile_id, ", (", location,")")
	set_cell(location.x, location.y, tile_id)
	update_dirty_quadrants()

