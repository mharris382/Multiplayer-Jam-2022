class_name CharacterBase
extends PlayerMovement


enum AimDirection { FRONT, BELOW, ABOVE }

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

var found_player = false
func _ready(): 
	._ready()
func _process(delta):
	if found_player:
		return
	if Players.players[0] != null:
		found_player = true
		Players.players[0].connect("input_move", self, "on_player_move_input")
		
		Players.players[0].connect("input_ability_just_pressed", self, "on_player_just_pressed_ability")
		Players.players[0].connect("input_ability_pressed", self, "on_player_pressed_ability")
		Players.players[0].connect("input_ability_just_released", self,"on_player_released_ability")
		
		Players.players[0].connect("input_jump_just_pressed", self,"on_player_pressed_jump")
		Players.players[0].connect("input_jump_just_released", self,"on_player_released_jump")
		
		Players.players[0].connect("input_interact_just_pressed", self, "on_player_pressed_interact")
		
#since the validity is dependent on the kind of action being performed this 
#function must be implemented in the child classes
func is_direction_valid(aim_direction) -> bool:
	return false

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
	pass

func on_player_pressed_ability(aim):
	pass

func on_player_released_ability(aim):
	pass
	
func on_player_pressed_interact():
	pass
	
func on_player_released_interact():
	pass



func get_aim_position(aim_direction):
	match(aim_direction):
		AimDirection.FRONT:
			return front_aim_point.position
		AimDirection.BELOW:
			return below_aim_point.position
		AimDirection.ABOVE:
			return above_aim_point.position
