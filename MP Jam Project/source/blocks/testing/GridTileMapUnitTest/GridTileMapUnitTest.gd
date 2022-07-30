extends Node


# Declare member variables here. Examples:
# var a = 2
# var b = "text"

onready var grid_tile_map = $"GridTileMap"
var block_id
var block_name = "block_base"
# Called when the node enters the scene tree for the first time.
func _ready():
	block_id = grid_tile_map.tile_set.find_tile_by_name(block_name)
	
