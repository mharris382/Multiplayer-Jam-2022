extends Node

signal game_state_changed(prev_state, new_state)
signal players_assigned(transporter_player, builder_player)

signal avatar_was_assigned(player, avatar)
signal avatar_was_unassigned(player, avatar)

enum GameState {MAIN_MENU, CHARACTER_SELECT, IN_GAME}


const TRANSPORTER = 1
const BUILDER = 2
const DEVELEMENT_MODE_ON = true

var players = []
var game_state setget game_state_set, game_state_get

func _TESTS():
	var p1 = get_player_1()
	assert(players[0] == p1)
	var p2 = get_player_2()
	assert(players[1] == p2)
	assert(_get_other_player(p1)==p2)
	assert(_get_other_player(p2)==p1)
	

func _ready():
	if Players.players.size() == 2:
		queue_free()

	var p1_arr = get_tree().get_nodes_in_group("Player1")
	if p1_arr.size() > 0:
		Players.players.append(p1_arr[0])
	else:
		Players.players.append(Player.new(1))
		
	var p2_arr = get_tree().get_nodes_in_group("Player2")
	if p2_arr.size() > 0:
		Players.players.append(p2_arr[0])
	else:
		Players.players.append(Player.new(2))
	
	for p in players:
		add_child(p)
	
	var tree = get_tree()
	assert(tree != null)
	assert(tree.root != null)
	#assert(parent_players != null)
	#for p in players:
	#	add_child_below_node(parent_players, p)
#	if DEVELEMENT_MODE_ON:
#		players[0].assignment = TRANSPORTER
#		players[1].assignment = BUILDER
#		game_state = GameState.IN_GAME
#		#TODO: load into a lobby scene
#	else:
#		game_state = GameState.MAIN_MENU
		#TODO: load into main menu
	print("loaded Players.gd")

func _process(delta):
	if Players.players.size() != 2:
		print("Extra Game.gd")
		return
	var p1 = players[0]
	var p2 = players[1]
	match game_state:
		GameState.MAIN_MENU, GameState.CHARACTER_SELECT:
			if p1.is_assigned() and p2.is_assigned():
				game_state = GameState.IN_GAME
				#print("process game")
			elif !p1.is_assigned() and !p2.is_assigned():
				#p1._process(delta)
				#p2._process(delta)
				
				# the first player to press a button will be transporter (for now)
				if p1.has_input():
					p1.assignment = TRANSPORTER
					p2.assignment = BUILDER
				if p2.has_input():
					p2.assignment = TRANSPORTER
					p2.assignment = BUILDER
				print("p1 has input")
			elif p1.is_assigned():
				p2.assignment = p1.partner
			else: # p2.is_assigned()
				p1.assignment = p2.partner
		GameState.IN_GAME:
			if !p1.is_assigned() or !p2.is_assigned():
				game_state = GameState.CHARACTER_SELECT
			#else:
				#for player in players:
					#player._process(delta)

func create_parent(name):
	var parent =  Node.new()
	parent.name = name
	add_child(parent)

func switch_player_roles():
	for player in players:
		player.switch_role()

func game_state_get():
	return game_state

func game_state_set(new_state):
	if new_state != game_state:
		var prev_state = game_state
		game_state = new_state
		emit_signal("game_state_changed", prev_state, game_state)

static func get_player_1() -> Player:
	return Players.players[0] as Player

static func get_player_2() -> Player:
	return Players.players[1] as Player

func _get_other_player(player):
	assert(player != null)
	var p1 = get_player_1()
	var p2 = get_player_2()
	match player.player_number:
		1:
			return p2
		2:
			return p1
	return null


func assign_avatar_to_player(avatar, player_number):
	var player = players[player_number]
	_assign_player(player, avatar)
	
	
func _assign_player(player : Node, avatar : Node2D):
	print("assigning player ", player, " to ", avatar)
	player.avatar = avatar
	player.connect("input_move", avatar, "on_player_move_input")
	player.connect("input_ability_just_pressed", avatar, "on_player_just_pressed_ability")
	player.connect("input_ability_pressed", avatar, "on_player_pressed_ability")
	player.connect("input_ability_just_released", avatar,"on_player_released_ability")
	player.connect("input_jump_just_pressed", avatar,"on_player_pressed_jump")
	player.connect("input_jump_just_released", avatar,"on_player_released_jump")
	player.connect("input_interact_just_pressed", avatar, "on_player_just_pressed_interact")
	emit_signal("player_assigned_to_character", player)
	emit_signal("avatar_was_assigned", player, avatar)

func _unassign_player(player, avatar):
	player.avatar = null
	player.disconnect("input_move", avatar, "on_player_move_input")
	player.disconnect("input_ability_just_pressed", avatar, "on_player_just_pressed_ability")
	player.disconnect("input_ability_pressed", avatar, "on_player_pressed_ability")
	player.disconnect("input_ability_just_released", avatar,"on_player_released_ability")
	player.disconnect("input_jump_just_pressed", avatar,"on_player_pressed_jump")
	player.disconnect("input_jump_just_released", avatar,"on_player_released_jump")
	player.disconnect("input_interact_just_pressed", avatar, "on_player_just_pressed_interact")
	
	emit_signal("player_assigned_to_character", null)
	emit_signal("avatar_was_unassigned", player, avatar)
