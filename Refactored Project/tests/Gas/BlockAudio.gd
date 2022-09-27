extends Node2D


export var build_pitch_range = Vector2(0.9, 1.1)
export var build_volume_range = Vector2(-20, 0)

export var remove_pitch_range = Vector2(0.9, 1.1)
export var remove_volume_range = Vector2(-30, -10)

onready var build_audio : AudioStreamPlayer2D = $"Build AudioStreamPlayer2D"
onready var rng : RandomNumberGenerator = RandomNumberGenerator.new()



func _on_Block_Editor_OnBlockBuilt(cell):
	build_audio.pitch_scale = rng.randf_range(build_pitch_range.x, build_pitch_range.y)
	build_audio.volume_db = rng.randf_range(build_volume_range.x, build_volume_range.y)
	build_audio.play(0)


func _on_Block_Editor_OnBlockRemoved(cell):
	build_audio.pitch_scale = rng.randf_range(remove_pitch_range.x, remove_pitch_range.y)
	build_audio.volume_db = rng.randf_range(remove_volume_range.x, remove_volume_range.y)
	build_audio.play(0)
