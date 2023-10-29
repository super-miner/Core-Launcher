extends Node

func run_game() -> void:
	OS.execute(ProjectSettings.globalize_path("res://") + "/Commands/RunGame.bat", [])
