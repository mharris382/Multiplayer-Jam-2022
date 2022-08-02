extends CharacterBase
export var throw_force : float = 100

onready var throw_origin : Node2D #these can be the same transform
onready var aim_transform : Node2D



func is_direction_valid(aim_direction) -> bool:
	match(aim_direction):
		AimDirection.BELOW:
			return !is_on_floor()
		AimDirection.ABOVE:
			return !is_on_ceiling()
		AimDirection.FRONT:
			return !is_on_floor()
	return true
	
func front_point_position():
	return front_aim_point.position

func pick_up_a_block():
	if Input.is_action_just_pressed("ability_%s" % player_id):
		var block = move_and_collide(front_point_position(), false, true, true)
		if block != null:
			if block.collider.is_in_group("dynamic_block"):
				var new_block = load("res://source/blocks/DynamicBlocks/PickedBlock.tscn").instance()
				block.collider.queue_free()
				front_aim_point.add_child(new_block)
			
func _input(event):
	pick_up_a_block()

func throw_a_block():
	var spawn_position = throw_origin.position
	var impulse_force = aim_transform.transform.xform(Vector2.RIGHT) * throw_force
	#get block PackedScene
	#instance block
	#apply force
