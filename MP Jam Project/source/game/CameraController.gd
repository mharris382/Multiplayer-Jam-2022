extends Node

var player_position_map = {}
var player_path_map = {}

var player_avatars = {}
var avatars = [null, null]

export var viewport_size = Vector2(958, 540)

export var prefer_zoom= Vector2(1,1)
var target_zoom = Vector2(1,1)

onready var camera = $Camera2D


var avatar_min_pos = Vector2(100000,100000)
var avatar_max_pos = Vector2(-100000,-100000)

onready var test_point = $TestPoint
onready var test_point1 = $TestPoint2
onready var test_point2 = $TestPoint3
onready var label = $Label

func _test_pos(pnt, position):
	pnt.position = position
	pnt.get_child(0).name = String(pnt.position)
func _test_1():
	var pnt = test_point as Node2D
	_test_pos(test_point,camera.get_camera_screen_center())
	
func _test_2():
	var center =test_point.position
	var pnt = test_point1 as Node2D
	var viewport = get_viewport()
	test_point1.position = center - ((viewport.size / 2.0) * camera.zoom)
	
func _test_3():
	var center =test_point.position
	var viewport = get_viewport()
	test_point2.position = center + ((viewport.size / 2.0) * camera.zoom)
	
	pass
func _test_4():
	var center = camera.get_camera_screen_center()
	
	var offset = (get_viewport().size / 2.0) * camera.zoom
	var top_left = center - offset
	var bottom_right = center + offset
	var p1 =avatars[0].global_position
	var p2 =avatars[1].global_position
	
	avatar_min_pos = Vector2(min(p1.x, p2.x),  min(p1.y, p2.y))
	avatar_max_pos = Vector2(max(p1.x, p2.x),  max(p1.y, p2.y))
	var avatar_dist = (avatar_max_pos - avatar_min_pos)
	var y_dist = avatar_dist.y
	
	var size  =  get_viewport().size * camera.zoom
	var t_size = get_viewport().size * prefer_zoom
	
	if t_size.y > y_dist:
		print("able to zoom in")
		
	if size.y < y_dist:
		print("need to zoom out")
	
	var x_dist = avatar_dist.x
	if size.x < x_dist:
		print("Need to change x zoom")
	var req_size = avatar_max_pos - avatar_min_pos
	
	pass
func determine_size(minimum_size):
	pass

func _tests():
	_test_1()
	_test_2()
	_test_3()
	_test_4()
	_print_results()
	
func _print_results():
	label.text = "Camera Screen Center: %s\nGlobal Transform Center: %s\n...%s" % [test_point.position, test_point1.position, test_point2.position]
	
	
	
	
var target_size = Vector2(0,0)

func _init():
	Players.connect("avatar_was_assigned", self, "_on_avatar_assigned_to_player")
	Players.connect("avatar_was_unassigned", self, "_on_avatar_unassigned_to_player")
	
	
func _ready():
	camera.current = true
	target_zoom = camera.zoom
	var viewport = get_viewport()

func _process(delta):
	var current_camera_position = camera.position
	camera.current = true
	_tests()
	var target_position = _calculate_target_position()
	if target_position == null: #edge case: null if avatar count = 0
		return
		
	camera.position = target_position
	var viewport = get_viewport()
	


#-----------------------------------------------
#helpers

func _compare_position(position):
	var x_max = avatar_max_pos.x
	var x_min = avatar_min_pos.x
	if x_max < position.x:
		x_max = position.x
	elif x_min > position.x:
		x_min = position.x
		
	var y_max = avatar_max_pos.y
	var y_min = avatar_min_pos.y
	if y_max < position.y:
		y_max = position.y
	elif y_min > position.y:
		y_min = position.y
		
	avatar_max_pos = Vector2(x_max, y_max)
	avatar_min_pos = Vector2(x_min, y_min)

func _calculate_target_position():
	var target_position = Vector2.ZERO #initialize to zero
	var count = 0
	avatar_min_pos = Vector2(100000,100000)
	avatar_max_pos = Vector2(-100000,-100000)
	
	for avatar in avatars:
		assert(avatar != null)
		if avatar != null:
			count += 1
			target_position += avatar.global_position
			_compare_position(avatar.global_position)
	
	if count == 0:
		camera.position = Vector2.ZERO
		return null
	
	#this is a simple average, can be turned into a weighted average with some modifications
	return Vector2(target_position.x / float(count), target_position.y / float(count))



#-----------------------------------------------
#signals 

func _on_avatar_assigned_to_player(player, avatar):
	assert(player != null)
	assert(avatar != null)
	avatars[player.player_number-1] = avatar
	print("P%d avatar assigned "%player.player_number, avatar)
	assert(avatars[player.player_number-1] != null)
	
func _on_avatar_unassigned_to_player(player, avatar):
	if player_position_map.has(player) and player_position_map[player] != null:
		player_position_map[player].remote_path = null
