class_name Cell
extends Node2D

signal tile_state_changed

var tile_map : TileMap
var grid_transform : Transform2D
var grid_pos : Vector2 setget grid_pos_set, grid_pos_get 

var tile_id = -1 setget tile_id_set, tile_id_get
var tile_name = "" setget tile_name_set


func _init(map : TileMap, grid_transform : Transform2D, x:int, y:int):
	assert(map != null)
	var tree = get_tree()
	
	tile_map = map
	assert(tile_map.tile_set != null)

func tile_name_get():
	if tile_id == -1:
		return ""
	return tile_map.tile_set.tile_get_name(tile_id)

func tile_name_set(new_value):
	if tile_name != new_value:
		if new_value != "":
			tile_id = tile_map.tile_set.find_tile_by_name(new_value)
		else:
			tile_id = -1
		tile_name = new_value

func tile_id_set(new_tile_id : int):
	if(tile_id != new_tile_id):
		#broadcast tile id changed event
		tile_id = new_tile_id
		tile_name = tile_map.tile_set.tile_get_name(tile_id)
		emit_signal("tile_state_changed", tile_id, grid_pos)
		

func tile_id_get() -> int:
	return tile_id
	

func grid_pos_set(new_grid_pos : Vector2):
	grid_pos = new_grid_pos

func grid_pos_get() -> Vector2:
	return grid_pos
	
