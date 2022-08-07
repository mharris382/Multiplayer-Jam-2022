class_name Player
extends Node

signal assignment_changed(player, role)# 1=TRANSPORTER, 2=BUILDER

signal input_aim(aim, use_mouse)
signal input_move(move)
signal input_interact_just_pressed()

signal input_jump_just_pressed
signal input_jump_just_released

signal input_ability_just_pressed(aim)
signal input_ability_pressed(aim)
signal input_ability_just_released(aim)

signal input_ability_aim(aim_dir)
signal input_aim_changed(is_aiming)

enum AssignedCharacter { NONE, TRANSPORTER, BUILDER }

export var use_mouse_for_aim = false

const INTERACT_BUTTON = "interact_p%d"
const JUMP_BUTTON = "jump_p%d"
const USE_ABILITY_BUTTON = "ability_p%d"
const AIM_ABILITY_BUTTON = "aim_ability_p%d"

const debug = false


var player_number : int = 0
var assignment = 0 setget assignment_set, assignment_get
var has_had_input : bool
var partner : int #partner role, 0=none, 1=tran, 2=build


var currently_aiming = false
var ability_pressed = false
var last_non_zero_aim : Vector2

func _init(player_num):
	player_number = player_num
	name = "player_%d" % player_num
	assignment = AssignedCharacter.NONE



func _ready():
	#connect all buttons to on_any_input 
	connect("input_ability_just_pressed", self, "on_any_input")
	connect("input_ability_pressed", self, "on_any_input")
	connect("input_ability_just_released", self, "on_any_input")
	connect("input_jump_just_pressed", self, "on_any_input")
	connect("input_jump_just_released", self, "on_any_input")

func _process(delta):
	var move = Input.get_axis("move_left_p%d" % player_number, "move_right_p%d" % player_number)
	emit_signal("input_move", move)
	

	
	
	
	if Input.is_action_just_pressed(INTERACT_BUTTON%player_number):
		emit_signal("input_interact_just_pressed")
		
	if Input.is_action_just_pressed(JUMP_BUTTON%player_number):
		emit_signal("input_jump_just_pressed")

	if Input.is_action_just_released(JUMP_BUTTON%player_number):
		emit_signal("input_jump_just_released")
		
#	if Input.is_action_just_pressed(USE_ABILITY_BUTTON%player_number):
#		emit_signal("input_ability_just_pressed", aim)
#
#	if Input.is_action_pressed(USE_ABILITY_BUTTON%player_number):
#		emit_signal("input_ability_pressed", aim)
#
#	if Input.is_action_just_released(USE_ABILITY_BUTTON%player_number):
#		emit_signal("input_ability_just_released", aim)

func _process_aim():
	
	#aim axis input
	var aim :Vector2= _get_aim_input()
	
	if !currently_aiming:#start aiming from either pressing aim button or from aim input axis
		if aim.length_squared() != 0 or Input.is_action_just_pressed(AIM_ABILITY_BUTTON % player_number):
			_start_aiming_internal()
		else: #not aiming don't do anything else 
			return
	
	#make sure we only pass non-zero aim vector
	if aim.length_squared() == 0:
		aim = last_non_zero_aim
	else:
		last_non_zero_aim = aim
	
	#at this point we know we are aiming, we would like to figure out 
	if Input.is_action_just_released(AIM_ABILITY_BUTTON % player_number):
		_stop_aiming_internal()
	else:
		_aiming_internal(aim)
	

func _aiming_internal(aim):
	if aim.length_squared() == 0:
		aim = last_non_zero_aim
	else:
		last_non_zero_aim = aim
	emit_signal("input_ability_aim")

func _start_aiming_internal():
	currently_aiming = true
	emit_signal("input_aim_changed", true)

func _stop_aiming_internal():
	currently_aiming = false
	emit_signal("input_aim_changed", true)
	last_non_zero_aim = Vector2.RIGHT #use right as the default (don't set this to zero)

#public functions
func _get_aim_input():
	return Input.get_vector("aim_right_p%d" % player_number, "aim_left_p%d" % player_number, "aim_up_p%d" % player_number, "aim_down_p%d" % player_number)	
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
				print("Player%d assigned to transporter" % player_number)
				name = "player_%d (T)" % player_number
				partner = AssignedCharacter.BUILDER
			else:
				print("Player%d assigned to builder" % player_number)
				name = "player_%d (B)" % player_number
				partner = AssignedCharacter.TRANSPORTER
		emit_signal("assignment_changed", self, assignment)

func assignment_get() :
	return assignment

#helper functions

func switch_role():
	if assignment == AssignedCharacter.NONE:
		return
	assignment = partner
	emit_signal("assignment_changed", assignment)

func on_any_input(arg = null):
	has_had_input = true

func on_other_player_assigned(player, role):
	assignment = player.partner
