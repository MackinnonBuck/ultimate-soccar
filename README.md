# ultimate-soccar
A game written in C# and MonoGame, inspired by Rocket League.

## Physics Information
Ultimate SocCar uses the Farseer Physics library, but with some modifications. Such modifications are listed here:

* FarseerPhysics.Settings.MaxRotation was changed from PI/2 to PI in order to allow higher wheel angular velocities.