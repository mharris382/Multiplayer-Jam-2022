extends Sprite


export var show_icons = false

onready var icon_sprite = $"IconSprite"
onready var source_icon = load("res://assets/ui/editor/source-icon-rounded.png")
onready var sink_icon = load("res://assets/ui/editor/sink-icon-rounded.png")
onready var disabled_icon = load("res://assets/ui/editor/pause-icon.png")

var rate = 0

func _ready():
	_update_icons()

func _rate_changed(new_rate):
	if new_rate != rate:
		rate = sign(new_rate)
		_update_icons()

func _update_icons():
	if show_icons:
		icon_sprite.visible = true
		match(rate):
			0:
				icon_sprite.texture = disabled_icon
			1:
				icon_sprite.texture = source_icon
			-1:
				icon_sprite.texture = sink_icon
	else:
		icon_sprite.visible = false
