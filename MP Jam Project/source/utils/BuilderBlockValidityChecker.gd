extends Node


enum BuilderActionValidityResult{
	ACTION_IS_VALID = 0,
	BLOCK_DOES_NOT_EXIST = 1,
	BLOCK_IS_NON_DESTRUCTABLE = 2,
	TILE_IS_BLOCKED = 3
	TILEMAP_NOT_FOUND = 4
}

onready var area = $Area2D


func determine_build_action_validity(block_name, grid_location):
	if Blocks.has_block(block_name) == false:
		return BuilderActionValidityResult.BLOCK_DOES_NOT_EXIST
	if Blocks.has_block(block_name) and not Blocks.get_block_data(block_name).destructable:
		return BuilderActionValidityResult.BLOCK_IS_NON_DESTRUCTABLE

	var overlapping_bodies = area.get_overlapping_bodies()
	var overlapping_areas = area.get_overlapping_areas()
	var active_tilemaps = get_tree().get_nodes_in_group("Tilemap")
	
	if active_tilemaps.size() == 0:
		return  BuilderActionValidityResult.TILEMAP_NOT_FOUND
		
	var active_tilemap = active_tilemaps[0]
	
	if active_tilemap == null:
		return BuilderActionValidityResult.TILEMAP_NOT_FOUND

	return BuilderActionValidityResult.ACTION_IS_VALID


func is_valid_build(block_name, grid_location) -> bool:
	if Blocks.has_block(block_name) == false:
		print("")
		return false
	
	return false



func _on_Area2D_area_entered(area):
	print(get_parent().name, " area entered ", area.name)
	pass # Replace with function body.


func _on_Area2D_area_exited(area):
	print(get_parent().name, " area exited ", area.name)
	pass # Replace with function body.
