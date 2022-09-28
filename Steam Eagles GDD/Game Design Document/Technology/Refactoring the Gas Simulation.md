The original gas simulation needs to be refactored desperately.  It relies heavily on singletons,   `GasStuff`  is particularly harmful, as it's name is fairly useless.   Additionally I've begun to port the simulation to unity, for increased functionality and performance.  I've already written an adapter layer that makes the Unity tilemap api match the godot tilemap api.

# Motivations
why does the gas simulation need to be refactored?

#### Answer
the last iteration was a rapid prototyping iteration.  I made the primary focus experimentation rather than scalability and optimization.  The resulting code is about what you would expect prototype code to look like.  


# Goals
by the end of this sprint, we should have:
1. More optimized version of the gas simulation
2. Gas Simulation implemented in Unity
3. Suite of Unit Tests for the critical parts of the sim
4. A clean, reusable codebase that is well documented

# Organization
![[Unity Volume Mixer Naive Setup.png]]

 ![[Gas Sim Unity Port - Project Structure (I22).png]]