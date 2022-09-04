tool
extends Node2D

export var show_solution_map = false setget set_show_map

func set_show_map(show_map):
	show_solution_map = show_map
	#get_node("StaticMap Actual").visible = !show_map
	#get_node("StaticMap Solution").visible = show_map

#onready var initial_map : TileMap = $"StaticMap Actual"
#onready var solution_map : TileMap = $"StaticMap Solution"

onready var cells : Node2D = $"StaticMap/Cells"
