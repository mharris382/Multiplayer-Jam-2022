class_name StateMachine
extends Node

export var initial_state := NodePath()
var character : Character


onready var state = get_node(initial_state)


func _ready() -> void:
	yield(owner, "ready")
	print("state machine ready")
	for child in get_children():
		child.state_machine = self
	state.enter()
	character = owner as Character

func _unhandled_input(event: InputEvent) -> void:
	state.handle_input(event)


func _process(delta: float) -> void:
	state.update(delta)


func _physics_process(delta: float) -> void:
	state.physics_update(delta)


func transition_to(target_state_name: String, msg: Dictionary = {}) -> void:
	if not has_node(target_state_name):
		return

	state.exit()
	state = get_node(target_state_name)
	state.enter(msg)
	print("Transitioned to: ", state.name)
