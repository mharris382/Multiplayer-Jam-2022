extends Area2D

signal move_to_puzzle(puzzle)

export var puzzle: Resource


func _on_LevelTransition_body_entered(body):
	if body is PlayerMovement:
		if puzzle.completed == false:
			emit_signal("move_to_puzzle", puzzle)
		else:
			print("Puzzle %s has been completed" % puzzle.puzzle_name)
