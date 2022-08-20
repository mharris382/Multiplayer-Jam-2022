extends Node
#NOTE: this script was originally meant to allow more than 2 avatars but I simplified it, as it is currently it will only function with 0 avatars or 2 avatars
var avatars = [null, null]
var target_zoom = Vector2(1,1)

export var avatar_padding = Vector2(25, 25)
export var prefer_zoom= Vector2(1,1)
export var limit_zoom = Vector2(4, 4)

onready var camera = $Camera2D
onready var builder_transform = $"Builder"
onready var transporter_transform = $"Transporter"

#func _init():
#	Players.connect("avatar_was_assigned", self, "_on_avatar_assigned_to_player")
#	Players.connect("avatar_was_unassigned", self, "_on_avatar_unassigned_to_player")
#
func _ready():
	camera.current = true
	target_zoom = camera.zoom
	avatars[0] = builder_transform
	avatars[1] = transporter_transform

func _process(delta):
	var current_camera_position = camera.position
	camera.current = true
	
	var target_location = _get_target_location()
	var target_zoom = _get_target_zoom()
	
	camera.position = target_location
	camera.zoom = target_zoom

#-----------------------------------------------
#helpers

func _get_avatar_position(avatar_index :int):
	if avatar_index > avatars.size():
		return camera.position
	var avatar = avatars[avatar_index] as Node2D
	if avatar == null:
		return camera.position
	return avatar.position

func _get_target_location():
	var target_position = _calculate_target_position()
	if target_position == null: #edge case: null if avatar count = 0
		return camera.position
	return target_position

func _get_required_size():#to ensure all player avatars are visible
	var p1 = _get_avatar_position(0) as Vector2
	var p2 = _get_avatar_position(1) as Vector2
	
	var avatar_min_pos = Vector2(min(p1.x, p2.x),  min(p1.y, p2.y))
	var avatar_max_pos = Vector2(max(p1.x, p2.x),  max(p1.y, p2.y))
	var dif = avatar_max_pos - avatar_min_pos
	dif += avatar_padding
	return dif

func _get_target_zoom():
	assert(limit_zoom.x >= prefer_zoom.x)
	assert(limit_zoom.y >= prefer_zoom.y)
	
	var viewport_size = get_viewport().size
	
	var p1 = _get_avatar_position(0) as Vector2
	var p2 = _get_avatar_position(1) as Vector2
	
	var avatar_min_pos = Vector2(min(p1.x, p2.x),  min(p1.y, p2.y))
	var avatar_max_pos = Vector2(max(p1.x, p2.x),  max(p1.y, p2.y)) 
	
	
	var required_size = _get_required_size() #required size to ensure all player avatars are visible
	var current_size  = viewport_size * camera.zoom #current size of viewport in worldspace (after considering zoom)
	var prefer_size = viewport_size * prefer_zoom #prefered size is the most zoomed in allowed
	var limit_size = viewport_size *  limit_zoom #limit size is the most zoomed out allowed
	
	var target_size = Vector2(max(required_size.x, current_size.x), max(required_size.y, current_size.y))
	
	target_size.x = clamp(target_size.x, prefer_size.x, limit_size.x )
	target_size.y = clamp(target_size.y, prefer_size.y, limit_size.y )
	
	var target_zoom = target_size/viewport_size
	var zoom_max = max(target_zoom.x, target_zoom.y)
	return Vector2(zoom_max, zoom_max)

func _calculate_target_position():
	var target_position = Vector2.ZERO #initialize to zero
	var count = 0
	
	for avatar in avatars:
		assert(avatar != null)
		if avatar != null:
			count += 1
			target_position += avatar.global_position
	
	if count == 0:
		camera.position = Vector2.ZERO
		return null
	
	#this is a simple average, can be turned into a weighted average with some modifications
	return Vector2(target_position.x / float(count), target_position.y / float(count))

#-----------------------------------------------
#signals 

#func _on_avatar_assigned_to_player(player, avatar):
#	assert(player != null)
#	assert(avatar != null)
#	avatars[player.player_number-1] = avatar
#	print("P%d avatar assigned "%player.player_number, avatar)
#	assert(avatars[player.player_number-1] != null)
#
#func _on_avatar_unassigned_to_player(player, avatar):
#	avatars[player.player_number-1] = null
