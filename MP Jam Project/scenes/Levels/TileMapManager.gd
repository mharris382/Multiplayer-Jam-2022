extends TileMap

signal level_completed
signal level_progress_changed(old_progress, new_progress)

var previous_progress

# Called when the node enters the scene tree for the first time.
func _ready():
	pass


func build_dynamic_block(grid_pos):
	var block_name = delete_tile(grid_pos)
	if block_name != null:
		var data = Blocks.get_block_data(block_name)
		if data.dynamic_block != null:
			var new_block = data.dynamic_block.instance()
			var pos = map_to_world(grid_pos)
			get_parent().add_child(new_block)
			new_block.block_name = block_name
			new_block.should_teleport = true
			new_block.node_pos = pos
			
func update_dirty_quadrants():
	.update_dirty_quadrants()


func cell_get_block_data(grid_pos) -> BlockData:
	var id = get_cell(grid_pos.x, grid_pos.y)
	var tile_name = tile_set.tile_get_name(id)
	return Blocks.get_block_data(tile_name)


func cell_has_block_data(grid_position) -> bool:
	var id = get_cell(grid_position.x, grid_position.y)
	var tile_name = tile_set.tile_get_name(id)
	return Blocks.has_block(tile_name)


func delete_tile(tile_position):
	var id = get_cell(tile_position.x, tile_position.y)
	if id != -1:
		var name = tile_set.tile_get_name(id)
		set_cell(tile_position.x, tile_position.y, -1)
		fix_invalid_tiles()
		return name
	else:
		return null
