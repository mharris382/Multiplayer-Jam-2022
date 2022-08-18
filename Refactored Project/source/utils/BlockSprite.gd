extends Sprite
var current_name

func _on_block_changed(block):
	print("BlockSprite: on block changed to ", block)
	if block is String:
		
		_update_block_sprite(block)
	else:
		print("BlockSprite: Invalid block changed parameter:() must be string type) ", block)
		
		
func _update_block_sprite(block_name : String):
	region_enabled = true
	var tile_set : TileSet = Blocks.block_library.block_tile_set
	var tile_id = tile_set.find_tile_by_name(block_name)
	if tile_id == -1:
		print("BlockSprite: Error: no tile found in tile_set:", tile_set, " named ", block_name)
	else:
		current_name = block_name
		region_rect = tile_set.tile_get_region(tile_id)

static func get_block_region(block_name):
	var tile_set : TileSet = Blocks.block_library.tile_set
	var tile_id = tile_set.find_tile_by_name(block_name)
