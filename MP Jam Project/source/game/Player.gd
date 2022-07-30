class_name Player
extends Node

signal assignment_changed

enum AssignedCharacter { NONE, TRANSPORTER, BUILDER }

var player_number = 0
var assignment = 0 setget assignment_set, assignment_get
var actions

class PlayerActions:
	var player_number
	
	func _init(num):
		player_number = num
		

class Axis:
	var positive
	var negative
	

static func MoveAxis():
	var axis = Axis.new()
	axis.positive = "move_right"
	axis.negative = "move_left"
	return axis

static func AimVerticalAxis():
	var axis = Axis.new()
	axis.positive = "aim_up"
	axis.negative = "aim_down"
	return axis
func _init(player_num):
	player_number = player_num
	actions = PlayerActions.new(player_num)
	assignment = AssignedCharacter.NONE

func _ready():
	pass

func _process(delta):
	pass

func is_assigned():
	return assignment != AssignedCharacter.NONE

func has_any_inputs():
	return false

func assignment_set(new_assignment):
	if assignment != new_assignment:
		assignment = new_assignment
		print("setter called, ", assignment)
		emit_signal("assignment_changed", assignment)

func assignment_get() :
	print("getter called, ", assignment)
	return assignment
