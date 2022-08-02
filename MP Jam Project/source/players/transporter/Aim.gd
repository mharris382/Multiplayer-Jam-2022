extends Position2D
export var minimum_range := Vector2(-100,-100)
export var maximum_range := Vector2(100,100)

func _process(delta):
	var pos = get_local_mouse_position()
	position.x = clamp(pos.x, minimum_range.x, maximum_range.x)
	position.y = clamp(pos.y, minimum_range.y, maximum_range.y)
