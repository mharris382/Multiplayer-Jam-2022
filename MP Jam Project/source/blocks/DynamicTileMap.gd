extends TileMap

func _ready():
	instance_dynamics()

func instance_dynamics():
	var cells: Array = get_used_cells()
	for cell in cells:
		var id = get_cellv(cell)
		var tile_name = tile_set.tile_get_name(id)
		var block = Blocks.get_block_data(tile_name)
		if block != null and block.dynamic_block != null:
			var new_block = block.dynamic_block.instance()
			get_parent().add_child(new_block)
			var new_position = map_to_world(cell)
			new_block.should_teleport = true
			new_block.node_pos = new_position
		set_cellv(cell, -1)
		update_dirty_quadrants()
