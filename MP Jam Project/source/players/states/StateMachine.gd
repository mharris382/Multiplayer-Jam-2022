class_name StateMachine
extends Node

signal state_changed(state_name)

export var initial_state := NodePath()

onready var state:State = get_node(initial_state)

func _ready():
	yield(owner, "ready")
	for child in get_children():
		child.state_machine = self
	state.enter()

