# COMP472-Assignment-1

An introduction to AI using different search algorithms, including A\* search in order to solve the 8 tile puzzle.
Search algorithms tested:

- Uninformed Algorithms:
 - Depths First search
 - Breath First search
- Informed Algorithms
 - Greedy search with Manhattan distance heuristic
 - A\* search with the following heuristics:
   - Manhattan distance
    - Misplaced tiles
     - The minimum of Manhattan distance or Misplaced tiles
      - The invented "Row Swap" heuristic based off of linear conflicts
      - The inadmissible Row and Column count heuristic
