class_name BlockInventory
extends Node

signal block_type_added(new_block)
signal block_type_removed(removed_block)
signal block_count_changed(block_name, amount) #called when count changed, but a block was not added or removed


#note if the inventory size is -1, the inventory will not be limited by space
export var inventory_size = -1

var inventory = {}


#returns -1 if the block name is invalid
func get_block_amount(block_name) -> int:
	if not Blocks.has_block(block_name):
		return -1
	if not inventory.has(block_name):
		return 0
	return inventory[block_name]

func remove_block(block_name):
	if not Blocks.has_block(block_name):
		return
	
	var cnt = get_block_amount(block_name)
	if  cnt > 0:
		cnt-=1
	inventory[block_name] = cnt
	
	if cnt <= 0:
		inventory.erase(block_name)
		emit_signal("block_type_removed", block_name)
	else:
		emit_signal("block_count_changed", block_name, get_block_amount(block_name))

#returns whether or not the block was added successfully
func try_add_block(block_name) -> bool:
	var cnt = get_block_amount(block_name)
	if cnt == -1:
		return false
	inventory[block_name] = cnt + 1
	
	if cnt == 0:
		emit_signal("block_type_added", block_name)
	else:
		emit_signal("block_count_changed", block_name, cnt+1)
	return true

func _on_Character_block_pickup_occured(picked_up_block_name):
	if picked_up_block_name == null or picked_up_block_name == "":
		print("BlockInventory.gd tried to pickup null block")
		return

	if not Blocks.has_block(picked_up_block_name):
		printerr("BlockInventory.gd Missing Block named: %s"%picked_up_block_name)
		return
	
	if try_add_block(picked_up_block_name):
		print("Successfully picked up block")
