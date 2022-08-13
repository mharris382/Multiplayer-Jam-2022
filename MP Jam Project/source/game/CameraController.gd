extends Node

var player_position_map = {}
var player_path_map = {}

var player_avatars = {}
var avatars = [null, null]
onready var camera = $Camera2D

func _init():
	Players.connect("avatar_was_assigned", self, "_on_avatar_assigned_to_player")
	Players.connect("avatar_was_unassigned", self, "_on_avatar_unassigned_to_player")

func _ready():
	pass

func _process(delta):
	var current_camera_position = camera.position
	camera.current = true
	var players_to_track = player_avatars.keys()
	var target_position = Vector2.ZERO #initialize to zero
	var count = 0
	for avatar in avatars:
		assert(avatar != null)
		if avatar != null:
			count += 1
			target_position += avatar.global_position
	
	if count == 0:
		camera.position = Vector2.ZERO
		return
	
	#this is a simple average, can be turned into a weighted average with some modifications
	target_position = Vector2(target_position.x / float(count), target_position.y / float(count))
	
	camera.position = target_position

func _on_avatar_assigned_to_player(player, avatar):
	assert(player != null)
	assert(avatar != null)
	avatars[player.player_number-1] = avatar
	print("P%d avatar assigned "%player.player_number, avatar)
	assert(avatars[player.player_number-1] != null)
	
func _on_avatar_unassigned_to_player(player, avatar):
	if player_position_map.has(player) and player_position_map[player] != null:
		player_position_map[player].remote_path = null
