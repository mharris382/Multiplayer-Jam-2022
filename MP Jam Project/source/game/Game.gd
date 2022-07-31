extends Node

enum GameState {MAIN_MENU, CHARACTER_SELECT, LEVEL_SELECT, IN_GAME}

const TRANSPORTER = 1
const BUILDER = 2
const DEVELEMENT_MODE_ON = true

var players = []

#for organization of dynamic nodes
var parent_players : Node
var parent_level_select : Node
var parent_levels : Node
var parent_ui : Node

onready var transporter_scene = preload("res://source/players/transporter/transporter.tscn")
onready var builder_scene = preload("res://source/players/builder/builder.tscn")

var transporter : CharacterBase
var builder : CharacterBase

func _ready():
	parent_level_select = create_parent("LevelSelection")
	parent_players = create_parent("Player")
	parent_levels = create_parent("Levels")
	parent_ui = create_parent("UI")
	
	players.append(Player.new(1))
	players.append(Player.new(2))

	for p in players:
		add_child_below_node(parent_players, p)

	if DEVELEMENT_MODE_ON:
		players[0].assignment = TRANSPORTER
		players[1].assignment = BUILDER

func _process(delta):
	var p1 = players[0]
	var p2 = players[1]
	
	
	if p1.is_assigned() and p2.is_assigned():
		_process_game(delta)
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


func _process_game(delta):
	for player in players:
		if player.avatar == null:
			create_avatar_for_player(player)
		player._process(delta)

func create_avatar_for_player(player : Player):
	assert(player.assignment != 0)
	if player.assignment == TRANSPORTER:
		player.avatar = transporter_scene.instance()
	else:
		player.avatar = builder_scene.instance()

func create_parent(name):
	var parent =  Node.new()
	parent.name = name
	add_child(parent)
	
func switch_player_roles():
	destroy_avatars()
	for player in players:
		player.switch_role()
		create_avatar_for_player(player)

func destroy_avatars():
	if transporter != null:
		transporter.queue_free()
	if builder != null:
		builder.queue_free()
	
