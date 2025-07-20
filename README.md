# MiddleUnityDeveloperTestTask
<img width="1612" height="901" alt="image" src="https://github.com/user-attachments/assets/71ee2157-bb9b-402c-ac30-4835d55e8deb" />

## Task
### Overview 
- It is necessary to implement a multiplayer turn-based strategy game for two players.
- The playing field is generated at the start of the session.
- Two players start with some specific set of units (mirror set) on different sides of the playing field.
- Each player can perform two actions in his turn: Move and Attack
- The player is allocated 60 seconds for each turn.
### PlayGround
- A playing field of any size
- You can specify which obstacles may appear, and the area within which obstacles may appear
- You can specify the minimum and maximum number of obstacles of this type in each individual obstacle.
- You can set the points of appearance of units for both players (A zone with randomization of the position of units is allowed)
- You can set the starting line-up of player armies
### Units
- Two types of units - slow but long-range and fast but with a small range of attack. Health and damage are optional, we believe that everyone has one health point and one damage unit.
- The player selects a unit with the left mouse button. The selected unit is highlighted (The backlight can be made in any form. Indicator above the unit, offline, circle under the unit)
- The right mouse button predicts the path for this unit to the point of click (If the left mouse button is pressed, the predicted path is erased)
- By double-clicking the right button, the unit is sent to the specified point
- The player cannot build a path that exceeds the unit's movement speed in length, and the unit cannot start moving to a point if the length of the path to it exceeds its speed.
- The unit cannot pass through obstacles
- A unit cannot push other units or pass through them while moving, the path must be built around other units.
- The selected unit has a display of its attack radius. If a path is built, then the attack radius is displayed from the final point of the path.
- All enemy units within the attack radius (around the unit when there is no path and around the final point when there is a path) are highlighted
- Right-click on the enemy unit to give the order to attack. If the attack is successful, the target unit is destroyed.
- The attack radius must take into account the size of the units. The target unit may be partially within the attack radius and may be a valid target.
- Do not use colliders or triggers to identify targets within the attack radius
- The server does not allow performing invalid actions (primitive protection against cheats)
### NotImplemented:
- Resolve situations where one player avoids a fight, make the following draw resolution mechanism:
- You cannot attack opponents who are behind an obstacle.
## Problems 
- There is no clear division into client-server logic.

## Time
- Deadline time: unlimited.
- Completion time: 3 days.

## Result
- ?
