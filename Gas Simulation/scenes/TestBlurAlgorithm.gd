extends Node


# Declare member variables here. Examples:
# var a = 2
# var b = "text"
onready var blur = $"Blur Algorithm"

# Called when the node enters the scene tree for the first time.
func _ready():
	var test_input = Vector2(0,0)
	for x in range(0, 4):
		for y in range(0, 4):
			var result = blur._get_possible_blocked_directions(x,y)
			print("For ", x, ", ", y, " => ", result ," mask")
			var dirs = blur._bitmask_to_vector_array(result)
	var tilemap =blur._gasTileMap

# Called every frame. 'delta' is the elapsed time since the previous frame.
#func _process(delta):
#	pass
