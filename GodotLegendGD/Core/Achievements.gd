class_name PoleGameAchievement

# PoleGameAchievement.gd - Achievement system
# Ported from Achievements.cs

var achievement_guid: int
var achieved_code: int
var achievement_name: String
var description: String
var value: int
var secret: bool
var achieved: bool = false
var when_achieved: float = 0.0  # Unix timestamp


func _init(p_achievement_guid: int, p_achieved_code: int, p_name: String,
		p_value: int, p_secret: bool, p_description: String) -> void:
	achievement_guid = p_achievement_guid
	achieved_code = p_achieved_code
	achievement_name = p_name
	description = p_description
	value = p_value
	secret = p_secret


# Static achievement list - initialized once
static var list: Array[PoleGameAchievement] = []
static var _initialized: bool = false


static func initialize() -> void:
	if _initialized:
		return
	_initialized = true

	# Show Some Discipline
	list.append(PoleGameAchievement.new(2002, 3242, "Show Some Discipline", 10, false,
		"Proudly display a Discipline bar on your Engineering jacket."))

	# Desperate Times, Desperate Measures
	list.append(PoleGameAchievement.new(543, 5324, "Desperate Times, Desperate Measures", 10, false,
		"Wind up your arm fully, completely before you toss a road apple."))

	# It's The Jam! It's All Good For You
	list.append(PoleGameAchievement.new(98, 12354, "It's The Jam! It's All Good For You", 20, false,
		"Offer 'za or a drink to the Engineering Society President."))

	# You're a Hoser
	list.append(PoleGameAchievement.new(69, 6542, "You're a Hoser", 20, false,
		"Cool down the frosh with water from the firehose."))

	# Like Homecoming, but with Lanolin
	list.append(PoleGameAchievement.new(2000, 562, "Like Homecoming, but with Lanolin", 50, false,
		"Quench the crowd's thirst 'til they slam their leather jackets."))

	# Pole in Ten (Years!)
	list.append(PoleGameAchievement.new(5, 52, "Pole in Ten (Years!)", 100, false,
		"Stall the frosh for at least 10 minutes."))

	# Golden Soda
	list.append(PoleGameAchievement.new(1999, 373, "Golden Soda", 50, false,
		"Offer a drink to hard-working Al 'Pop Boy' Burchell."))

	# Dizzying Heights
	list.append(PoleGameAchievement.new(747, 474, "Dizzying Heights", 50, false,
		"Send a frosh flying from tam to pit-water."))

	# Exam Avoidance
	list.append(PoleGameAchievement.new(13, 90210, "Exam Avoidance", 50, false,
		"Stall the frosh for five minutes without lobbing a physics 'smart bomb'."))

	# Fully Loaded Fun Fur
	list.append(PoleGameAchievement.new(399, 9999991, "Fully Loaded Fun Fur", 70, false,
		"Stuff your pockets with 99 apples, 99 slices of 'za, or 99 Clark mugs."))

	# Iron Ring Ceremony
	list.append(PoleGameAchievement.new(7, 62463, "Iron Ring Ceremony", 100, false,
		"Unleash the power of the mighty Iron Ring."))

	# Double Fisting
	list.append(PoleGameAchievement.new(7777, 32592, "Double Fisting", 150, false,
		"Wear two Iron Rings at the same time!"))

	# === SECRET ACHIEVEMENTS ===

	# Hungry Hungry Hippo
	list.append(PoleGameAchievement.new(96, 13312, "Hungry Hungry Hippo", 50, true,
		"Feed anything to the Golden Words Hippo. He's not fussy."))

	# How do you like THEM Apples?
	list.append(PoleGameAchievement.new(99, 5676, "How do you like THEM Apples?", 100, true,
		"Toss an apple at an artsci or commie in the pit."))

	# You messed up!
	list.append(PoleGameAchievement.new(11, 24601, "You messed up!", 20, true,
		"Receive a Tri-Pub Ban - a dubious distiction at best!"))

	# Work Hard, Party Harder
	list.append(PoleGameAchievement.new(50, 3423, "Work Hard, Party Harder", 50, true,
		"Quaff some golden soda. It'd be rude not to."))


# Helper to find achievement by GUID
static func find_by_guid(guid: int) -> PoleGameAchievement:
	for achievement in list:
		if achievement.achievement_guid == guid:
			return achievement
	return null


# Helper to unlock achievement by GUID
static func unlock(guid: int) -> bool:
	var achievement := find_by_guid(guid)
	if achievement != null and not achievement.achieved:
		achievement.achieved = true
		achievement.when_achieved = Time.get_unix_time_from_system()
		return true
	return false


# Get total achievement points earned
static func get_total_points() -> int:
	var total: int = 0
	for achievement in list:
		if achievement.achieved:
			total += achievement.value
	return total


# Get total possible achievement points
static func get_max_points() -> int:
	var total: int = 0
	for achievement in list:
		total += achievement.value
	return total
