extends Sprite

export var start_on = false

onready var light = $Light2D


func _ready():
	Set_Light_On(start_on)

func On_Button_Pressed():
	Set_Light_On(true)

func Off_Button_Pressed():
	Set_Light_On(false)

func Set_Light_On( is_on : bool):
	modulate = Color.green if is_on else Color.red
	light.modulate = modulate
