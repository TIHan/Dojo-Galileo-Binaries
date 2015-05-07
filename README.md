# Dojo - Galileo

## Instructions

### Windows

- Open Galileo.fsx in Visual Studio. Make sure your F# interactive is set to 32-bit!

### OSX

- Download and install SDL2 https://www.libsdl.org/release/SDL2-2.0.3.dmg
- Open Galileo.fsx in Xamarin Studio. Make sure you have all of Mono installed.
- 

#### API

```fsharp
type Entity<'T> =

    member SetUpdate : (TimeSpan -> TimeSpan -> 'T -> 'T) -> unit

    member LastKnownState : 'T
    
type Planet =
    {
        Translation: Matrix4x4
        Rotation: Matrix4x4
        Scale: Matrix4x4
    }

module Galileo =

    val init : unit -> unit

    val spawnPlanet : string -> Entity<Planet>

    val setUpdateCameraPosition : (unit -> Vector3) -> unit

    val setUpdateLookAtPosition : (unit -> Vector3) -> unit
```
