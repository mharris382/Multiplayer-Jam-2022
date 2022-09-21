tool
extends Node2D


export var light_on = true;
export var light_intensity = 1.0
export var light_color = Color.white
export var off_color = Color.black
export var on_color = Color.white
onready var light : Light2D = $Sprite/Light2D
onready var sprite : Sprite = $Sprite
func _get_light():
	if(light == null):
		light = get_node("Sprite/Light2D")
	if sprite == null:
		sprite = get_node("Sprite")
	
# Called when the node enters the scene tree for the first time.
func _ready():
	_get_light()


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
	_get_light()
	sprite.self_modulate = lerp(off_color, on_color, clamp(light_intensity, 0, 1))
	light.visible = light_on
	light.energy = light_intensity
	light.color = light_color
	light.enabled = light_on
	


func _on_Tween_tween_step(object, key, elapsed, value):
	pass # Replace with function body.
