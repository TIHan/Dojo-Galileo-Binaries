(*
Galileo Dojo
+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

The goal of this dojo is to have some simple fun with 3D
graphics. Galileo has a few built-in functions that allow
you to create and animate 3D spheres, using OpenGL and 
Ferop (https://github.com/TIHan/Ferop).

The dojo guides you through the existing functionality. Go
through each section one by one, run the code and read the
explanations, and... have fun with it!
*)

// ------------------------------------------------------------------------- //
(*
0. Introduction
+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

- if you are on Windows: make sure FSI is set to 32 bits.
Go to Tools > Options > F# Tools > F# Interactive, and set
64-bit F# Interactive to "false".

Then, just run the code below, to load the required
dependencies.
*)

#r @"System.Runtime.dll"
#r @"System.Numerics.Vectors.dll"
#r @"Galileo.dll"

open System
open System.Numerics
open Galileo
open Game

Galileo.init ()

// ------------------------------------------------------------------------- //
(*
1. Spawning a planet
+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

To start with, we will simply create a planet.
*)

// Position the camera / view at location
// X = 0, Y = 0, Z = 2
Galileo.setUpdateCameraPosition (fun () ->
    Vector3 (0.f, 0.f, 2.f)
)

// Create a "planet" earth, at default position
// X = 0, Y = 0, Z = 0.
// The image "earth.jpg" is used to decorate the planet surface.
let earth = Galileo.spawnPlanet "earth.jpg"

// TODO: what happens if you change the camera, to, say,
// X = 0, Y = 0, Z = 2.5 ?


// ------------------------------------------------------------------------- //
(*
2. Changing the angle of view
+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

By changing where the camera is located, you can create an
animation: the planet doesn't move, but the "cameraman" 
is moving around it.
*)

for i in - 10 .. 10 do
    // wait for 1 sec (1000 milliseconds)
    Async.Sleep 1000 |> Async.RunSynchronously
    // change the camera position
    let position = float32 i
    Galileo.setUpdateCameraPosition (fun () ->
        Vector3 (0.f, position, 2.f)
    )

// ------------------------------------------------------------------------- //
(*
3. Changing the position of a planet.
+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

A planet, after it was created with Galileo.spawnPlanet, 
can be updated, using (for instance) earth.SetUpdate, like
below.
The block of code defined in SetUpdate will be called at 
regular time intervals, and redraw the planet accordingly.
*)

// reposition the camera somewhere reasonable...
Galileo.setUpdateCameraPosition (fun () ->
    Vector3 (0.f, 0.f, 4.f)
)

// The following function is a bit artificial, and is there
// mainly to illustrate how the "rendering loop" works.
// When you run this function, you should see time and 
// interval print in a loop, everytime the rendering of the
// planet is updated.
earth.SetUpdate (fun time interval planet ->
    printfn "Time: %A" time
    printfn "Interval: %A" interval
    // ignore this for now
    { planet with Translation = Matrix4x4.CreateTranslation(Vector3.Zero) }
)


// To update the position of a planet, you apply a 
// translation, which defines in what location the planet
// should be re-drawn.
earth.SetUpdate (fun time interval planet ->
    // where (X,Y,Z) should the planet be drawn
    let location = Vector3 (1.f, 0.f, 0.f)
    // draw the same planet, with one change: 
    // translate it to location
    { planet with Translation = Matrix4x4.CreateTranslation location }
)


// Vector3 has a couple of built-in utilies...
earth.SetUpdate (fun time interval planet ->
    { planet with Translation = Matrix4x4.CreateTranslation (Vector3.Zero) }
)

// Try out Vector3.UnitX, Vector3.UnitY and Vector3.UnitZ.


// ------------------------------------------------------------------------- //
(*
4. Changing the size of a planet
+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

Let's create a second planet, the moon, and change its 
size, by using Scale.
*)

// we create the moon...
let moon = Galileo.spawnPlanet "moon.jpg"

// we move the earth to (3, 0, 0) to get it out of the way:
earth.SetUpdate (fun time interval planet ->
    { planet with Translation = Matrix4x4.CreateTranslation (Vector3.UnitX * 3.f) }
)


// Scale defines the size of the planet
moon.SetUpdate (fun time interval planet ->
    { planet with Scale = Matrix4x4.CreateScale (0.5f) }
)

// Try out changing scale of the moon.
// Perhaps even animate it?

// ------------------------------------------------------------------------- //
(*
5. Moving a planet by rotation
+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

We have used Matrix4x4.CreateTranslation to move a planet
to a certain position. In a similar fashion, you can use
Matrix4x4.CreateRotation to move it following a circular
movement.
*)

let PI = Math.PI |> single

moon.SetUpdate (fun time interval planet ->
    // we can retrieve the position of other planets
    let center = earth.LastKnownState.Translation.Translation
    let angle = PI
    // we move the planet to a position rotated by 
    // Pi radians (180 degrees) around the center of earth: 
    { planet with Translation = Matrix4x4.CreateRotationY (angle, center) }
)


// ------------------------------------------------------------------------- //
(*
6. Rotating a planet
+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

What we REALLY want is to see the moon constantly rotating 
around the earth. To do this, we need to apply a rotation,
but change the angle every time we redraw, increasing it
over time.
One way to do that is to create a mutable reference to the
angle, and increase it every time a redraw is triggered.
*)

let acc = ref 0.f
moon.SetUpdate (fun time interval planet ->
    let center = earth.LastKnownState.Translation.Translation
    let angle = !acc
    acc := !acc + 0.05f
    { planet with 
        Translation = Matrix4x4.CreateRotationY (angle, center)
        Scale = Matrix4x4.CreateScale (0.5f) }
)


// ------------------------------------------------------------------------- //
(*
6. Free form!
+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

That's it - this gives you a sense for what you can do with
Galileo. Now have fun, and create some nice stuff :)

Here are some ideas:
- use different textures for the planets
- a replica of the main planets in the solar system
- the moon circles around the earth, which circles around the sun...
- who cares about planets! let's have bouncy balls
- how about creating a Saturn-like planet with a ring?

And... share your creations, either on Twitter or with us!
And let us know if you have ideas on how to make this better.  
*)
