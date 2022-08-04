class_name Builder
extends CharacterBase

var block_schemes: Array = []
var picked_id: int = -1 
var charges = 0
var placement_position: Vector2 = Vector2(0,0)
var build_mode = false

func _ready():
	assign_player(Players.players[1])

func is_direction_valid(aim_direction) -> bool:
	match(aim_direction):
		AimDirection.BELOW:
			return !is_on_floor()
		AimDirection.ABOVE:
			return !is_on_ceiling()
		AimDirection.FRONT:
			return !is_on_floor()
	return true
	
func manage_building():
	if build_mode == true:
		var mouse_pos = get_local_mouse_position()
		placement_position.x = round(mouse_pos.x / 128)
		placement_position.y = round(mouse_pos.y / 128)
		placement_position *= 128
		print(placement_position)
	
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
						break
		else:
			var block = Blocks.instance_static_block(block_schemes[picked_id])
			if block != null:
				get_parent().add_child(block)
				block.position = to_global(placement_position)
	
	
func change_picked_by(value):
	picked_id = clamp(picked_id + value, 0, block_schemes.size())
