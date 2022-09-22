extends Node2D


# Declare member variables here. Examples:
# var a = 2
# var b = "text"

onready var build_audio : AudioStreamPlayer2D = $"Build AudioStreamPlayer2D"


func _ready():
	pass # Replace with function body.


# Called every frame. 'delta' is the elapsed time since the previous frame.
#func _process(delta):
#	pass


func _on_Block_Editor_OnBlockBuilt(cell):
	build_audio.play(0)


func _on_Block_Editor_OnBlockRemoved(cell):
	build_audio.play(0)
