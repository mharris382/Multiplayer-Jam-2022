class_name Builder
extends CharacterBase

export var block_schemes: Array = []
var picked_id: int = 0 
var charges = 0
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

func on_player_just_pressed_interact():
	destroy_block()
	
func manage_building():
	if build_mode == true:
		transform_position_to_grid()
		if Input.is_mouse_button_pressed(BUTTON_LEFT):
			build_block()
			build_mode = false
	
func build_block():
	if charges > 0 and block_schemes.size() > 0:
		var look_for_tile = !Blocks.block_has_static_scene(block_schemes[picked_id])
		if look_for_tile:
			if Blocks.has_block(block_schemes[picked_id]):
				for node in get_tree().get_nodes_in_group("Tilemap"):
					var id = node.find_tile_by_name(block_schemes[picked_id])
					if id != -1:
						var block_position = node.global_to_map(placement_position)
						node.set_cellv(block_position, id)
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
	collision_pos.y += 1
	var collided_block = move_and_collide(collision_pos, false, true, true)
	if collided_block != null:
		if "should_teleport" in collided_block:
			collided_block.queue_free()
			change_picked_by(1)
	
	
func transform_position_to_grid():
	var mouse_pos = get_local_mouse_position()
	placement_position.x = round(mouse_pos.x / 128)
	placement_position.y = round(mouse_pos.y / 128)
	placement_position *= 128
		
		
func change_picked_by(value):
	picked_id = clamp(picked_id + value, 0, block_schemes.size())
