extends Control

#time to fade out current track and fade in next track
export var music_transition_time = 2.0

# Called when the node enters the scene tree for the first time.
func _ready():
	pass # Replace with function body.




func _on_Feedback_Completed_Button_button_down():
	AudioManager.play_feedback(AudioEnums.AudioFeedbacks.PUZZLE_COMPLETE)

func _on_Feedback_Bad_Button_button_down():
	AudioManager.play_feedback(AudioEnums.AudioFeedbacks.PUZZLE_BAD)


func _on_Feedback_Build_Button_button_down():
	AudioManager.play_feedback(AudioEnums.AudioFeedbacks.BLOCK_BUILD)


func _on_Music_Lobby_Button_button_down():
	AudioManager.change_music_track(AudioEnums.MusicTracks.LOBBY, music_transition_time)
	
	# the code above is identical to the following lines (prefer more explict calls)
	# AudioManager.get_audio_stream(AudioManager.MusicTracks.LOBBY, music_transition_time)
	# AudioManager.change_music_track(AudioManager.music_lobby, music_transition_time)
	# AudioManager.change_music_track(load("res://scenes/Audio/Music/Music_Lobby.tscn"), music_transition_time)


func _on_Feedback_Ambient_Button_button_down():
	AudioManager.change_ambient_track(load("res://scenes/Audio/Sounds/Ambient/SFX_Ambient.tscn"), 1)


func _on_Feedback_Good_Button_button_down():
	AudioManager.play_feedback(AudioEnums.AudioFeedbacks.PUZZLE_GOOD)


func _on_Feedback_Pickup_Button_button_down():
	AudioManager.play_feedback(AudioEnums.AudioFeedbacks.BLOCK_PICKUP)


func _on_Feedback_Invalid_Button_button_down():
	AudioManager.play_feedback(AudioEnums.AudioFeedbacks.BLOCK_INVALID)
