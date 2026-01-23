# GitHub Check-in Manifest

**Generated:** January 23, 2026
**Repository:** https://github.com/robburke/greasepole
**Total Files:** 2,720

---

## Project Overview

This repository contains three implementations of "Legend of the Greasepole":

1. **SilverLegend/** - Original Silverlight 4 implementation (canonical reference)
2. **GodotLegend/** - Godot 4 with C# port
3. **GodotLegendGD/** - Godot 4 with GDScript (web-ready version)

---

## Summary by Project

| Project | Files | Description |
|---------|-------|-------------|
| SilverLegend/ | 924 | Original Silverlight 4 - canonical game logic reference |
| GodotLegend/ | 905 | Godot 4 C# port - initial migration |
| GodotLegendGD/ | 882 | Godot 4 GDScript - web export version |
| docs/ | 6 | Migration documentation |
| Root | 3 | .gitignore, Pillarbox.jpg, .vscode/settings.json |

---

## What's Excluded (.gitignore)

```
# IDE / Editor
.vs/
*.suo
*.user
*.csproj.user
.idea/
.claude/

# C# / .NET Build Artifacts
bin/
obj/
pub_output/
pub_self_contained/
pub_self_cotained/
*.dll
*.pdb
*.exe

# Godot
.godot/
*.import

# Export templates
templates*.zip
temp_templates/
temp_templates_*/

# Build Outputs
GodotLegendGD/builds/
GodotLegend/exports/

# Deploy Scripts (private)
GodotLegendGD/deploy/

# Silverlight Build Artifacts
SilverLegend/**/bin/
SilverLegend/**/obj/
SilverLegend/**/ClientBin/
SilverLegend/**/*.xap

# OS Generated
.DS_Store
Thumbs.db
Desktop.ini

