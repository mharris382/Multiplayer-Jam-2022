extends Node2D
#NOTE: if you have merge conflicts take yours but make sure to comment out line 45 to avoid errors

export var block_base_id = 4 #use id for now, i'm not certain name works yet, but i know id will
export var block_jump_id = 5 #we don't have a block data for jump yet

var block_base = preload("res://source/blocks/BlockFunctions/base/BlockNormalPlatform.tscn")
var block_jumper = preload("res://source/blocks/BlockFunctions/jumper/BlockJumper.tscn")

var block_ghost =preload("res://source/blocks/BlockFunctions/ghost/BlockGhost.tscn")
var FlyingText = preload("res://source/ui/flying_text/FlyingText.tscn")#? move this to UI system

onready var tile_map :TileMap = $"Tilemap"

var last_hovered_tile = Vector2(0,0)
var preview_block= null
var ref_array = []


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
	tile_map.modulate = Color(1,1,1,0) #this is interesting, i like it

#*On mouse input (placing blocks)
func _input(event):
	match event.get_class():
		"InputEventMouseButton":
			#*place
			if event.button_index == BUTTON_LEFT and event.pressed:
				var grid_position = mouse_to_grid_pos()
				var block_id =1
				tile_map.set_cell(grid_position.x, grid_position.y, block_id)
				
				#instance an block object
				var block_data = Blocks.get_block_data(block_base_id)
				assert(block_data != null)
				var static_block_scene = block_data.static_block
				var block_ins = static_block_scene.instance()
				assert(block_ins is Node2D)
				block_ins.global_position=tile_map.map_to_world(Vector2(grid_position.x, grid_position.y))
				self.add_child(block_ins)
			#* Destroy
			elif event.button_index == BUTTON_RIGHT and event.pressed:
				var target = get_object_under_cursor()
				if(target!=null):
					var block_data = Blocks.get_block_data(block_base_id)
					assert(block_data != null)
					if(block_data.destructable):#? block hardeness	
						var grid_position = mouse_to_grid_pos()		
						tile_map.set_cell(grid_position.x, grid_position.y, -1)			
						target.queue_free()
					else:#? move this to UI system
						var text_ins = FlyingText.instance()
						text_ins.rect_global_position=get_global_mouse_position()
						self.add_child(text_ins)

		"InputEventMouseMotion":
			var grid_position = mouse_to_grid_pos()
			if(grid_position!=last_hovered_tile):
				
				if(preview_block!=null):
					preview_block.queue_free()
				
				preview_block = block_ghost.instance()
				
				#? this sync algo may cause something bad 
				var target_block=block_base.instance()
				preview_block.get_node("Sprite").texture = target_block.get_node("Sprite").texture
				target_block.queue_free()

				preview_block.global_position=tile_map.map_to_world(Vector2(grid_position.x, grid_position.y))
				self.add_child(preview_block)


#*mouse to grid pos
func mouse_to_grid_pos() ->Vector2:
	var x = get_local_mouse_position().x / tile_map.cell_size.x
	var y = get_local_mouse_position().y / tile_map.cell_size.y
	return tile_map.world_to_map(Vector2(x, y) * tile_map.cell_size)


#*get the current object those are under the cursor rn base on ySort
func get_object_under_cursor():
	var ret=null
	var space = get_world_2d().direct_space_state
	var mouse_pos = get_global_mouse_position()
	var results = space.intersect_point(mouse_pos, 32, [], 2147483647, false, true)	
	for result in results:
		if (ret == null or (ret.global_position.y<result.collider.owner.global_position.y)):
			ret = result.collider.owner
	return ret

func delete_tile(tile_position):
	print(tile_position)
	#print(tile_map.get_used_cells())
	var id = tile_map.get_cell(tile_position.x, tile_position.y)
	print(id)
	var name = tile_map.tile_set.tile_get_name(id)
	print(name)
	tile_map.set_cell(position.x, position.y, -1)
