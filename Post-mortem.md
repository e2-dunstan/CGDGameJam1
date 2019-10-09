# Spiderman Post Mortem

#### The Boomers: Ellie, Alex, Dan, Ben, Sol, Lewis.

##
## Teamwork and Project Development

### GitHub

- We used the GitHub issues and project pages. Hashes in commits to reference to them.
- Milestones of progression.
- Need to focus on the priorities in future projects.
- Level delayed combination of features.
- Branches were used. More Develop pulling may have been beneficial.
- More granular issues, instead of player combat punching, web shot etc.
- Look into use git project cards for better management. Maybe a big issue then multiple cards for each issue.

### Communication

- Outline mechanics being implemented clearer. Visualise what is in progress as well as communicating on Slack.
- Regular meetings and set a hard time in slack for all people to follow.
- Shouldve nmade prefabs of individual mechanics for the level developer to split up a load of work.
- Plan out level design on paper first and agree on it.
- When discussing mechanics confirm exactly how they'll work in the game. eg wall climbing was different in the first place due to lack of communication.

### Code
- Clear names for functions and variables for better legibility.
- Comments in code to assist that.
- If systems are working together then sit down together and explain the system in simple terms.

##
## The Game

### Core Mechanics and Features

- Player states
- Player movement

The web swing mechanic is arguably what makes a Spiderman game. Ellie implemented this by utilising Unity's physics 2D spring joint and a line renderer combined with the player states. The player's rigid body gravity scale was increased to make the swing faster, and drag was disabled so the swing speed was solely dependent on the player's input. The origin point (spring joint position) was offset from the player in the direction they are looking by a set number pixels in the x-axis with a multiplier based on the player's velocity which increased this offset. The swing state also defined the horizontal and vertical inputs to control the swing speed (horizontal) and moving up and down the web (vertical). These features were not initially in place however after external playtesting we noticed players naturally trying to do it so it was added.

- Wall crawling

- Combat - how this worked with web swing and shooting webs could have been improved.
- Collectibles
- Highscore system (is this local or online? Possible improvement there)

### Difficulty Loop

- Level - More web swinging sections. More people on the level next time as the level is core to the gameplay.
- Combat - Different variety of enemies. Maybe change punch into a combo that included the web into it. Limitations hurt feedback to player aka when hitting the enemies
- Boss fight - Boss fight couldve been more adaptive maybe based on player score / time.

### Feedback

- Playtesting, feedback form
- Controls

### Micro macro meta improvements

Micro: Maybe add a slight bit of momentum to swinging if your stationary. Player felt slipery. Webbing to wall climbing felt a bit clunky at times. 

Macro: Variation of enemies could've been better and couldve been placed in better places, Some enemies obstructed clear path of movement which made an unreasonable spike of difficulty. Didn't have time to implement the swinging gauntlet 

Meta: Reasonbly well with the restriction had. We shouldve also saved times as well as scores to encourage speed runs.

### Playtest
Small sample size for playtesting, in the future have more people play.


` insert screenshots here `
