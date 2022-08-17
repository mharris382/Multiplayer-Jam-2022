extends Control

signal on_transporter_ui_dirty(block_center, block_next, block_prev)

signal on_builder_center_changed(block_name, block_count)
signal on_builder_next_changed(block_name) 
signal on_builder_prev_changed(block_name)

func _ready():
	pass
