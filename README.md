## More visible pause

RimWorld mod.

Adds colored border when pause is active.

[Steam workshop link](https://steamcommunity.com/sharedfiles/filedetails/?id=1736472227)

For RimWorld version 1.1+ this mod require [Harmony mod](https://steamcommunity.com/sharedfiles/filedetails/?id=2009463077) lib distribution.

### Implementation details

Uses [Harmony] to patch _RimWorld.MapInterface.MapInterfaceOnGUI_BeforeMainTabs_

### How to update this when new version of RimWorld become available

  1. Create new entry in LoadFolders.xml
  1. Create folder for new Assembly
  1. Update ```About.xml``` supported versions
  1. Add new RimWorld assembly (C:\Program Files (x86)\Steam\steamapps\common\RimWorld\RimWorldWin64_Data\Managed), remove old assembly.
  1. Check if harmony assembly need to be incremented. For this:
     1. go the Harmony mod on steam 
     1. go to the git hub and snuff version (https://github.com/pardeike/HarmonyRimWorld/blob/v2.3.1.0/Current/Assemblies/0Harmony.dll).
     1. go to the NuGet and download new version
  1. Update ```OutputPath``` for release and debug configs in ```BorderOnPause.csproj``` to match folder created in step 2
  1. Clean build solution.

### Thanks

This mod wouldn't be possible without a lot of encouragement from my wife. 
Thank you. 

[Harmony]: https://github.com/pardeike/Harmony
