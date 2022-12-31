Sample project that contains 3 different implementations of the well known Snake game:

1. A collider based mvp implemented in just a single Snake class
2. A look up table based mvp that uses a 2d grid to detect boundaries, snakeparts, pickups using an additional SnakeField class
3. A more elaborate component based snake game that uses something similar to a model view controller setup where we have a SnakeModel and several different SnakeViews.
The SnakeModel is a wrapper around a LinkedList, which makes for a way more optimized implementation then the listbased approach used for sample 1 & 2.

Views implemented:
- simple gizmo based debug view
- cube based simple view
- more elaborate lerp based caterpillar view with better looking graphics and game flow/ui.

