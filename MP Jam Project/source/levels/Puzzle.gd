extends Node2D

onready var actual_map = $Interactives/Actual
onready var solution_map = $Interactives/Solution
var solution_tiles: Dictionary = {}

func _ready():
	initialise_puzzle()
	print(puzzle_percent())
	
		
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
