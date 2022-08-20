class_name Level
extends Node


signal on_level_loaded(level, lvl_scn)
signal on_level_unloaded(level)
signal on_level_reset(level)
signal on_level_complete(level)


var transporter : Transporter
var builder : Builder
var is_completed = false
export var lobby_reference: PackedScene


func _ready():
	var doors = $Lobby/Doors
	for door in doors.get_children():
		door.connect("move_to_puzzle", self, "on_Level_move_to_puzzle")

# PROPERTIES
func on_Level_move_to_puzzle(puzzle):
	var new_level = puzzle.puzzle_scene.instance()
	$Lobby.queue_free()
	call_deferred("add_child", new_level)
	

func is_completed_get():
	return is_completed
	
func is_completed_set(is_level_completed):
	if is_completed != is_level_completed:
		if is_level_completed:
			emit_signal("on_level_complete", self)

func is_loaded_get():
	return
	
func is_loaded_set():
	return

