tool
extends Node2D


var tileset :TileSet
var tilemap :TileMap 

func _ready():
	tilemap = get_parent() as TileMap
	assert(tilemap != null)
	tileset = tilemap.tile_set
	
func _process(delta):
	pass

func smooth_node(x, y, i):
	if i >= 1:
		return
	var next = i-1
	var neighbor =tilemap.get_cell(x+1, y)
	if(neighbor < i):
		print("Setting ", x, y, next)
		tilemap.set_cell(x+1, y, next)

