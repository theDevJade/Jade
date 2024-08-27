# JadeLib

## JadeLib is a versatile library for Exiled SCP:SL that has a wide variety of features and some useful bundled in assemblies.

### Features:
- Player Statistics system.
- Useful Extensions
- Multi hint system with elements and other plugin support (WIP)
- Some utility abstract classes relating to reflection
- Easy 914 recipe creation.
- Useful audio API featuring playing audio, converting audio (any file to ogg), downloading during runtime from Youtube, and more.
- FeatureGroups that allow reflection based subscribing of events.

*Documentation is soon to come for all of these features.*

## Getting Started
In order to initialize JadeLib from code you must call, multiple plugins initializing will not cause any issues.
```csharp
Jade.Initialize();
```

---
### For Server Owners

Either compile the source or use one of the releases, drop into dependencies and enjoy.
Ensure you don't have duplicate assemblies, check `FodyWeavers.xml` for any already downloaded dependencies and removed. They are bundled.

### Credits:
- RueI for hint parsing and ElemCombiner
- Fody Weavers w/ Costura.
- VideoLibrary by omansak

### Licenses for included dependencies
- libvideo: BSD 2-Clause License
