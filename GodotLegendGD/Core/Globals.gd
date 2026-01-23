extends Node

# Global Singleton: Globals

var GameLoop = null
var RenderingService = null
var InputService = null
var SoundService = null
var SettingsService = null
var TimerService = null

# Game state managers
var myGameConditions: GameConditions = null
var myLayers: Layers = null

# Logging
func Debug(message: String):
	print("[DEBUG] ", message)

func Analytic(message: String):
	print("[ANALYTIC] ", message)

# Repository for Frame Descriptors
var frames: Array = []
var frames_mirror: Array = []

func _ready():
	# Initialize achievements list
	PoleGameAchievement.initialize()

	# Create game state managers
	myGameConditions = GameConditions.new()
	myLayers = Layers.new()

	# Initialize frame arrays
	frames.resize(Enums.GameBitmapEnumeration.bmpENDOFBITMAPS + 1)
	frames_mirror.resize(Enums.GameBitmapEnumeration.bmpENDOFBITMAPS + 1)

	# Initialize with empty FrameDescs (optional, or do on demand)
	for i in range(frames.size()):
		frames[i] = FrameDesc.new()
		frames_mirror[i] = FrameDesc.new()
