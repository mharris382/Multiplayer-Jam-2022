extends Camera2D


# Declare member variables here. Examples:
# var a = 2
# var b = "text"
export var min_zoom = 1.0
export var max_zoom = 4.0
export var zoom_speed = 1.0
export var move_speed = Vector2(100, 100)
# Called when the node enters the scene tree for the first time.
func _ready():
	pass # Replace with function body.


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
		var move = Input.get_vector("move_left_p1", "move_right_p1", "move_up_p1", "move_down_p1")
		if not move.length_squared() <= 0:
			move.y *=-1
			var velocity = (move * delta) * move_speed
			var pos = self.position
			pos += velocity
			pos.x = clamp(pos.x , self.limit_left, self.limit_right)
			pos.y = clamp(pos.y, self.limit_top, self.limit_bottom)
			self.position = pos
		
		var zoomInput = Input.get_axis("move_down_p2", "move_up_p2");
		if zoomInput == 0:
			return
		zoomInput *= delta * zoom_speed
		var zm = zoom.x + zoomInput
		zm = clamp(zm, min_zoom, max_zoom)
		zoom = Vector2(zm, zm)
