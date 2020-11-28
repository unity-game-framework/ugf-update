# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [5.2.0](https://github.com/unity-game-framework/ugf-update/releases/tag/5.2.0) - 2020-11-28  

### Release Notes

- [Milestone](https://github.com/unity-game-framework/ugf-update/milestone/11?closed=1)  
    

### Added

- Add player loop print to display nested update groups ([#35](https://github.com/unity-game-framework/ugf-update/pull/35))  
    - Add `UpdateUtility.PrintUpdateGroup` to print full hierarchy of subgroups, update collection, add and remove queue.
    - Add `IUpdateGroup.Print` extension method as shortcut for `UpdateUtility.PrintUpdateGroup` method.

## [5.1.0](https://github.com/unity-game-framework/ugf-update/releases/tag/5.1.0) - 2020-11-22  

### Release Notes

- [Milestone](https://github.com/unity-game-framework/ugf-update/milestone/10?closed=1)  
    

### Added

- Add IUpdateCollection add and remove methods ([#31](https://github.com/unity-game-framework/ugf-update/pull/31))  
    - Add `IUpdateCollection.Add`, `Remove` and `Contains` methods.

### Fixed

- Fix UpdateLoop update function change has no effect ([#30](https://github.com/unity-game-framework/ugf-update/pull/30))  
    - Fix `UpdateLoopBase` does not apply update function changes into player loop.

## [5.0.0](https://github.com/unity-game-framework/ugf-update/releases/tag/5.0.0) - 2020-11-17  

### Release Notes

- [Milestone](https://github.com/unity-game-framework/ugf-update/milestone/9?closed=1)  
    

### Changed

- Extend IUpdateLoop to manage player loop ([#26](https://github.com/unity-game-framework/ugf-update/pull/26))  
    - Add `IUpdateLoop.Contains`, `Add`, `Remove`, `AddFunction` and `RemoveFunction` methods to change player loop.
    - Add `UpdateLoopBase` abstract class as default implementation of some methods of `IUpdateLoop` interface.
    - Change `UpdateLoopUnity` to inherit from `UpdateLoopBase` and implement required members.
    - Change `UpdateProvider` to work only through `IUpdateLoop` object passed on construction.
- Remove inheritance of update function pointer when add subsystem ([#25](https://github.com/unity-game-framework/ugf-update/pull/25))  
    - Add `UpdateUtility.AddSubSystem` without update function argument to add subsystem with empty update function pointer.
    - Change `UpdateUtility.TryAddSubSystem` to add subsystems without update function pointer inherited from parent.

## [4.0.0](https://github.com/unity-game-framework/ugf-update/releases/tag/4.0.0) - 2020-11-15  

### Release Notes

- [Milestone](https://github.com/unity-game-framework/ugf-update/milestone/8?closed=1)  
    

### Changed

- Update to Unity 2020.2 ([#18](https://github.com/unity-game-framework/ugf-update/pull/18))  
    - Change `TItem` generic constraint for all groups to be a class.
    - Change find by path extensions to use forward flash (`/`) as separator instead of dot (`.`).

## [3.3.1-preview](https://github.com/unity-game-framework/ugf-update/releases/tag/3.3.1-preview) - 2019-11-17  

- [Commits](https://github.com/unity-game-framework/ugf-update/compare/3.3.0-preview...3.3.1-preview)
- [Milestone](https://github.com/unity-game-framework/ugf-update/milestone/7?closed=1)

### Fixed
- `UpdateUtility.PrintPlayerLoop`: fix argument exception when indent is zero.

## [3.3.0-preview](https://github.com/unity-game-framework/ugf-update/releases/tag/3.3.0-preview) - 2019-11-09  

- [Commits](https://github.com/unity-game-framework/ugf-update/compare/3.2.0-preview...3.3.0-preview)
- [Milestone](https://github.com/unity-game-framework/ugf-update/milestone/6?closed=1)

### Added
- `IUpdateLoop`: `Reset` method to reset current state of the player loop.

## [3.2.0-preview](https://github.com/unity-game-framework/ugf-update/releases/tag/3.2.0-preview) - 2019-10-12  

- [Commits](https://github.com/unity-game-framework/ugf-update/compare/3.1.0-preview...3.2.0-preview)
- [Milestone](https://github.com/unity-game-framework/ugf-update/milestone/5?closed=1)

### Added
- `UpdateGroup` as non-generic implementation.
- `UpdateProvider`: `GetGroup` by path and `Remove` by group methods.

## [3.1.0-preview](https://github.com/unity-game-framework/ugf-update/releases/tag/3.1.0-preview) - 2019-10-11  

- [Commits](https://github.com/unity-game-framework/ugf-update/compare/3.0.0-preview...3.1.0-preview)
- [Milestone](https://github.com/unity-game-framework/ugf-update/milestone/4?closed=1)

### Added
- `IUpdateProvider` to manage update groups in player loop.
- `IUpdateLoop` interface to control player loop.
- `TryFindCollection` and `TryFindGroup` extensions for `IUpdateProvider` and `IUpdateGroup` to find group using path.

## [3.0.0-preview](https://github.com/unity-game-framework/ugf-update/releases/tag/3.0.0-preview) - 2019-10-03  

- [Commits](https://github.com/unity-game-framework/ugf-update/compare/2.0.0-preview...3.0.0-preview)
- [Milestone](https://github.com/unity-game-framework/ugf-update/milestone/3?closed=1)

### Added
- `UpdateUtility.ResetPlayerLoopToDefault`: to reset current player loop.

### Changed
- Update to Unity 2019.3.
- Rework update collections and update group.

## [2.0.0-preview](https://github.com/unity-game-framework/ugf-update/releases/tag/2.0.0-preview) - 2019-09-22  

- [Commits](https://github.com/unity-game-framework/ugf-update/compare/1.0.0-preview...2.0.0-preview)
- [Milestone](https://github.com/unity-game-framework/ugf-update/milestone/2?closed=1)

### Added
- Package short description.
- `UpdateList`, `UpdateListHandler`, `UpdateSet` and `UpdateSetHandler` implementation of the update collection.
- `UpdateGroup`: a group with update and subgroups collection.
- `UpdateUtility.PrintPlayerLoop`: display `loopConditionFunction` and `updateFunction` for each subsystem.
- `UpdateUtility.AddSubSystem`: `updateFunction` argument as pointer of the native update function to use in new subsystem. 

### Changed
- Rework all update collections.
- Update to Unity 2019.2.
- `UpdateUtility.TryAddSubSystem`: change arguments signature.

## [1.0.0-preview](https://github.com/unity-game-framework/ugf-update/releases/tag/1.0.0-preview) - 2019-07-12  

- [Commits](https://github.com/unity-game-framework/ugf-update/compare/a5288f5...1.0.0-preview)
- [Milestone](https://github.com/unity-game-framework/ugf-update/milestone/1?closed=1)

### Added
- This is a initial release.


