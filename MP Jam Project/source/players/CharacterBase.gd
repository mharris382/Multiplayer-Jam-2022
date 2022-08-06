class_name CharacterBase
extends PlayerMovement

signal player_assigned_to_character(player)
signal character_changed_direction(direction) #LEFT = -1, #RIGHT = 1
signal character_state_changed(prev_state, next_state, move_input)
enum PlayerState{
	IDLE=1,
	RUNNING=2,
	IN_AIR=3
}

enum AimDirection { FRONT, BELOW, ABOVE }
enum FacingDirection { LEFT=-1, RIGHT=1 }

var facing_direction = FacingDirection.RIGHT
var aim_input : Vector2
var found_player = false
var last_player_state = 1
var last_direction = 1

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
	set_collision_layer_bit(5, true)
	pass
	
func on_player_released_interact():
	print("%s released interact" % name)
	set_collision_layer_bit(5, false)
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
