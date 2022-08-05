class_name Builder
extends CharacterBase

export var block_schemes: Array = []
var picked_id: int = 0 
var charges = 1
var placement_position: Vector2 = Vector2(0,0)
var build_mode = false

func _ready():
	assign_player(Players.players[1])


func _process(delta):
	manage_building()
	
	
func is_direction_valid(aim_direction) -> bool:
	match(aim_direction):
		AimDirection.BELOW:
			return !is_on_floor()
		AimDirection.ABOVE:
			return !is_on_ceiling()
		AimDirection.FRONT:
			return !is_on_floor()
	return true


func on_player_just_pressed_ability(aim):
	build_mode = !build_mode
	print("Build mode is %s, right now." % build_mode)


func on_player_just_pressed_interact():
	destroy_block()

	
func manage_building():
	if build_mode == true:
		update_placement_position()
		if Input.is_mouse_button_pressed(BUTTON_LEFT):
			build_block()
			build_mode = false
	
	
func build_block():
	if charges > 0 and block_schemes.size() > 0:
		var position_for_block = position + placement_position
		var look_for_tile = !Blocks.block_has_static_scene(block_schemes[picked_id])
		if look_for_tile:
			if Blocks.has_block(block_schemes[picked_id]):
				for node in get_tree().get_nodes_in_group("Tilemap"):
					var id = node.tile_set.find_tile_by_name(block_schemes[picked_id])
					if id != -1:
						var block_position = node.world_to_map(position_for_block)
						if node.get_cellv(block_position) == -1:
							node.set_cellv(block_position, id)
							node.update_dirty_quadrants()
							change_picked_by(-1)
							break
		else:
			var block = Blocks.instance_static_block(block_schemes[picked_id])
			if block != null:
				get_parent().add_child(block)
				block.position = to_global(placement_position)
				change_picked_by(-1)
	

func destroy_block():
	var collision_pos = front_aim_point.position
	var collided_block = move_and_collide(collision_pos + Vector2(0,1), false, true, true)
	if collided_block != null:
		if "should_teleport" in collided_block:
			collided_block.queue_free()
			change_picked_by(1)
	
	
func colliding_with_block(block_position):
	var collision = move_and_collide(block_position, false, true, true)
	var tilemap = get_tree().get_nodes_in_group("Tilemap")[0]
	var predicted_position = tilemap.world_to_map(collision.position - collision.normal)
	var actual_position = tilemap.world_to_map(block_position + position)
	return predicted_position == actual_position
	
	
func update_placement_position():
	var mouse_pos = get_local_mouse_position()
	placement_position = mouse_pos
		
		
func change_picked_by(value):
	picked_id = clamp(picked_id + value, 0, block_schemes.size())
