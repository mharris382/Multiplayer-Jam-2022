class_name CharacterBase
extends PlayerMovement
#also emits null when character is unassigned
signal player_assigned_to_character(player)
signal character_changed_direction(direction) #LEFT = -1, #RIGHT = 1
signal character_state_changed(prev_state, next_state, move_input)


signal block_pickup_occured(picked_up_block_name)



enum PlayerState{
	IDLE=1,
	RUNNING=2,
	IN_AIR=3
}

enum AimDirection { FRONT, BELOW, ABOVE, RIGHT, LEFT }
enum FacingDirection { LEFT=-1, RIGHT=1 }

var facing_direction = FacingDirection.RIGHT
var aim_input : Vector2
var found_player = false
var last_player_state = 1
var last_direction = 1

onready var front_aim_point = $"ControlPoints/Front"
onready var below_aim_point = $"ControlPoints/Below"
onready var above_aim_point = $"ControlPoints/Above"
onready var right_aim_point = $"ControlPoints/Right"
onready var left_aim_point = $"ControlPoints/Left"


func _ready(): 
	._ready()
	
	
func _process(delta):
	var current_state= last_player_state
	if not is_on_floor():
		current_state = PlayerState.IN_AIR
	elif move_input.length() > 0.1:
		current_state = PlayerState.RUNNING
	else:
		current_state = PlayerState.IDLE
	if last_player_state != current_state:
		var _mv_input = move_input
		if is_jumping:
			_mv_input.y = 1
		else:
			_mv_input.y = -1
		emit_signal("character_state_changed", last_player_state, current_state, move_input)
		last_player_state = current_state

	if move_input.length_squared() > 0:
		var direction = last_direction
		if move_input.x > 0 and direction < 0:
			direction = 1
			emit_signal("character_changed_direction", direction)
			last_direction = direction
		elif move_input.x < 0 and direction > 0:
			direction = -1
			emit_signal("character_changed_direction", direction)
			last_direction = direction


func _get_player_number():
	return 1

func assign_player(player):
	Players.assign_avatar_to_player(self, _get_player_number())
	emit_signal("player_assigned_to_character", player)

func unassign_player(player):
	Players.assign_avatar_to_player(self, _get_player_number())
	emit_signal("player_assigned_to_character", null)

func try_interact():
	pass

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
	#print("CharacterBase.%s just pressed ability" % name)
	pass

func on_player_pressed_ability(aim):
	#print("CharacterBase.%s pressed ability" % name)
	pass

func on_player_released_ability(aim):
	#print("CharacterBase.%s released ability" % name)
	pass
	
func on_player_just_pressed_interact():
	#print("CharacterBase. %s pressed interact" % name)
	set_collision_layer_bit(5, true)
	
func on_player_released_interact():
	#print("CharacterBase.%s released interact" % name)
	set_collision_layer_bit(5, false)

