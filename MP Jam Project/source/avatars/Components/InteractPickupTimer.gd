extends Timer

# (prevents repeat button presses) 
# listens for an interact signal and starts the timer which will reset the interact button


func _on_Builder_try_pickup_blocks():
	paused = false
	start()
	
# in Jam project this was attached in the Character scene, however I think we might be able to use it in the Player.gd instead (would need some kind of modification or given a scene)
