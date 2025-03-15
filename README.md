Colonization - Strategy Game with Automation Elements

A strategy game where players manage bases, automate resource gathering, and expand territory using AI-driven units and complex state logic. 
Built in Unity, this project showcases advanced gameplay mechanics and efficient code architecture.

📌 Overview
Colonization combines base management, resource automation, and strategic expansion. Players command bases to produce units, 
set expansion flags, and prioritize objectives dynamically. The game emphasizes AI behavior, interactive mechanics, and intricate state management systems.

🕹️ Key Features

### Automated Unit Production
- Resource-Driven Creation: Bases automatically spawn units upon accumulating 3 resources.
- Independent Resource Pools: Each base maintains its own resource collection, enabling parallel progression.

Territory Expansion via Flags
- Dynamic Flag Placement: Click a base and then a map location to set/relocate a flag (limited to one active flag per base).
- Map Constraints: Flags can only be placed within the playable map boundaries.

Strategic Base Construction
- Priority Shifting: Setting a flag triggers the base to gather 5 resources for sending a unit to build a new base.
- Unit Requirements: 
  - Units dispatched to flags construct new bases upon arrival.
  - Construction is blocked if the player has only 1 unit remaining.
- Post-Construction Reset: After building a new base, the original base resumes unit production, and the flag is removed.

🛠 Technologies & Architecture
- Unity: Handles rendering, physics, and core gameplay systems.
- C#: Implements:
  - OOP Principles: Modular class structures for bases, units, and resources.
  - Coroutines: Asynchronous actions for resource accumulation and unit movement.
  - Event-Driven Logic: Responsive UI and gameplay interactions.
- State Management System: Controls base priorities (e.g., switching between unit production and expansion modes).
