extends Timer


export(int, 1, 32) var iterations_per_sec = 4


func _start_timer():
	.start(1 / iterations_per_sec)
