extends Node


const character_group = "Characters"


var transporter
var builder

var wait_for_players_ready_timer  : Timer

func _ready():
	return
	var  tree = get_tree()
	var characters = tree.get_nodes_in_group(character_group)
	if !characters.size() >= 2:
		return
	for character in characters:
		if character is Transporter:
			transporter = (character as Transporter)
		elif character is Builder:
			builder = (character as Builder)
	assert(transporter != null)
	assert(builder != null)
	wait_for_players_ready_timer = Timer.new()
	wait_for_players_ready_timer.wait_time = 0.1
	wait_for_players_ready_timer.connect("timeout", self, "_timeout")
	wait_for_players_ready_timer.start()

func transporter_set(transporter_character):
	transporter = transporter_character
	
func builder_set(builder_character):
	builder = builder_character

func _timeout():
	pass