# Misc
*.log
*.tmp
*.bak
```

---

## Root Level Files (3)

```
.gitignore
.vscode/settings.json
Pillarbox.jpg
```

---

## docs/ (6 files)

Migration documentation preserved for posterity:

```
docs/migration/MIGRATION_PLAN.md
docs/migration/layer_rendering_analysis.md
docs/migration/porting_analysis.md
docs/migration/porting_rules.md
docs/migration/sound_backlog.md
docs/migration/sound_comparison_audit_report.md
```

---

## GodotLegend/ - C# Version (905 files)

### Source Code (95 files)

```
GodotLegend/Core/AI/AIDefine.cs
GodotLegend/Core/AI/AIDefine.cs.uid
GodotLegend/Core/AI/aiBackground.cs
GodotLegend/Core/AI/aiBackground.cs.uid
GodotLegend/Core/AI/aiCrowd.cs
GodotLegend/Core/AI/aiCrowd.cs.uid
GodotLegend/Core/AI/aiEnums.cs
GodotLegend/Core/AI/aiEnums.cs.uid
GodotLegend/Core/AI/aiFrosh.cs
GodotLegend/Core/AI/aiFrosh.cs.uid
GodotLegend/Core/AI/aiMenuAndDisplay.cs
GodotLegend/Core/AI/aiMenuAndDisplay.cs.uid
GodotLegend/Core/AI/aiMisc.cs
GodotLegend/Core/AI/aiMisc.cs.uid
GodotLegend/Core/AI/aiPopUp.cs
GodotLegend/Core/AI/aiPopUp.cs.uid
GodotLegend/Core/AI/aiProjectile.cs
GodotLegend/Core/AI/aiProjectile.cs.uid
GodotLegend/Core/AI/aiSupport.cs
GodotLegend/Core/AI/aiSupport.cs.uid
GodotLegend/Core/AI/aiWaterEffects.cs
GodotLegend/Core/AI/aiWaterEffects.cs.uid
GodotLegend/Core/AI/poleposn.cs
GodotLegend/Core/AI/poleposn.cs.uid
GodotLegend/Core/Achievements.cs
GodotLegend/Core/Achievements.cs.uid
GodotLegend/Core/Control/GameConditions.cs
GodotLegend/Core/Control/GameConditions.cs.uid
GodotLegend/Core/Control/GameLoop.cs
GodotLegend/Core/Control/GameLoop.cs.uid
GodotLegend/Core/Control/Layers.cs
GodotLegend/Core/Control/Layers.cs.uid
GodotLegend/Core/Control/SoundbankInfo.cs
GodotLegend/Core/Control/SoundbankInfo.cs.uid
GodotLegend/Core/GameBitmapEnumeration.cs
GodotLegend/Core/GameBitmapEnumeration.cs.uid
GodotLegend/Core/Globals.cs
GodotLegend/Core/Globals.cs.uid
GodotLegend/Core/IGameTimerService.cs
GodotLegend/Core/IGameTimerService.cs.uid
GodotLegend/Core/IInputService.cs
GodotLegend/Core/IInputService.cs.uid
GodotLegend/Core/IRenderingService.cs
GodotLegend/Core/IRenderingService.cs.uid
GodotLegend/Core/ISettingPersistanceService.cs
GodotLegend/Core/ISettingPersistanceService.cs.uid
GodotLegend/Core/ISoundService.cs
GodotLegend/Core/ISoundService.cs.uid
GodotLegend/Core/ServiceFactory.cs
GodotLegend/Core/ServiceFactory.cs.uid
GodotLegend/Core/Sprite/FrameDesc.cs
GodotLegend/Core/Sprite/FrameDesc.cs.uid
GodotLegend/Core/Sprite/SpriteInit.cs
GodotLegend/Core/Sprite/SpriteInit.cs.uid
GodotLegend/Core/Sprite/SpriteSet.cs
GodotLegend/Core/Sprite/SpriteSet.cs.uid
GodotLegend/Core/Sprite/TSprite.cs
GodotLegend/Core/Sprite/TSprite.cs.uid
GodotLegend/GodotLegend.csproj
GodotLegend/GodotLegend.csproj.old
GodotLegend/GodotLegend.sln
GodotLegend/Scenes/Main.tscn
GodotLegend/Scenes/WindowWrapper.tscn
GodotLegend/Scripts/Main.cs
GodotLegend/Scripts/Main.cs.uid
GodotLegend/Scripts/WindowWrapper.cs
GodotLegend/Scripts/WindowWrapper.cs.uid
GodotLegend/Services/BitmapLoadInfo.cs
GodotLegend/Services/BitmapLoadInfo.cs.uid
GodotLegend/Services/BitmapRegistry.cs
GodotLegend/Services/BitmapRegistry.cs.uid
GodotLegend/Services/BitmapRegistryData.txt
GodotLegend/Services/GameBitmapData.txt
GodotLegend/Services/GameBitmapDataClean.txt
GodotLegend/Services/GodotInputService.cs
GodotLegend/Services/GodotInputService.cs.uid
GodotLegend/Services/GodotRenderingService.cs
GodotLegend/Services/GodotRenderingService.cs.uid
GodotLegend/Services/GodotServiceFactory.cs
GodotLegend/Services/GodotServiceFactory.cs.uid
GodotLegend/Services/GodotSettingsService.cs
GodotLegend/Services/GodotSettingsService.cs.uid
GodotLegend/Services/GodotSound.cs
GodotLegend/Services/GodotSound.cs.uid
GodotLegend/Services/GodotSoundService.cs
GodotLegend/Services/GodotSoundService.cs.uid
GodotLegend/Services/GodotTimerService.cs
GodotLegend/Services/GodotTimerService.cs.uid
GodotLegend/Shaders/color_replace.gdshader
GodotLegend/Shaders/color_replace.gdshader.uid
GodotLegend/export_presets.cfg
GodotLegend/favicon.ico
GodotLegend/icon.svg
GodotLegend/project.godot
```

### Assets (810 files)

- **Graphics:** 530 PNG files in `GodotLegend/Assets/Graphics/`
- **Sound:** 280 MP3 files in `GodotLegend/Assets/Sound/`

---

## GodotLegendGD/ - GDScript Version (882 files)

### Source Code (72 files)

```
GodotLegendGD/Core/AI/AIDefine.gd
GodotLegendGD/Core/AI/AIDefine.gd.uid
GodotLegendGD/Core/AI/AIFlyInAndOut.gd
GodotLegendGD/Core/AI/AIFlyInAndOut.gd.uid
GodotLegendGD/Core/AI/AIFrosh.gd
GodotLegendGD/Core/AI/AIFrosh.gd.uid
GodotLegendGD/Core/AI/AIMethods.gd
GodotLegendGD/Core/AI/AIMethods.gd.uid
GodotLegendGD/Core/AI/AISupport.gd
GodotLegendGD/Core/AI/AISupport.gd.uid
GodotLegendGD/Core/AI/PolePosition.gd
GodotLegendGD/Core/AI/PolePosition.gd.uid
GodotLegendGD/Core/AI/aiBackground.gd
GodotLegendGD/Core/AI/aiBackground.gd.uid
GodotLegendGD/Core/AI/aiCrowd.gd
GodotLegendGD/Core/AI/aiCrowd.gd.uid
GodotLegendGD/Core/AI/aiMenuAndDisplay.gd
GodotLegendGD/Core/AI/aiMenuAndDisplay.gd.uid
GodotLegendGD/Core/AI/aiMisc.gd
GodotLegendGD/Core/AI/aiMisc.gd.uid
GodotLegendGD/Core/AI/aiPopUp.gd
GodotLegendGD/Core/AI/aiPopUp.gd.uid
GodotLegendGD/Core/AI/aiProjectile.gd
GodotLegendGD/Core/AI/aiProjectile.gd.uid
GodotLegendGD/Core/AI/aiWaterEffects.gd
GodotLegendGD/Core/AI/aiWaterEffects.gd.uid
GodotLegendGD/Core/AIMethods.gd.uid
GodotLegendGD/Core/Achievements.gd
GodotLegendGD/Core/Achievements.gd.uid
GodotLegendGD/Core/Control/GameConditions.gd
GodotLegendGD/Core/Control/GameConditions.gd.uid
GodotLegendGD/Core/Control/GameLoop.gd
GodotLegendGD/Core/Control/GameLoop.gd.uid
GodotLegendGD/Core/Control/Layers.gd
GodotLegendGD/Core/Control/Layers.gd.uid
GodotLegendGD/Core/Control/SoundbankInfo.gd
GodotLegendGD/Core/Control/SoundbankInfo.gd.uid
GodotLegendGD/Core/Enums.gd
GodotLegendGD/Core/Enums.gd.uid
GodotLegendGD/Core/FrameDesc.gd
GodotLegendGD/Core/FrameDesc.gd.uid
GodotLegendGD/Core/Globals.gd
GodotLegendGD/Core/Globals.gd.uid
GodotLegendGD/Core/Sprite/SpriteInit.gd
GodotLegendGD/Core/Sprite/SpriteInit.gd.uid
GodotLegendGD/Core/Sprite/SpriteSet.gd
GodotLegendGD/Core/Sprite/SpriteSet.gd.uid
GodotLegendGD/Core/Sprite/TSprite.gd
GodotLegendGD/Core/Sprite/TSprite.gd.uid
GodotLegendGD/Scenes/WindowWrapper.gd
GodotLegendGD/Scenes/WindowWrapper.gd.uid
GodotLegendGD/Scenes/WindowWrapper.tscn
GodotLegendGD/Services/BitmapRegistry.gd
GodotLegendGD/Services/BitmapRegistry.gd.uid
GodotLegendGD/Services/InputService.gd
GodotLegendGD/Services/InputService.gd.uid
GodotLegendGD/Services/RenderingService.gd
GodotLegendGD/Services/RenderingService.gd.uid
GodotLegendGD/Services/SettingsService.gd
GodotLegendGD/Services/SettingsService.gd.uid
GodotLegendGD/Services/SoundService.gd
GodotLegendGD/Services/SoundService.gd.uid
GodotLegendGD/Services/TimerService.gd
GodotLegendGD/Services/TimerService.gd.uid
GodotLegendGD/Shaders/color_replace.gdshader
GodotLegendGD/Shaders/color_replace.gdshader.uid
GodotLegendGD/export/post_export.bat
GodotLegendGD/export/web_shell.html
GodotLegendGD/export_presets.cfg
GodotLegendGD/icon.svg
GodotLegendGD/project.godot
```

### Assets (810 files)

- **Graphics:** 530 PNG files in `GodotLegendGD/Assets/Graphics/`
- **Sound:** 280 MP3 files in `GodotLegendGD/Assets/Sound/`

---

## SilverLegend/ - Silverlight Original (924 files)

### Source Code & Config (109 files)

```
SilverLegend/SilverLegend.sln
SilverLegend/SilverLegend.Web/AnalyticsModel.Designer.cs
SilverLegend/SilverLegend.Web/AnalyticsModel.edmx
SilverLegend/SilverLegend.Web/AnalyticsSvc.svc
SilverLegend/SilverLegend.Web/AnalyticsSvc.svc.cs
SilverLegend/SilverLegend.Web/AnalyticsUtil.cs
SilverLegend/SilverLegend.Web/Default.aspx
SilverLegend/SilverLegend.Web/Default.aspx.cs
SilverLegend/SilverLegend.Web/Default.aspx.designer.cs
SilverLegend/SilverLegend.Web/LeaderboardModel.Designer.cs
SilverLegend/SilverLegend.Web/LeaderboardModel.edmx
SilverLegend/SilverLegend.Web/LeaderboardSvc.svc
SilverLegend/SilverLegend.Web/LeaderboardSvc.svc.cs
SilverLegend/SilverLegend.Web/Properties/AssemblyInfo.cs
SilverLegend/SilverLegend.Web/SilverLegend.Web.Publish.xml
SilverLegend/SilverLegend.Web/SilverLegend.Web.csproj
SilverLegend/SilverLegend.Web/Silverlight.js
SilverLegend/SilverLegend.Web/SplashScreen.js
SilverLegend/SilverLegend.Web/SplashScreen.xaml
SilverLegend/SilverLegend.Web/Test.aspx
SilverLegend/SilverLegend.Web/Test.aspx.cs
SilverLegend/SilverLegend.Web/Test.aspx.designer.cs
SilverLegend/SilverLegend.Web/Web.Debug.config
SilverLegend/SilverLegend.Web/Web.Release.config
SilverLegend/SilverLegend.Web/Web.config
SilverLegend/SilverLegend.Web/clientaccesspolicy.xml
SilverLegend/SilverLegend.Web/images/...
SilverLegend/SilverLegend.Web/indexSimple.html
SilverLegend/SilverLegend/App.xaml
SilverLegend/SilverLegend/App.xaml.cs
SilverLegend/SilverLegend/CustomControls/FullScreenControl.xaml
SilverLegend/SilverLegend/CustomControls/FullScreenControl.xaml.cs
SilverLegend/SilverLegend/CustomControls/MuteControl.xaml
SilverLegend/SilverLegend/CustomControls/MuteControl.xaml.cs
SilverLegend/SilverLegend/Images/...
SilverLegend/SilverLegend/Page.xaml
SilverLegend/SilverLegend/Page.xaml.cs
SilverLegend/SilverLegend/Properties/...
SilverLegend/SilverLegend/Service References/...
SilverLegend/SilverLegend/ServiceReferences.ClientConfig
SilverLegend/SilverLegend/Services/GameSettingsServiceSilverlight.cs
SilverLegend/SilverLegend/Services/GameTimerServiceSilverlight.cs
SilverLegend/SilverLegend/Services/InputServiceSilverlight.cs
SilverLegend/SilverLegend/Services/RenderingServiceSilverlight.cs
SilverLegend/SilverLegend/Services/ServiceFactorySilverlight.cs
SilverLegend/SilverLegend/Services/SoundServiceSilverlight.cs
SilverLegend/SilverLegend/SilverLegend.csproj
SilverLegend/SilverLegend/Util/GameLoop.cs
SilverLegend/SilverLegend/Util/KeyHandler.cs
SilverLegend/SilverLegendCore/AI/AIDefine.cs
SilverLegend/SilverLegendCore/AI/aiBackground.cs
SilverLegend/SilverLegendCore/AI/aiCrowd.cs
SilverLegend/SilverLegendCore/AI/aiEnums.cs
SilverLegend/SilverLegendCore/AI/aiFrosh.cs
SilverLegend/SilverLegendCore/AI/aiMenuAndDisplay.cs
SilverLegend/SilverLegendCore/AI/aiMisc.cs
SilverLegend/SilverLegendCore/AI/aiPopUp.cs
SilverLegend/SilverLegendCore/AI/aiProjectile.cs
SilverLegend/SilverLegendCore/AI/aiSupport.cs
SilverLegend/SilverLegendCore/AI/aiWaterEffects.cs
SilverLegend/SilverLegendCore/AI/poleposn.cs
SilverLegend/SilverLegendCore/Achievements.cs
SilverLegend/SilverLegendCore/Control/GameConditions.cs
SilverLegend/SilverLegendCore/Control/GameLoop.cs
SilverLegend/SilverLegendCore/Control/Layers.cs
SilverLegend/SilverLegendCore/Control/SoundbankInfo.cs
SilverLegend/SilverLegendCore/GameBitmapEnumeration.cs
SilverLegend/SilverLegendCore/Globals.cs
SilverLegend/SilverLegendCore/IGameTimerService.cs
SilverLegend/SilverLegendCore/IInputService.cs
SilverLegend/SilverLegendCore/IRenderingService.cs
SilverLegend/SilverLegendCore/ISettingPersistanceService.cs
SilverLegend/SilverLegendCore/ISoundService.cs
SilverLegend/SilverLegendCore/Properties/AssemblyInfo.cs
SilverLegend/SilverLegendCore/ServiceFactory.cs
SilverLegend/SilverLegendCore/SilverLegendCore.csproj
SilverLegend/SilverLegendCore/Sprite/FrameDesc.cs
SilverLegend/SilverLegendCore/Sprite/SpriteInit.cs
SilverLegend/SilverLegendCore/Sprite/SpriteSet.cs
SilverLegend/SilverLegendCore/Sprite/TSprite.cs
SilverLegend/SilverLegendAssetsGame/Properties/AssemblyInfo.cs
SilverLegend/SilverLegendAssetsGame/SilverLegendAssetsGame.csproj
SilverLegend/SilverLegendAssetsMenu/Properties/AssemblyInfo.cs
SilverLegend/SilverLegendAssetsMenu/SilverLegendAssetsMenu.csproj
```

### Assets (815 files)

- **Graphics:** 535 BMP files in `SilverLegend/SilverLegendAssetsGame/Graphics/`
- **Sound:** 280 MP3 files in `SilverLegend/SilverLegendAssetsGame/Sound/`

---

## Notes

- **`.import` files excluded:** Godot regenerates these on project open
- **`.uid` files included:** Godot 4.x resource identifiers (small, text-based)
- **Assets duplicated:** Graphics/Sound exist in both Godot projects (identical content, different formats for Silverlight BMPs vs Godot PNGs)
- **Deploy scripts excluded:** Contains server paths (GodotLegendGD/deploy/)

---

## Verification

To verify this manifest matches what will be committed:

```bash
cd e:/projects/Greasepole2025
git add --dry-run . 2>&1 | grep "^add " | wc -l
# Should output: 2720
```
