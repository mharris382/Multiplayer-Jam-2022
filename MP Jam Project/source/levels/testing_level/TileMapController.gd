extends Node2D



var block_base = preload("res://source/blocks/base/BlockNormalPlatform.tscn")
var block_jumper = preload("res://source/blocks/jumper/BlockJumper.tscn")

onready var tile_map :TileMap = $"Tilemap"



#*Initial the Virtual Block packed OBJ grid
func _ready():	
	var used_tiles = tile_map.get_used_cells()
	for used_tile in used_tiles:
		var id = tile_map.get_cell(used_tile.x, used_tile.y)
		var name = tile_map.tile_set.tile_get_name(id)
		#print("Grid Location: ", used_tile, " ID: ", id, " Name: ", name)
		match name:
			"base":
				var block_ins = block_base.instance()
				block_ins.global_position=tile_map.map_to_world(Vector2(used_tile.x, used_tile.y))
				self.add_child(block_ins) 
			"jumper":
				var block_ins = block_jumper.instance()
				block_ins.global_position=tile_map.map_to_world(Vector2(used_tile.x, used_tile.y))
				self.add_child(block_ins)
	#turn off the tilemap texture which only use for level designing
	tile_map.modulate = Color(1,1,1,0)






#*On mouse input (placing blocks)
func _input(event):
	if event is InputEventMouseButton:
		if event.button_index == BUTTON_LEFT and event.pressed:
			var x = get_local_mouse_position().x / tile_map.cell_size.x
			var y = get_local_mouse_position().y / tile_map.cell_size.y
			var grid_position = tile_map.world_to_map(Vector2(x, y) * tile_map.cell_size)
			
			#create the block texture
			var block_id =1
			tile_map.set_cell(grid_position.x, grid_position.y, block_id)
			
			#instance an object
			var block_ins = block_base.instance()
			block_ins.global_position=tile_map.map_to_world(Vector2(grid_position.x, grid_position.y))
			self.add_child(block_ins)


