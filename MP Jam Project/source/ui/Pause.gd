extends Control

func _ready():
	hide()

func _input(event):
	if Input.is_action_just_pressed("pause"):
		get_tree().paused = !get_tree().paused
		visible = !visible
