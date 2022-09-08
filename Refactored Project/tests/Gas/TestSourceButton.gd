extends Node2D

signal On_Button_Pressed()
signal Off_Button_Pressed()

export var startOff = true
onready var gasLight = $"Gas Light"

func _ready():
	if startOff:
		gasLight.Off_Button_Pressed()
	else:
		gasLight.On_Button_Pressed()




func _on_On_Button_button_down():
	gasLight.On_Button_Pressed()
	emit_signal("On_Button_Pressed")

func _on_Off_Button_button_down():
	gasLight.Off_Button_Pressed()
	emit_signal("Off_Button_Pressed")
