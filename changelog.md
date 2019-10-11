# Changelog
All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## Unreleased - 2019-01-01
- [Commits](https://github.com/unity-game-framework/ugf-update/compare/0.0.0...0.0.0)
- [Milestone](https://github.com/unity-game-framework/ugf-update/milestone/0?closed=1)

### Added
- Nothing.

### Changed
- Nothing.

### Deprecated
- Nothing.

### Removed
- Nothing.

### Fixed
- Nothing.

### Security
- Nothing.

## 3.1.0-preview - 2019-10-11
- [Commits](https://github.com/unity-game-framework/ugf-update/compare/3.0.0-preview...3.1.0-preview)
- [Milestone](https://github.com/unity-game-framework/ugf-update/milestone/4?closed=1)

### Added
- `IUpdateProvider` to manage update groups in player loop.
- `IUpdateLoop` interface to control player loop.
- `TryFindCollection` and `TryFindGroup` extensions for `IUpdateProvider` and `IUpdateGroup` to find group using path.

## 3.0.0-preview - 2019-01-01
- [Commits](https://github.com/unity-game-framework/ugf-update/compare/2.0.0-preview...3.0.0-preview)
- [Milestone](https://github.com/unity-game-framework/ugf-update/milestone/3?closed=1)

### Added
- `UpdateUtility.ResetPlayerLoopToDefault`: to reset current player loop.

### Changed
- Update to Unity 2019.3.
- Rework update collections and update group.

## 2.0.0-preview - 2019-09-22
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

## 1.0.0-preview - 2019-07-12
- [Commits](https://github.com/unity-game-framework/ugf-update/compare/a5288f5...1.0.0-preview)
- [Milestone](https://github.com/unity-game-framework/ugf-update/milestone/1?closed=1)

### Added
- This is a initial release.

---
> Unity Game Framework | Copyright 2019
