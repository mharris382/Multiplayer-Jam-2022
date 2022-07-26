class_name Builder
extends CharacterBase

signal block_built
signal try_pickup_blocks()
export var picked_block_name = "Block_Floor_Pipe_1"

export var block_schemes: PoolStringArray = []
export var charges = 1
export var auto_pickup_blocks = true

var picked_id: int = 0 
var placement_position: Vector2 = Vector2(0,0)
var build_mode = false

func get_equipped_block_name():
	return picked_block_name

func _ready():
	._ready()
	assign_player(Players.players[1])

func _get_player_number():
	return 1
func _process(delta):
	._process(delta)
	
	manage_building()
	if auto_pickup_blocks:
		get_node("Block Area2D").enable_pickups = true
	
	
func is_direction_valid(aim_direction) -> bool:
	match(aim_direction):
		AimDirection.BELOW:
			check_validity(below_aim_point)
		AimDirection.ABOVE:
			check_validity(above_aim_point)
		AimDirection.RIGHT:
			check_validity(right_aim_point)
		AimDirection.LEFT:
			check_validity(left_aim_point)
	return true

func check_validity(node):
	var checker = node.get_node("BuilderValidityChecker")
	if checker == null:
		return true
	var position = checker.position
	var map = get_tree().get_nodes_in_group("Tilemap")
	if map.size() == 0:
		print("Builder.gd ERROR: cannot build without tilemap")
	if map.size() > 1:
		print("Buidler.gd found ", map.size() ," tilemaps")
	var grid_pos = map[0].world_to_map(checker.position)
	var block = get_equipped_block_name()
	var validity = checker.determine_build_action_validity(block, grid_pos)
	
func on_player_just_pressed_ability(aim):
	build_mode = !build_mode
	if build_mode:
		print("Builder TODO: show preview")
	else:
		print("Builder TODO: hide preview")
	print("Build mode is %s, right now." % build_mode)


func on_player_just_pressed_interact():
	.on_player_just_pressed_interact()
	emit_signal("try_pickup_blocks")
	
func manage_building():
	if build_mode == true:
		update_placement_position()
		if Input.is_mouse_button_pressed(BUTTON_LEFT):
			build_block()
#			build_mode = false

func build_block():
	if charges > 0 and block_schemes.size() > 0:
		var position_for_block = position + placement_position
		var block_name = block_schemes[picked_id]
		_build_block_internal(position_for_block, block_name)
		
func _build_block_internal(position_for_block, block_name):
	if  !Blocks.block_has_static_scene(block_name):
		_build_block_as_tile(position_for_block, block_name)
	else:
		_build_block_as_static_block(position_for_block, block_name)

func _build_block_as_tile(position_for_block, block_name):
	if Blocks.has_block(block_name):
			for node in get_tree().get_nodes_in_group("Tilemap"):
				var id = node.tile_set.find_tile_by_name(block_name)
				if id != -1:
					var block_position = node.world_to_map(position_for_block)
					if node.get_cellv(block_position) == -1:
						node.set_cellv(block_position, id)
						node.update_dirty_quadrants()
						charges -= 1
						emit_signal("block_built")
						break

func _build_block_as_static_block(position_for_block, block_name):
	var block = Blocks.instance_static_block(block_name)
	if block != null:
		get_parent().add_child(block)
		block.position = to_global(placement_position)
		charges -= 1
		emit_signal("block_built")

func destroy_block():
	var collision_pos = front_aim_point.position
	var collided_block = move_and_collide(collision_pos, false, true, true)
	if collided_block != null:
		if "should_teleport" in collided_block.collider:
			print("it does work")
			collided_block.collider.queue_free()
			charges += 1
			

func update_placement_position():
	var mouse_pos = get_local_mouse_position()
	placement_position = mouse_pos
	
	
func change_picked_by(value):
	picked_id = clamp(picked_id + value, 0, block_schemes.size())


func _on_block_picked_up(block_data):
	AudioManager.play_feedback(AudioEnums.AudioFeedbacks.BLOCK_PICKUP)
	charges += 1
	pass # Replace with function body.
