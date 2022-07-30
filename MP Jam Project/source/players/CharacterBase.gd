class_name CharacterBase
extends player_movement


enum AimDirection { FRONT, BELOW, ABOVE }

onready var front_aim_point = $"ControlPoints/Front"
onready var below_aim_point = $"ControlPoints/Below"
onready var above_aim_point = $"ControlPoints/Above"

# Called when the node enters the scene tree for the first time.
func _ready():
	pass # Replace with function body.


# Called every frame. 'delta' is the elapsed time since the previous frame.
#func _process(delta):
#	pass

func get_aim_position(aim_direction):
	match(aim_direction):
		AimDirection.FRONT:
			return front_aim_point.position
		AimDirection.BELOW:
			return below_aim_point.position
		AimDirection.ABOVE:
			return above_aim_point.position

#since the validity is dependent on the kind of action being performed this 
#function must be implemented in the child classes
func is_direction_valid(aim_direction) -> bool:
	return false
