extends Line2D

var deltaAngle = .1
var angle = 1.2
var length = 120
func _process(delta):
	angle += (delta * deltaAngle)
	var dir = transform.rotated(angle)
	var p0 = position
	var p1 = p0 + (dir.xform(Vector2.RIGHT) * length)
	points[0] = p0
	points[1] = p1
