class_name AIWaterEffects

# aiWaterEffects.gd - Static class with water effect animations
# Ported from aiWaterEffects.cs
#
# Per porting_rules.md:
# - This is a static class with logic functions only
# - All shared state accessed via AIMethods autoload
# - Frame access via AIMethods.frm and AIMethods.frm_m

# === SPLASH ANIMATIONS ===

static func ai_splash_m(s: TSprite) -> void:
	# Animates a medium-sized splash, then deletes the associated sprite
	if s.n_cc < 12:
		s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpSPLASHM1 + (s.n_cc / 4)])
	else:
		s.b_deleted = true


static func ai_splash_ml(s: TSprite) -> void:
	# Animates a medium-sized left-oriented splash, then deletes the associated sprite
	if s.n_cc < 12:
		s.set_frame(AIMethods.frm_m[Enums.GameBitmapEnumeration.bmpSPLASHM1 + (s.n_cc / 4)])
	else:
		s.b_deleted = true


static func ai_splash_l(s: TSprite) -> void:
	# Animates a large splash, then deletes the associated sprite
	if s.n_cc < 12:
		s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpSPLASHL1 + (s.n_cc / 4)])
	else:
		s.b_deleted = true


static func ai_splash_s(s: TSprite) -> void:
	# Animates a small splash, then deletes the associated sprite
	if s.n_cc < 12:
		s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpSPLASHS1 + (s.n_cc / 4)])
	else:
		s.b_deleted = true


# === RIPPLE ANIMATION ===

static func ai_ripple(s: TSprite) -> void:
	# Animates a ripple, then deletes the associated sprite
	if s.n_cc < 9:
		s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpRIPPLE1 + (s.n_cc / 3)])
	else:
		s.b_deleted = true
