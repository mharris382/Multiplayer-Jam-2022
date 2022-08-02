class_name Player
extends Node

signal assignment_changed

signal input_move(move)
signal input_interact_just_pressed

signal input_jump_just_pressed
signal input_jump_just_released

signal input_ability_just_pressed(aim)
signal input_ability_pressed(aim)
signal input_ability_just_released(aim)

enum AssignedCharacter { NONE, TRANSPORTER, BUILDER }
const debug = false


var player_number : int = 0
var assignment = 0 setget assignment_set, assignment_get
var has_had_input : bool
var partner : int #partner role, 0=none, 1=tran, 2=build
func _init(player_num):
	player_number = player_num
	assignment = AssignedCharacter.NONE

func _ready():
	#connect all buttons to on_any_input 
	connect("input_ability_just_pressed", self, "on_any_input")
	connect("input_ability_pressed", self, "on_any_input")
	connect("input_ability_just_released", self, "on_any_input")
	connect("input_jump_just_pressed", self, "on_any_input")
	connect("input_jump_just_released", self, "on_any_input")

func _process(delta):
	var aim  = Input.get_vector("aim_right_p%d" % player_number, "aim_left_p%d" % player_number, "aim_up_p%d" % player_number, "aim_down_p%d" % player_number)	
	var move = Input.get_axis("move_left_p%d" % player_number, "move_right_p%d" % player_number)
	emit_signal("input_move", move)
	if Input.is_action_just_pressed("interact_p%d"%player_number):
		emit_signal("input_interact_pressed")
		
	if Input.is_action_just_pressed("jump_p%d"%player_number):
		emit_signal("input_jump_just_pressed")

	if Input.is_action_just_released("jump_p%d"%player_number):
		emit_signal("input_jump_just_released")
		
	if Input.is_action_just_pressed("ability_p%d"%player_number):
		emit_signal("input_ability_just_pressed", aim)
		
	elif Input.is_action_pressed("ability_p%d"%player_number):
		emit_signal("input_ability_pressed", aim)

	if Input.is_action_just_released("ability_p%d"%player_number):
		emit_signal("input_ability_just_released", aim)

#public functions

func reset_input():
	has_had_input = false

func is_assigned():
	return assignment != AssignedCharacter.NONE

func has_input():
	return has_had_input
	
#property functions


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

#helper functions

func switch_role():
	if assignment == AssignedCharacter.NONE:
		return
	assignment = partner
	emit_signal("assignment_changed", assignment)

func on_any_input():
	has_had_input = true
