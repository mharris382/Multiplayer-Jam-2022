extends Node2D

signal preview_ui_state_changed()

signal block_changed

enum PreviewState{
	NO_PREVIEW,
	INVALID,
	VALID
}

var previewing_block
var block_name = "" setget block_name_set, block_name_get

func block_name_get():
	return block_name
	
func block_name_set(new_block_name):
	if new_block_name.length() > 0:
		print("dynamic block set to ", new_block_name)
		block_name = new_block_name
		emit_signal("block_changed", block_name)

func _ready():
	pass # Replace with function body.

func show_preview(block_name, preview_parent_node):
	pass
	
func hide_preview():
	pass
	
