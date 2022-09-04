extends TileMap

func _ready():
	instance_dynamics()

func instance_dynamics():
	var puzzle_parent = get_node(@".")
	if not puzzle_parent is Puzzle:
		puzzle_parent = get_node(@".").get_node(@".")

	var cells: Array = get_used_cells()
	for cell in cells:
		var id = get_cellv(cell)
		var tile_name = tile_set.tile_get_name(id)
		var data = Blocks.get_block_data(tile_name)
		if data != null and data.dynamic_block != null:
			var new_block = data.dynamic_block.instance()
			get_parent().add_child(new_block)
			var new_position = map_to_world(cell)
			new_block.should_teleport = true
			new_block.node_pos = new_position
		set_cellv(cell, -1)
	update_dirty_quadrants() #only call this once at the end of the loop not for each 
							 #got it chief
