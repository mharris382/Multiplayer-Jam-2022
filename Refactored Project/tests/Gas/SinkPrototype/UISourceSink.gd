extends Node2D

signal On_Button_Pressed()
signal Off_Button_Pressed()


export var show_icons = false
export var startOff = true

onready var icon_sprite = $"IconSprite"
onready var gasLight = $"Gas Light"

onready var source_icon = load("res://assets/ui/editor/source-icon-rounded.png")
onready var sink_icon = load("res://assets/ui/editor/sink-icon-rounded.png")
onready var disabled_icon = load("res://assets/ui/editor/pause-icon.png")



func _ready():
	if startOff:
		gasLight.Off_Button_Pressed()
	else:
		gasLight.On_Button_Pressed()

func _update_icons():
	if show_icons:
		pass
	else:
		pass


func _on_On_Button_button_down():
	gasLight.On_Button_Pressed()
	emit_signal("On_Button_Pressed")

func _on_Off_Button_button_down():
	gasLight.Off_Button_Pressed()
	emit_signal("Off_Button_Pressed")

