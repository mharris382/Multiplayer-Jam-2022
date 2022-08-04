extends Node

enum PuzzleState { Unsolved=0, Solved=1, In_Progress=2 }

var puzzle_state = 0

func _notification(what):
	if what == NOTIFICATION_PARENTED:
		var parent = get_parent()
		if parent.has_node("StaticMap Actual") and parent.has_node("StaticMap Solution"):
			#this puzzle was just started
			pass
		return
	pass

func _ready():
	pass # Replace with function body.

