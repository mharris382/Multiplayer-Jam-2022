tool
extends Node
#NOTE: this script was originally meant to allow more than 2 avatars but I simplified it, as it is currently it will only function with 0 avatars or 2 avatars
var avatars = [null, null]
var target_zoom = Vector2(1,1)

export var avatar_padding = Vector2(25, 25)
export var prefer_zoom= Vector2(1,1)
export var limit_zoom = Vector2(4, 4)
export var reset_speed = 4.0

export var lower_offset = Vector2(0, -214)
export var upper_offset = Vector2(0, -388)

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
	_get_avatars()
	

func _process(delta):
	if camera == null:
		camera = get_node("Camera2D")
		camera.current = true
	var current_camera_position = camera.position
	camera.current = true
	
	_set_offsets()
	var target_location = _get_target_location()
	var target_zoom = _get_target_zoom()
	camera.position = target_location
	camera.zoom = target_zoom
	camera.zoom = Vector2(lerp(target_zoom.x, prefer_zoom.x, delta), lerp(target_zoom.y, prefer_zoom.y, delta * reset_speed))
	

#-----------------------------------------------
#helpers

func _get_avatar_position(avatar_index :int):
	_get_avatars()
	
	if avatar_index > avatars.size():
		printerr("index is invalid: ", avatar_index)
		avatar_index = 0
		
	var avatar = avatars[avatar_index] as Node2D
	if avatar == null:
		printerr("Avatar at index is null", avatar_index)
		return camera.global_position
	return avatar.global_position
	
func _get_avatar_actual_position(avatar_index :int):
	_get_avatars()
	
	if avatar_index > avatars.size():
		printerr("index is invalid: ", avatar_index)
		avatar_index = 0
		
	var avatar = avatars[avatar_index] as Node2D
	if avatar == null:
		printerr("Avatar at index is null", avatar_index)
		return camera.global_position
	return avatar.get_parent().global_position
	
func _get_target_location():
	var target_position = _calculate_target_position()
	if target_position == null: #edge case: null if avatar count = 0
		return camera.global_position
	return target_position
	
func _set_offsets():
	var p1 = _get_avatar_actual_position(0) as Vector2
	var p2 = _get_avatar_actual_position(1) as Vector2
	if p1.y > p2.y:
		var lower = avatars[0] as Node2D
		var upper = avatars[1] as Node2D
		lower.position = lower_offset
		upper.position = upper_offset
	else:
		var lower = avatars[1] as Node2D
		var upper = avatars[0] as Node2D
		lower.position = lower_offset
		upper.position = upper_offset
	

func _get_required_size():#to ensure all player avatars are visible
	var p1 = _get_avatar_position(0) as Vector2
	var p2 = _get_avatar_position(1) as Vector2
	
	var avatar_min_pos = Vector2(min(p1.x, p2.x),  min(p1.y, p2.y))
	var avatar_max_pos = Vector2(max(p1.x, p2.x),  max(p1.y, p2.y))
	var dif = avatar_max_pos - avatar_min_pos
	if dif.x > dif.y:
		dif = Vector2(dif.x, dif.x)
	else:
		dif = Vector2(dif.y, dif.y)
	dif += avatar_padding
	return dif

var size2 = Vector2(1080, 1080)
func _get_target_zoom():
	assert(limit_zoom.x >= prefer_zoom.x)
	assert(limit_zoom.y >= prefer_zoom.y)
	
	var viewport_size = size2# get_viewport().size
	
	var p1 = _get_avatar_position(0) as Vector2
	var p2 = _get_avatar_position(1) as Vector2
	
	var avatar_min_pos = Vector2(min(p1.x, p2.x),  min(p1.y, p2.y))
	var avatar_max_pos = Vector2(max(p1.x, p2.x),  max(p1.y, p2.y)) 
	
	
	var required_size = _get_required_size() #required size to ensure all player avatars are visible
	if required_size.x < target_zoom.x or required_size.y < target_zoom.y:
		print("required size = ", required_size, " target zoom = ", target_zoom)
	var current_size  = viewport_size * camera.zoom #current size of viewport in worldspace (after considering zoom)
	var prefer_size = viewport_size * prefer_zoom #prefered size is the most zoomed in allowed
	var limit_size = viewport_size *  limit_zoom #limit size is the most zoomed out allowed
	
	var target_size = Vector2(max(required_size.x, current_size.x), max(required_size.y, current_size.y))
	
	target_size.x = clamp(target_size.x, prefer_size.x, limit_size.x )
	target_size.y = clamp(target_size.y, prefer_size.y, limit_size.y )
	
	var target_zoom = target_size/viewport_size
	var zoom_max = min(target_zoom.x, target_zoom.y)
	return Vector2(zoom_max, zoom_max)
func _get_avatars():
	if avatars.size() != 2 or (avatars[0] == null or avatars[1] == null):
		avatars.clear()
		avatars.append(get_parent().get_node("Builder/Camera Target"))
		avatars.append(get_parent().get_node("Transporter/Camera Target"))
		
func _calculate_target_position():
	var target_position = Vector2.ZERO #initialize to zero
	var count = 0
	
	_get_avatars()
		
	for avatar in avatars:
		assert(avatar != null)
		if avatar != null:
			count += 1
			target_position += avatar.global_position
	
	if count == 0:
		camera.position = Vector2.ZERO
		printerr("No avatars found!")
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
