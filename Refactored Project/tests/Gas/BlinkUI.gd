extends Node2D




onready var icons :Node2D = $"Icons"


func _on_Show_Icons_Button_button_down():
	icons.modulate = Color.transparent if icons.modulate==Color.white else Color.white


