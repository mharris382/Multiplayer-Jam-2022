extends State

export var num = 1

func handle_input(_event: InputEvent) -> void:
	if Input.is_action_just_pressed("jump_p1"):
		state_next()

func enter(_msg := {}) -> void:
	print("I am in a state named: ", name)

func exit() -> void:
	print("I did leave state: ", name)

func state_next():
	state_machine.transition_to("State%d" % num)
