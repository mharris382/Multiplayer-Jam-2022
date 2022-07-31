extends CharacterBase


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
	return $ControlPoints/Front.position

func pick_up_a_block():
	if Input.is_action_just_pressed("ability_%s" % player_id):
		var block = move_and_collide(front_point_position(), false, true, true)
		if block != null:
			print(block.collider)
			if block.collider.is_in_group("dynamic_block"):
				print("We have it")
			else:
				print("We don't have it")
			
func _input(event):
	pick_up_a_block()
