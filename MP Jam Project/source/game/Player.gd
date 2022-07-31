class_name Player
extends Node

signal assignment_changed
signal input_interact_just_pressed
signal input_jump_just_pressed

enum AssignedCharacter { NONE, TRANSPORTER, BUILDER }

var player_number = 0
var assignment = 0 setget assignment_set, assignment_get
var partner
var has_had_input
var avatar

func _init(player_num):
	player_number = player_num
	assignment = AssignedCharacter.NONE

func _ready():
	pass

func _process(delta):
	var aim  = Input.get_vector("aim_right_p%d" % player_number, "aim_left_p%d" % player_number, "aim_up_p%d" % player_number, "aim_down_p%d" % player_number)	
	var move = Input.get_axis("move_left_p%d" % player_number, "move_right_p%d" % player_number)
	
	if Input.is_action_just_pressed("interact_p%d"%player_number):
		emit_signal("input_interact_pressed")
		has_had_input = true
	if Input.is_action_just_pressed("jump_p%d"%player_number):
		has_had_input = true

func reset_input():
	has_had_input = false

func is_assigned():
	return assignment != AssignedCharacter.NONE

func has_input():
	return has_had_input

func assignment_set(new_assignment):
	if assignment != new_assignment:
		assignment = new_assignment
		if assignment != AssignedCharacter.NONE:
			if assignment == AssignedCharacter.TRANSPORTER:
				partner = AssignedCharacter.BUILDER
			else:
				partner = AssignedCharacter.TRANSPORTER
		print("setter called, ", assignment)
		emit_signal("assignment_changed", assignment)

func assignment_get() :
	print("getter called, ", assignment)
	return assignment

func switch_role():
	if assignment == AssignedCharacter.NONE:
		return
	assignment = partner
	emit_signal("assignment_changed", assignment)
