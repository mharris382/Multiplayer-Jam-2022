class_name Player
extends Node

signal assignment_changed(player, role)# 1=TRANSPORTER, 2=BUILDER

#note While I am leaving these signals here, I am going to call functions directly on the avatar as well (just trying it out)
signal input_aim(aim, use_mouse)
signal input_move(move)
signal input_interact_just_pressed()

signal input_jump_just_pressed
signal input_jump_just_released

signal input_ability_just_pressed(aim)
signal input_ability_pressed(aim)
signal input_ability_just_released(aim)




export var use_mouse_for_aim = false


const debug = false



#these are good
var player_number : int = 0
var has_had_input : bool
var avatar


func _init(player_num):
	player_number = player_num
	name = "player_%d" % player_num
	assignment = AssignedCharacter.NONE


func _ready():
	_listen_for_any_input()


func _process(delta):
	process_move_input()
	process_jump_inputs()
	process_all_ability_inputs()
	process_interact()
	

func process_move_input():
	var move_x = Input.get_axis("move_left_p%d" % player_number, "move_right_p%d" % player_number)
	emit_signal("input_move", move_x)
	avatar.move_input = Vector2(move_x, 0)


func process_jump_inputs():
	if Input.is_action_just_pressed("jump_p%d"%player_number):
		emit_signal("input_jump_just_pressed")
		avatar.is_jumping = true
	
	if Input.is_action_just_released("jump_p%d"%player_number):
		emit_signal("input_jump_just_released")
		avatar.is_jumping = false


func process_interact():
	if Input.is_action_just_pressed("interact_p%d"%player_number):
		emit_signal("input_interact_just_pressed")


func process_all_ability_inputs():
	var aim = ability_process_aim_input()
	avatar.aim_input = aim
	if Input.is_action_just_pressed("ability_p%d"%player_number):
		emit_signal("input_ability_just_pressed", aim)
		
	if Input.is_action_pressed("ability_p%d"%player_number):
		emit_signal("input_ability_pressed", aim)

	if Input.is_action_just_released("ability_p%d"%player_number):
		emit_signal("input_ability_just_released", aim)


func ability_process_aim_input():
	var aim = Vector2.ZERO
	aim  = Input.get_vector("aim_right_p%d" % player_number, "aim_left_p%d" % player_number, "aim_up_p%d" % player_number, "aim_down_p%d" % player_number)	
	emit_signal("input_aim", aim, use_mouse_for_aim)
	avatar.aim_input = aim
	return aim



#---------------------------------------------
#IGNORE STUFF BELOW

enum AssignedCharacter { NONE, TRANSPORTER, BUILDER }

#these are bad
var assignment = 0 setget assignment_set, assignment_get
var partner : int #partner role, 0=none, 1=tran, 2=build
var ability_pressed = false


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

func reset_input():
	has_had_input = false

func is_assigned():
	return avatar != null

func has_input():
	return has_had_input
	
func _listen_for_any_input():
	#connect all buttons to on_any_input 
	connect("input_ability_just_pressed", self, "on_any_input")
	connect("input_ability_pressed", self, "on_any_input")
	connect("input_ability_just_released", self, "on_any_input")
	connect("input_jump_just_pressed", self, "on_any_input")
	connect("input_jump_just_released", self, "on_any_input")
	
