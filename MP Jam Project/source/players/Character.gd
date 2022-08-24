extends Actor

signal block_pickup_occured(picked_up_block_name)


signal is_aiming_changed(is_aiming)

signal look_input_changed(new_look_input)




var look_input : Vector2 = Vector2.ZERO setget look_input_set, look_input_get
var is_aiming : bool = false setget is_aiming_set, is_aiming_get



#------------------------------------
#abil

func is_aiming_get():
	return is_aiming

func is_aiming_set(aiming):
	if is_aiming != aiming:
		is_aiming = aiming
		emit_signal("is_aiming_changed", is_aiming)


#------------------------------------
#Look input

func look_input_set(look):
	if look != look_input:
		look_input = look
		emit_signal("look_input_changed", look_input)

func look_input_get():
	return look_input




func _ready():
	connect("is_aiming_changed", self, "_on_is_aiming")

func _on_is_aiming(is_aim):
	if is_aim:
		pass
	pass

func _on_look_input(look_input):
	pass


func try_interact():
	pass

func on_player_aim_input(aim_input : Vector2):
	self.aim_input = aim_input
	
func on_player_move_input(move_input : float):
	self.move_input = Vector2(move_input, 0)

func on_player_pressed_jump():
	self.jump_pressed = true
	self.jump_just_pressed = true
	
	pass

func on_player_released_jump():
	self.jump_pressed = false
	pass


func on_player_just_pressed_interact():
	#print("CharacterBase. %s pressed interact" % name)
	set_collision_layer_bit(5, true)


func on_player_released_interact():
	#print("CharacterBase.%s released interact" % name)
	set_collision_layer_bit(5, false)
	
