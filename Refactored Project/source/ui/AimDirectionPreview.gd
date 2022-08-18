extends Line2D

export var fade_speed = 1
export var fade_pow = .2
export var preview_distance = 100
export var normalize_input = true

var cur_alpha = 1
var color

func _ready():
	color = default_color
	_on_aim_angle_changed(Vector2.RIGHT, 0)

func _on_aim_angle_changed(aim_direction :Vector2, aim_angle):
	var end_pnt
	var start_pnt
	
	if normalize_input:
		aim_direction = aim_direction.normalized()
	aim_direction *= preview_distance
	
	clear_points()
	start_pnt =transform.get_origin()
	end_pnt = transform.xform(aim_direction)
	
	add_point(start_pnt)
	add_point(end_pnt)

func interp(A, B, t):
	return A + (B - A) * t
