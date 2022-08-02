extends Node

signal game_state_changed(prev_state, new_state)

enum GameState {MAIN_MENU, CHARACTER_SELECT, IN_GAME}


const TRANSPORTER = 1
const BUILDER = 2
const DEVELEMENT_MODE_ON = true

var players = []
var game_state setget game_state_set, game_state_get

#for organization of dynamic nodes
var parent_players : Node
var parent_level_select : Node
var parent_levels : Node
var parent_ui : Node

func _ready():
	print("game ready")
	players.append(Player.new(1))
	players.append(Player.new(2))

	for p in players:
		add_child_below_node(parent_players, p)

	if DEVELEMENT_MODE_ON:
		players[0].assignment = TRANSPORTER
		players[1].assignment = BUILDER
		game_state = GameState.IN_GAME
		#TODO: load into a lobby scene
	else:
		game_state = GameState.MAIN_MENU
		#TODO: load into main menu
	print("loaded Players.gd")

func _process(delta):
	var p1 = players[0]
	var p2 = players[1]
	match game_state:
		GameState.MAIN_MENU, GameState.CHARACTER_SELECT:
			if p1.is_assigned() and p2.is_assigned():
				game_state = GameState.IN_GAME
				#print("process game")
			elif !p1.is_assigned() and !p2.is_assigned():
				p1._process(delta)
				p2._process(delta)
				
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
			else:
				for player in players:
					player._process(delta)

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
