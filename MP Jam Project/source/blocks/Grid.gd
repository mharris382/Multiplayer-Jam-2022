class_name Grid
extends Node2D

export var CellSize = Vector2(256,256)

var Cell = load("res://source/blocks/Cell.gd")
var was_initialised = false
var grid = []

onready var tile_map = $"DynamicGrid"



func create_new_cell(x, y) -> Cell:
	var grid_transform = Transform2D(transform)
	grid_transform.x *= CellSize
	var new_cell = Cell.new(tile_map, grid_transform, x, y)
	new_cell.connect("tile_state_changed", tile_map, "on_TileMap_tile_state_changed")
	return new_cell
	

func _init_grid():
	grid = []
	var space = 1024
	var width =  space / CellSize.x
	var height = space / CellSize.y
	for x in width:
		grid.append([])
		for y in height:
			grid[x].append(create_new_cell(x,y))
	was_initialised = true
			
# Called when the node enters the scene tree for the first time.
func _ready():
	_init_grid()
	#var tileset = tile_map.tile_set
	#for name in Tiles:
	#	Tiles[name]=tileset.find_tile_by_name(name)
	#	assert(Tiles[name] != -1)
#		print("Name: ", name, ", ID: ", Tiles[name])
#		continue
