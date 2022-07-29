class_name GridTileMap
extends TileMap

func get_cell_blockname_from_cell(location: Vector2):
	return tile_set.tile_get_name(get_cellv(location))


func on_TileMap_tile_state_changed(tile_id : int, location : Vector2):
	set_cell(location.x, location.y, tile_id)
	update_dirty_quadrants()
