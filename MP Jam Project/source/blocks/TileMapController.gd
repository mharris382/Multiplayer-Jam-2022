extends Node2D
#NOTE: if you have merge conflicts take yours but make sure to comment out line 45 to avoid errors

export var custom_tile_mapping = {
	"base" : "",
	"jump" : ""
}
export var block_base_id = 4 #use id for now, i'm not certain name works yet, but i know id will
export var block_jump_id = 5 #we don't have a block data for jump yet

var block_wrapper : PackedScene = preload("res://source/utils/BlockWrapper.tscn")
var block_ghost =preload("res://source/blocks/BlockFunctions/ghost/BlockGhost.tscn")
var FlyingText = preload("res://source/ui/flying_text/FlyingText.tscn")#? move this to UI system

onready var tile_map :TileMap = $"Tilemap"
onready var cells = $Cells

var last_hovered_tile = Vector2(0,0)
var preview_block= null
var ref_array = []
var min_cell : Vector2
var max_cell : Vector2

#*Initial the Virtual Block packed OBJ grid
func _ready():	
	max_cell = Vector2(-1000000, -1000000)
	min_cell = Vector2(10000000, 10000000)
	var used_tiles = tile_map.get_used_cells()
	for used_tile in used_tiles:
		var block = cell_get_block_data(used_tile)
		if block != null:
			build_static_block(block, used_tile)
			if max_cell.x < used_tile.x:
				max_cell.x = used_tile.x
			elif min_cell.x > used_tile.x:
				min_cell.x = used_tile.x
			if max_cell.y < used_tile.y:
				max_cell.y = used_tile.y
			elif min_cell.y > used_tile.y:
				min_cell.y = used_tile.y

#		match name:
#			"base":
#				if Blocks.block_has_static_scene(block_base_id):
#					var pos = tile_map.map_to_world(used_tile)
#					var block_ins = Blocks.instance_static_block(block_base_id, pos)
#					self.add_child(block_ins)
#			"jumper":
#				if Blocks.block_has_static_scene(block_jump_id):
#					var pos = tile_map.map_to_world(Vector2(used_tile.x, used_tile.y))
#					var block_ins = Blocks.instance_static_block(block_jump_id, pos)
#					self.add_child(block_ins)

	#turn off the tilemap texture which only use for level designing
	tile_map.modulate = Color(1,1,1,0) #this is interesting, i like it
	var grid_size = max_cell - min_cell
	print("Grid size = ", grid_size)

func cell_has_block(grid_position):
	return

func cell_get_block_data(grid_position) -> BlockData:
	var id = tile_map.get_cell(grid_position.x, grid_position.y)
	var tile_name = tile_map.tile_set.tile_get_name(id)
	return Blocks.get_block_data(tile_name)

func cell_has_block_data(grid_position) -> bool:
	var id = tile_map.get_cell(grid_position.x, grid_position.y)
	var tile_name = tile_map.tile_set.tile_get_name(id)
	return Blocks.has_block(tile_name)

func cell_clear(grid_pos):
	var block_wrapper = get_block_wrapper(grid_pos)
	if block_wrapper!=null:
		block_wrapper.queue_free()
		delete_tile(grid_pos)
	
func cell_init_block_wrapper(grid_pos):
	var tile_id = tile_map.get_cellv(grid_pos)
	var tile_name = tile_map.tile_set.tile_get_name(tile_id)
	if !Blocks.has_block(tile_name):
		return
	var block_wrapper = get_or_create_block_wrapper(grid_pos)
	block_wrapper.name = "(%d, %d) %s" % [tile_name, grid_pos.x, grid_pos.y]
	block_wrapper.block_data = Blocks.get_block_data(tile_name)

func set_block_wrapper_block(grid_pos, block):
	var block_wrapper = get_or_create_block_wrapper(grid_pos)
	pass
	
func get_block_wrapper(grid_pos) -> Node2D:
	var path_to_wrapper = "%d/%d" % [grid_pos.x, grid_pos.y]
	if cells.has_node(path_to_wrapper):
		return cells.get_node(path_to_wrapper)
	return null

func get_or_create_block_wrapper(grid_pos : Vector2) -> Node2D:
	var existing_wrapper = get_block_wrapper(grid_pos)
	if existing_wrapper != null:
		return existing_wrapper
	
	var new_block_wrapper=block_wrapper.instance()
	tile_map.map_to_world(grid_pos)
	return new_block_wrapper

func build_static_block(block, grid_pos):
	if Blocks.block_has_static_scene(block):
		var block_ins = Blocks.instance_static_block(block, tile_map.map_to_world(Vector2(grid_pos.x, grid_pos.y)))
		get_or_create_block_wrapper(grid_pos).add_child(block_ins)


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
	var id = tile_map.get_cell(tile_position.x, tile_position.y)
	var name = tile_map.tile_set.tile_get_name(id)
	tile_map.set_cell(tile_position.x, tile_position.y, -1)
	tile_map.fix_invalid_tiles()

# Tilemap should not be recieving input.  that is not part of it's responsibility
# See Single Responsibility Principle - SOLID 

##*On mouse input (placing blocks)
#func _input(event):
#	match event.get_class():
#		"InputEventMouseButton":
#			#*place
#			if event.button_index == BUTTON_LEFT and event.pressed:
#				var grid_position = mouse_to_grid_pos()
#				var block_id =1
#				tile_map.set_cell(grid_position.x, grid_position.y, block_id)
#
#				#instance an block object
#				var block_data = Blocks.get_block_data(block_base_id)
#				assert(block_data != null)
#				var static_block_scene = block_data.static_block
#				var block_ins = static_block_scene.instance()
#				assert(block_ins is Node2D)
#				block_ins.global_position=tile_map.map_to_world(Vector2(grid_position.x, grid_position.y))
#				self.add_child(block_ins)
#			#* Destroy
#			elif event.button_index == BUTTON_RIGHT and event.pressed:
#				var target = get_object_under_cursor()
#				if(target!=null):
#					var block_data = Blocks.get_block_data(block_base_id)
#					assert(block_data != null)
#					if(block_data.destructable):#? block hardeness	
#						var grid_position = mouse_to_grid_pos()		
#						tile_map.set_cell(grid_position.x, grid_position.y, -1)			
#						target.queue_free()
#					else:#? move this to UI system
#						var text_ins = FlyingText.instance()
#						text_ins.rect_global_position=get_global_mouse_position()
#						self.add_child(text_ins)
#
#		"InputEventMouseMotion":
#			var grid_position = mouse_to_grid_pos()
#			if(grid_position!=last_hovered_tile):
#
#				if(preview_block!=null):
#					preview_block.queue_free()
#
#				preview_block = block_ghost.instance()
#
#				#? this sync algo may cause something bad 
#				var target_block=block_base.instance()
#				preview_block.get_node("Sprite").texture = target_block.get_node("Sprite").texture
#				target_block.queue_free()
#
#				preview_block.global_position=tile_map.map_to_world(Vector2(grid_position.x, grid_position.y))
#				self.add_child(preview_block)



