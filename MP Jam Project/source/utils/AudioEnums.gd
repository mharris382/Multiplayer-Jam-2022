class_name AudioEnums

#integers are declared so there isn't any overlap in func AudioManager.get_named_audio_stream(enum)

enum AudioFeedbacks {
	BLOCK_BUILD = 1,
	BLOCK_DICONNECT = 2,
	BLOCK_PICKUP=3,
	BLOCK_INVALID=4,
	PUZZLE_GOOD=5,
	PUZZLE_BAD=6,
	PUZZLE_COMPLETE=7
}

enum MusicTracks {
	LOBBY= 101,
	MAIN_MENU = 102
}
