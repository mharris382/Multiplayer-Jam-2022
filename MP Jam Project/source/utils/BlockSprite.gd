extends Sprite


func _on_block_changed(block):
	if block is BlockData:
		_update_block_sprite(block as BlockData)
	elif block is String:
		var data =Blocks.get_block_data(block)
		if data != null:
			_update_block_sprite(data)
	else:
		print("Invalid block changed parameter: ", block)
		
		
func _update_block_sprite(block_data:BlockData):
	region_enabled = true
	var tile_set : TileSet = Blocks.block_library.tile_set
	var tile_name = block_data.resource_name
	var tile_id = tile_set.find_tile_by_name(tile_name)
	if tile_id == -1:
		print("Error: no tile found in tile_set:", tile_set, " named ", tile_name)
	else:
		region_rect = tile_set.tile_get_region(tile_id)
