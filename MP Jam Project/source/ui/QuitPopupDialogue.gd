extends PopupDialog


# Declare member variables here. Examples:
# var a = 2
# var b = "text"


func _on_Quit_Canceled_Button_down():
	visible = false
	


func _on_Quit_Quit_Button_down():
	get_tree().quit()
