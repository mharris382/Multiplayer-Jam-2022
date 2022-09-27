extends Node2D


onready var gas_sprite = $Hover_Gas
onready var block_sprite = $Hover_Block

func _on_Block_Editor_OnBlockHoverChanged(blockCell):
	block_sprite.global_position = blockCell

func _on_Block_Editor_OnGasHoverChanged(wp):
	gas_sprite.global_position = wp


func _on_Runtime_Map_Editor_OnBlockHoverChanged(blockCell):
	_on_Block_Editor_OnBlockHoverChanged(blockCell)


func _on_Runtime_Map_Editor_OnGasHoverChanged(gasCell):
	_on_Block_Editor_OnGasHoverChanged(gasCell)
