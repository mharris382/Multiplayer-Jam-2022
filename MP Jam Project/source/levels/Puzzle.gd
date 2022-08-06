extends Node2D

onready var actual_map : TileMap = $Interactives/Actual
onready var solution_map: TileMap = $Interactives/Solution
var solution_tiles: Dictionary = {}

func _ready():
	initialize_scene()
	initialise_puzzle()
	print(puzzle_percent())


# makes sure correct tilemap is visible, turns on scene camera (if one exists)
func initialize_scene():
	actual_map.visible = true
	solution_map.visible = false
	solution_map.collision_layer = 0
	solution_map.collision_mask = 0
	solution_map.occluder_light_mask
	
	var cam_node = get_node("Camera2D")
	if cam_node != null and cam_node is Camera2D:
		cam_node.current = true

func initialise_puzzle():
	for cell in solution_map.get_used_cells():
		var id = solution_map.get_cellv(cell)
		solution_tiles[cell] = id

func is_puzzle_correct() -> bool:
	var positions = solution_tiles.keys()
	for tile_pos in positions:
		var id = actual_map.get_cellv(tile_pos)
		if id != solution_tiles[tile_pos]:
			return false
	return true
	
func puzzle_percent():
	var positions = solution_tiles.keys()
	var count = 0
	for tile_pos in positions:
		var id = actual_map.get_cellv(tile_pos)
		if id == solution_tiles[tile_pos]:
			count += 1
	return float(count) / positions.size()



