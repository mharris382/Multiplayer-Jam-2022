extends Node2D
signal on_light_changed(new_intensity)
onready var timer = $Timer
onready var tween = $Timer

export var min_on_time = 0.1
export var max_on_time = .4
export var min_off_time = 0.2
export var max_off_time = 1
export var on_intensity = 1
export var off_intensity = 0.5

var wall_light

var is_on=false

void set_off():
	pass
void set_on():
	pass

