extends Node
#class_name StateMachine
#extends Node
#
#
#signal state_changed(current_state)
#
#signal player_input_jump(jump_input)
#signal player_input_aim(current_aim, last_non_zero_aim)
#signal player_input_move(move_input)
#
#
#
##bit pattern
## first bit is if the button is down or up
## second bit is if the button changed this frame
#enum BUTTON_INPUT_STATE { 
#	RELEASED = 0b00, 
#	PRESSED = 0b01, 
#	JUST_PRESSED = 0b11, 
#	JUST_RELEASED = 0b10
#}
#
#
## You should set a starting node from the inspector or on the node that inherits
## from this state machine interface. If you don't, the game will default to
## the first state in the state machine's children.
#export(NodePath) var start_state
#
#
#var states_map = {}
#
#var states_stack = []
#var current_state :State  = null
#var _active = false setget set_active
#var player
#
#var last_non_zero_aim
#
#
#
#onready var null_state = $NullState
#
#
#
#
#func _ready():
#	var player_object = get_parent() as Player
#	if player_object == null:
#		player_object = self
#	if not start_state:
#		start_state = get_child(0).get_path()
#	for child in get_children():
#		var err = child.connect("finished", self, "_change_state")
#		child.player = player_object
#		if err:
#			printerr(err)
#	initialize(start_state)
#
#
#func initialize(initial_state):
#	set_active(true)
#	states_stack.push_front(get_node(initial_state))
#	current_state = states_stack[0]
#	current_state.enter()
#
#
#func set_active(value):
#	_active = value
#	set_physics_process(value)
#	set_process_input(value)
#	if not _active:
#		states_stack = []
#		current_state = null
#
#
#func _unhandled_input(event):
#	if player == null:
#		print("No player found")
#		return
#	current_state.handle_input(event)
#
#
#func _physics_process(delta):
#	if player == null:
#		print("No player found")
#		return
#	var behaviour=current_state.update_physics(delta)
#
#func _process(delta):
#	if player == null:
#		print("No player found")
#		return
#
#	var jump_button_state = _get_jump_input()
#	var interact_button_state = _get_interact_input()
#
#	var behaviour=current_state.update(delta)
#
#
#func _on_animation_finished(anim_name):
#	if not _active:
#		return
#	current_state._on_animation_finished(anim_name)
#
#
#func _change_state(state_name):
#	if not _active:
#		return
#	current_state.exit()
#
#	match state_name:
#		"previous":
#			states_stack.pop_front()
#		"null", null:
#			pass
#		_:
#			pass
#	if state_name == "previous":
#		states_stack.pop_front()
#	else:
#		states_stack[0] = states_map[state_name]
#
#	current_state = states_stack[0]
#	emit_signal("state_changed", current_state)
#
#	if state_name != "previous":
#		current_state.enter()
#
##this method is here so the StateMachine can use itself as a null object in the case where player is missing from the parent
#func get_player_specific_input_action(action):
#	if player != null:
#		return player.get_player_specific_input_action(action)
#	return "%s_p%d"%[action, 1]
#
#func get_next_state():
#	if states_stack.size() <= 1:
#		return null_state
#	return states_stack[1]
#
#
#func _get_aim_input():
#	pass
#
#func _get_jump_input():
#	if current_state != null and current_state.has_method("get_jump_input"):
#		return current_state.get_jump_input()
#
#	var action = get_player_specific_input_action("jump")
#	var input = _get_action_input(action)
#
#	var state_input_override = current_state.override_jump_input(input)
#	if state_input_override != -1:
#		return state_input_override
#
#
#
#func _get_interact_input():
#	if current_state == null:
#		return -1
#
#	var action = get_player_specific_input_action("interact")
#	var input = _get_action_input(action)
#
#	var state_input_override = current_state.override_ability_input(input)
#	if state_input_override != -1:
#		return state_input_override
#
#
#
#func _get_ability_input():
#	if current_state == null:
#		return -1
#
#	var action = get_player_specific_input_action("ability")
#
#	var state_input_override = current_state.override_ability_input(action)
#	if state_input_override != -1:
#		return state_input_override
#
#	return _get_action_input(action)
#
#func _get_action_input(action):
#	if Input.is_action_just_pressed(action):
#		return BUTTON_INPUT_STATE.JUST_PRESSED
#	elif Input.is_action_just_released(action):
#		return BUTTON_INPUT_STATE.JUST_RELEASED
#	elif Input.is_action_pressed(action):
#		return BUTTON_INPUT_STATE.PRESSED
#	else:
#		return BUTTON_INPUT_STATE.RELEASED
#
#
#
##only called when player provides aim input, and will always be a non-zero value
#func _on_Player_aim_angle_changed(aim_direction : Vector2, aim_angle):
#	last_non_zero_aim = aim_direction
#	assert(aim_direction.length_squared() != 0)
#
##called on every frame regardless of whether aim vector is zero
#func _on_Player_aim_input(aim_direction):
#	emit_signal("player_input_aim", aim_direction, last_non_zero_aim)
#
#
#func _on_Player_move_changed(move_direction):
#	emit_signal("player_input_move", move_direction)
