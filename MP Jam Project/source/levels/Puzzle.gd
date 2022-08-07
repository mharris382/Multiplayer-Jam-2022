tool
class_name Puzzle
extends Node2D

export var toggle_tilemaps_in_editor = false
onready var actual_map : TileMap = $Interactives/Actual
onready var solution_map: TileMap = $Interactives/Solution
var solution_tiles: Dictionary = {}

var last_solution_vis
var last_actual_vis

func _ready():
	var test_word = "Furnace_1_On"
	print(test_word.left(test_word.length()-3))
	_initialize_scene()
	_initialise_puzzle()
	print("Puzzle completion: ", puzzle_percent())
	$Builder.connect("block_built", self, "_on_Puzzle_block_built")



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
	if positions.size() == 0: ##prevents a divide by 0 error
		print("Error: solution map is empty or cached solution has been erased")
		return
	for tile_pos in positions:
		var id = actual_map.get_cellv(tile_pos)
		if id == solution_tiles[tile_pos]:
			count += 1
	return float(count) / positions.size()


func _on_Puzzle_block_built():
	var result = puzzle_percent()
	if result == 1.0:
		print("Level is completed!")
	else:
		print("Puzzle has curently %d percent completion" % [result*100])

# makes sure correct tilemap is visible, turns on scene camera (if one exists)
func _initialize_scene():
	actual_map.visible = true
	solution_map.visible = false
	solution_map.collision_layer = 0
	solution_map.collision_mask = 0
	solution_map.occluder_light_mask
	
	var cam_node = get_node("Camera2D")
	if cam_node != null and cam_node is Camera2D:
		cam_node.current = true

func _initialise_puzzle():
	for cell in solution_map.get_used_cells():
		var id = solution_map.get_cellv(cell)
		solution_tiles[cell] = id
		
func _tool_toggle_maps_visible():
	actual_map.visible = !last_actual_vis
	solution_map.visible = !actual_map.visible
	last_actual_vis = actual_map.visible
	last_solution_vis =solution_map.visible 
