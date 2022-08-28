extends Node2D

export var mouse_offset = Vector2(64, 64)
export var tilemap_path : NodePath
export var valid_color = Color.springgreen
export var invalid_color = Color.crimson

var tilemap : TileMap
func _ready():
	return



func _process(delta):
	if not _has_tilemap():
		return
	var mp = get_global_mouse_position()
	#mp+= mouse_offset
	var cp = tilemap.world_to_map(mp)
	position = tilemap.map_to_world(cp)
	_show_cell_status(is_cell_valid(cp, tilemap))

#should be overriden, currently empty cells are valid, non-empty cells are invalid
func is_cell_valid(cell : Vector2, tm : TileMap) -> bool:
	return tm.get_cellv(cell) == -1

func _has_tilemap():
	if tilemap == null:
		tilemap = get_node(tilemap_path)
		if tilemap == null:
			return false
	return true

func _show_cell_status(validity : bool, visibility = true):
	modulate = valid_color if validity else invalid_color
	visible = visibility
