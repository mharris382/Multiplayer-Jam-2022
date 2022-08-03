extends Node

signal game_state_changed(prev_state, new_state)
signal players_assigned(transporter_player, builder_player)


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
	
func _init():
	players.append(Player.new(1))
	players.append(Player.new(2))
	for p in players:
		add_child(p)
	
func _ready():
	var tree = get_tree()
	assert(tree != null)
	assert(tree.root != null)
	#assert(parent_players != null)
	#for p in players:
	#	add_child_below_node(parent_players, p)
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


