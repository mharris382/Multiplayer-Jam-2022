class_name Level
extends Node


signal on_level_loaded(level, lvl_scn)
signal on_level_unloaded(level)
signal on_level_reset(level)
signal on_level_complete(level)


var transporter : Transporter
var builder : Builder
var is_completed = false
var current_puzzle
export var lobby_reference: PackedScene


func _ready():
	prepare_puzzles()

# PROPERTIES
func on_Level_move_to_puzzle(puzzle):
	current_puzzle = puzzle
	var new_level = puzzle.puzzle_scene.instance()
	unload_puzzles()
	$Lobby.queue_free()
	call_deferred("add_child", new_level)
	new_level.call_deferred("connect", "puzzle_completed", self, "on_Level_puzzle_completed")
	

func on_Level_puzzle_completed(puzzle):
	puzzle.disconnect("puzzle_completed", self, "on_Level_puzzle_completed")
	current_puzzle.completed = true
	get_child(0).queue_free()
	var lobby = lobby_reference.instance()
	call_deferred("add_child", lobby)
	call_deferred("prepare_puzzles")
	

func prepare_puzzles():
	var doors = $Lobby/Doors
	for door in doors.get_children():
		door.connect("move_to_puzzle", self, "on_Level_move_to_puzzle")
		
func unload_puzzles():
	var doors = $Lobby/Doors
	for door in doors.get_children():
		door.disconnect("move_to_puzzle", self, "on_Level_move_to_puzzle")
	
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

