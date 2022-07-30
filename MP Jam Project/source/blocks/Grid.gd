class_name Grid
extends Node2D

export var CellSize = Vector2(256,256)
export var map_size = 1024

var Cell = load("res://source/blocks/Cell.gd")
var block_lib : BlockLibrary 
var was_initialised = false
var grid = []

 #width/height of grid in godot units

onready var tile_map : GridTileMap = $DynamicGrid as GridTileMap

func _ready():
	init_block_lib()
	init_grid()
	validate_dependencies()

func init_grid():
	if was_initialised:
		return
	print("Grid - Found Block Library.")
	print("Grid - Creating Grid...")
	_init_grid_internal()
	print("Grid - Created Grid Successful.")
	
func init_block_lib():
	if block_lib == null:
		block_lib = get_node("/root/BlockLibrary")
	assert(block_lib!=null)
	
func validate_dependencies():
	assert(tile_map != null)
	assert(block_lib != null)



#---------------------------------------------------
#Public Functions

func create_new_cell(x, y) -> Cell:
	var grid_transform = Transform2D(transform)
	grid_transform.x *= CellSize
	var new_cell = Cell.new(tile_map, grid_transform, x, y)
	new_cell.connect("tile_state_changed", tile_map, "on_TileMap_tile_state_changed")
	return new_cell

func build_block_gridspace(tile_name : String,  gridspace_location : Vector2 ):
	print("Grid - Building Block (", tile_name, "), Location= (", gridspace_location, ")...")
	var cell = grid[(gridspace_location.x as int)][(gridspace_location.y as int)] as Cell
	cell.tile_name = tile_name

#---------------------------------------------------
#Private Functions

func _init_grid_internal():
	grid = []
	var size = Vector2(map_size / CellSize.x, map_size / CellSize.y)
	
	validate_dependencies()
	
	for x in size.x:
		grid.append([])
		for y in size.y:
			var new_cell = create_new_cell(x,y)
			grid[x].append(new_cell)

	was_initialised = true


#---------------------------------------------------
#Property Methods

func tile_map_set(new_tile_map):
	pass
func tile_map_get() -> GridTileMap:
	return tile_map
