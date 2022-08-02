extends CharacterBase
export var throw_force : float = 100
export (PackedScene) var test_block

var can_throw = false

onready var throw_origin : Node2D #these can be the same transform
onready var aim_transform : Position2D = $ControlPoints/Aim

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
	if Input.is_action_just_pressed("interact_%s" % player_id):
		var block = move_and_collide(front_point_position(), false, true, true)
		if block != null:
			if block.collider.is_in_group("dynamic_block"):
				var new_block = load("res://source/blocks/DynamicBlocks/PickedBlock.tscn").instance()
				block.collider.queue_free()
				front_aim_point.add_child(new_block)
				can_throw = true
			
func _input(event):
	pick_up_a_block()
	throw_a_block()

func throw_a_block():
	if can_throw and Input.is_action_just_pressed("ability_%s" % player_id):
		#var spawn_position = throw_origin.position
		var impulse_force = aim_transform.transform.xform(Vector2.RIGHT) * throw_force
		var new_block = test_block.instance()
		get_parent().add_child(new_block)
		new_block.set("should_teleport", true)
		new_block.set("node_pos", to_global(front_aim_point.position))
		front_aim_point.get_child(0).queue_free()
		can_throw = false
	#get block PackedScene
	#instance block
	#apply force
