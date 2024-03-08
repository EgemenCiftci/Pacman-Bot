# Pacman Bot
A bot that moves between 64 predefined waypoints to collect pellets and escape from ghosts.

Waypoints on the maze are junctions and the points where the player changes direction.

## Methodology:
1. Find the nearest waypoints using the waypoints dictionary
2. Simulate and calculate the score of each waypoint including the current waypoint
3. Move to the highest score waypoint

## Waypoint Score Calculation:
Score = ActiveDistance x 2 + PassiveDistance x 1 + Pellet x 1000

We can exactly predict the next location of Hunter and Ambush. Their distances are ActiveDistance.

We can only predict the next locations of Guard and Stranger because of their random behavior. Their minimum distances are PassiveDistance.

The pellet is collected pellets on the way.
