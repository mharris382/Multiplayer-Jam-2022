extends Node2D


onready var test_image = $Sprite
onready var test_aim = $Sprite2
onready var aim_preview = $AimPreview


func _on_aim_angle_changed(aim_direction, aim_angle):
	test_aim.position = transform.basis_xform(aim_direction * 145)
	aim_preview._on_aim_angle_changed(aim_direction, aim_angle)


func _on_move_changed(move_direction):
	test_image.position = transform.basis_xform(move_direction * 100)
