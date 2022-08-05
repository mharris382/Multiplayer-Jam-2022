extends AnimatedSprite

onready var _animated_sprite = $AnimatedSprite

func _ready():
	pass

func _process(delta):
	pass

func jump_up():
	if Input.is_action_pressed("ui_right"):
		_animated_sprite.play("jump")
	else:
		_animated_sprite.play("idle")
