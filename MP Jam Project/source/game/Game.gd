extends Node

var players = []

var parent_players : Node
var parent_level_select : Node
var parent_levels : Node
var parent_ui : Node

func _ready():
	parent_level_select = create_parent("LevelSelection")
	parent_players = create_parent("Player")
	parent_levels = create_parent("Levels")
	parent_ui = create_parent("UI")


func create_parent(name):
	var parent =  Node.new()
	parent.name = name
	add_child(parent)
	
	
func create_player(player_number):
	var new_player = Player.new(player_number)
	pass
