class_name SteamTilemap
extends TileMap

const MAX_STEAM_VALUE = 16
const MIN_STEAM_VALUE = 0


const DIRECTIONS_4 = [
	Vector2.RIGHT,
	Vector2.LEFT,
	Vector2.UP,
	Vector2.DOWN]

func get_steam(x : int, y:int) -> int:
	return get_cell(x, y)+1
	
func get_steamv(position : Vector2) -> int:
	return get_cell(position.x, position.y)



func clear_steam(x, y) -> void:
	set_steam(x, y, 0)
	
func clear_steamv(position : Vector2) -> void:
	clear_steam(position.x, position.y)


func set_steam(x, y, steam_value) -> bool:
	steam_value = clamp(steam_value, 0, MAX_STEAM_VALUE)
	var current_steam =get_steam(x,y)
	if steam_value != current_steam:
		set_cell(x, y, steam_value-1)
		return true
	return false

func set_steamv(grid_postiion : Vector2, steam_value : int) -> bool:
	return set_steam(grid_postiion.x, grid_postiion.y, steam_value)

#returns remainder of steam
func modify_steam(grid_postiion : Vector2, steam_amount : int) -> int:
	var current_steam =get_steam(grid_postiion.x, grid_postiion.y)
	var overflow = (current_steam + steam_amount) / MAX_STEAM_VALUE
	if not set_steam(grid_postiion.x, grid_postiion.y, current_steam + steam_amount):
		return steam_amount #return entire amount if set returns false 
	return overflow

func get_neighbors(grid_position, block_tilemap):
	var arr = []
	for dir in DIRECTIONS_4:
		var pos = grid_position+dir
		if block_tilemap.get_cellv(pos) == -1:
			arr.append(pos)
	return arr


func get_lower_neighbors(grid_position:Vector2):
	var arr = []
	var amount = get_steamv(grid_position)
	for dir in DIRECTIONS_4:
		var pos = grid_position+dir
		var neighbor_amount = get_steamv(pos)
		if neighbor_amount < amount:
			arr.append(pos)
	return arr

