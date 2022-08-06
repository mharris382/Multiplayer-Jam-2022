class_name CharacterBase
extends PlayerMovement

signal player_assigned_to_character(player)
signal character_changed_direction(direction) #LEFT = -1, #RIGHT = 1

enum AimDirection { FRONT, BELOW, ABOVE }
enum FacingDirection { LEFT=-1, RIGHT=1 }

var facing_direction = FacingDirection.RIGHT
var aim_input : Vector2

onready var front_aim_point = $"ControlPoints/Front"
onready var below_aim_point = $"ControlPoints/Below"
onready var above_aim_point = $"ControlPoints/Above"


func _notification(what):
	if what == NOTIFICATION_UNPARENTED:
		print("Unparented ", name)
	elif what == NOTIFICATION_PARENTED:
		print("Parented ", name, "to ", get_parent().name)
		var player = get_parent() as Player
		if player == null:
			return
			

func assign_player(player):
	player.connect("input_move", self, "on_player_move_input")
	player.connect("input_ability_just_pressed", self, "on_player_just_pressed_ability")
	player.connect("input_ability_pressed", self, "on_player_pressed_ability")
	player.connect("input_ability_just_released", self,"on_player_released_ability")
	player.connect("input_jump_just_pressed", self,"on_player_pressed_jump")
	player.connect("input_jump_just_released", self,"on_player_released_jump")
	player.connect("input_interact_just_pressed", self, "on_player_just_pressed_interact")
	emit_signal("player_assigned_to_character", player)

func unassign_player(player):
	player.disconnect("input_move", self, "on_player_move_input")
	player.disconnect("input_ability_just_pressed", self, "on_player_just_pressed_ability")
	player.disconnect("input_ability_pressed", self, "on_player_pressed_ability")
	player.disconnect("input_ability_just_released", self,"on_player_released_ability")
	player.disconnect("input_jump_just_pressed", self,"on_player_pressed_jump")
	player.disconnect("input_jump_just_released", self,"on_player_released_jump")
	player.disconnect("input_interact_just_pressed", self, "on_player_just_pressed_interact")
	emit_signal("player_assigned_to_character", null)

var found_player = false
func _ready(): 
	._ready()

#since the validity is dependent on the kind of action being performed this 
#function must be implemented in the child classes
func is_direction_valid(aim_direction) -> bool:
	return false
func on_player_aim_input(aim_input : Vector2):
	self.aim_input = aim_input
func on_player_move_input(move_input : float):
	self.move_input = Vector2(move_input, 0)

func on_player_pressed_jump():
	self.jump_pressed = true
	self.jump_just_pressed = true
	pass

func on_player_released_jump():
	self.jump_pressed = false
	pass

func on_player_just_pressed_ability(aim):
	print("%s just pressed ability" % name)
	pass

func on_player_pressed_ability(aim):
	print("%s pressed ability" % name)
	pass

func on_player_released_ability(aim):
	print("%s released ability" % name)
	pass
	
func on_player_just_pressed_interact():
	print("%s pressed interact" % name)
	pass
	
func on_player_released_interact():
	print("%s released interact" % name)
	pass


# making this function until we make use of aim parameter from ability
func get_aim_direction(value):
	if value == "left" or value == "right":
		return front_aim_point


func get_aim_position(aim_direction):
	match(aim_direction):
		AimDirection.FRONT:
			return front_aim_point.position
		AimDirection.BELOW:
			return below_aim_point.position
		AimDirection.ABOVE:
			return above_aim_point.position
