extends Node

onready var music_main_menu = load("res://scenes/Audio/Music/Music_Main_Menu.tscn")
onready var music_lobby = load("res://scenes/Audio/Music/Music_Lobby.tscn")

onready var puzzle_feedback_good = preload("res://scenes/Audio/Sounds/Feedbacks/Feedback_Puzzle_Progress_Good.tscn")
onready var puzzle_feedback_bad = preload("res://scenes/Audio/Sounds/Feedbacks/Feedback_Puzzle_Progress_Bad.tscn")
onready var puzzle_feedback_complete = preload("res://scenes/Audio/Sounds/Feedbacks/Feedback_Puzzle_Completed.tscn")

onready var block_feedback_pickup = preload("res://scenes/Audio/Sounds/Feedbacks/Feedback_Block_Pickup.tscn")
onready var block_feedback_build = preload("res://scenes/Audio/Sounds/Feedbacks/Feedback_Block_Build.tscn")
onready var block_feedback_disconnect = preload("res://scenes/Audio/Sounds/Feedbacks/Feedback_Block_Disconnect.tscn")
onready var block_feedback_invalid = preload("res://scenes/Audio/Sounds/Feedbacks/Feedback_Block_Invalid.tscn")

var feedbacks = {}
var music = {}
var current_music_player : AudioStreamPlayer
var next_music_player  : AudioStreamPlayer

var current_ambient_player : AudioStreamPlayer
var next_ambient_player  : AudioStreamPlayer

func change_music_track(music_track, transition_time):
	var audio_stream = get_audio_stream(music_track)
	print("TODO: implement AudioManager.change_music_track") #print("TODO: implement AudioManager.")

func play_feedback(feedback):
	print("TODO: implement AudioManager.player_feedback")
	var stream = get_feedbac(feedback).instance()
	add_child(stream)
	stream.playing = true
	stream.play()

func change_ambient_track(ambient_track, transition_time):
	print("TODO: implement AudioManager.change_ambient_track transition time")
	var instance = ambient_track.instance()
	add_child(instance)
	instance.playing = true
	instance.play()
	pass
func get_feedbac(audio):
	match audio:
		AudioEnums.AudioFeedbacks.BLOCK_BUILD:
			return block_feedback_build
		AudioEnums.AudioFeedbacks.BLOCK_DICONNECT:
			return block_feedback_disconnect
		AudioEnums.AudioFeedbacks.BLOCK_PICKUP:
			return block_feedback_pickup
		AudioEnums.AudioFeedbacks.BLOCK_INVALID:
			return block_feedback_invalid

		AudioEnums.AudioFeedbacks.PUZZLE_BAD:
			return puzzle_feedback_bad
		AudioEnums.AudioFeedbacks.PUZZLE_GOOD:
			return puzzle_feedback_good
		AudioEnums.AudioFeedbacks.PUZZLE_COMPLETE:
			return puzzle_feedback_complete
func get_audio_stream(audio):
	if audio is AudioStream:
		return audio
	match audio:
		AudioEnums.MusicTracks.LOBBY:
			return music_lobby
		AudioEnums.MusicTracks.MAIN_MENU:
			return music_main_menu
		
		AudioEnums.AudioFeedbacks.BLOCK_BUILD:
			return block_feedback_build
		AudioEnums.AudioFeedbacks.BLOCK_DICONNECT:
			return block_feedback_disconnect
		AudioEnums.AudioFeedbacks.BLOCK_PICKUP:
			return block_feedback_pickup
		AudioEnums.AudioFeedbacks.BLOCK_INVALID:
			return block_feedback_invalid

		AudioEnums.AudioFeedbacks.PUZZLE_BAD:
			return puzzle_feedback_bad
		AudioEnums.AudioFeedbacks.PUZZLE_GOOD:
			return puzzle_feedback_good
		AudioEnums.AudioFeedbacks.PUZZLE_COMPLETE:
			return puzzle_feedback_complete
	return null
