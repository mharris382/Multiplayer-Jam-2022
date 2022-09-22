tool
extends ParallaxBackground


export var furthest_layer = Vector2(0.1, 0.1)
export var closest_layer = Vector2(2,2)
export var power = Vector2(2, 2)

# Called when the node enters the scene tree for the first time.
func _ready():
	pass # Replace with function body.

func _process(delta):
	var child_cnt = get_child_count()
	var cnt = 0
	for child in get_children():
		if child is ParallaxLayer:
			var pl = child as ParallaxLayer
			cnt+=1
			var t = cnt / float(child_cnt)
			var tx = pow(t, power.x)
			var ty = pow(t, power.y)
			var rx = lerp(closest_layer.x, furthest_layer.x, tx)
			var ry = lerp(closest_layer.y, furthest_layer.y, ty)
			pl.motion_scale = Vector2(rx, ry)
	
