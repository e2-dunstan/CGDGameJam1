# Spiderman Post Mortem

#### The Boomers: Ellie, Alex, Dan, Ben, Sol, Lewis.

##
## Teamwork and Project Development

### GitHub

Github was a big focus in our project. Due to the size of the group we needed to make sure we followed a base set of rules when using Git so people wouldn't lose work from bad merges or the like. We leveraged the Git issue system to break down the game into smaller components with labels to identify what aspect of the development they affected then assign these components to a group member. We then worked in separate branches under the names of the issues and made sure to use the # of the issue in the commits to reference back to them. The projects board was semi-automated to assign these issues to their current state.

For the next project, we will also use the Git Projects board to further break down issues into more granular tasks so that a clear sense of progression can be seen whilst working on the project. We also need to make more use of setting and paying attention to priorities of features and ensure that we get high priority tasks completed before starting lower priority ones. However in the same vein as this if priorities need to be adjusted mid-project then communication for this should be used to ensure everyone is on the same page because in the end a game development is iterative process and changes are going to be made throughout development.
 
 <p float="left">
    <img src="/images/Git_issues_example.PNG" width="500" />
</p>

### Communication

As a whole communication for the group was pretty strong. There were times where some weren't as prompt at replying to messages as others would've liked but we never had an instance where someone just completely dropped off the radar. Everyone was reasonably good at messaging and responding but in the future there are some things we want to hammer home.

Primarily in the future we need to make sure we discuss in detail mechanics and exactly how we want them to work in the game because in certain cases through development people would interpret things differently and systems needed to be redeveloped because of the lack of specifics. By going into detail with each mechanic we can take away any ambiguity and waste less time needed to adapt systems that didn't fit the overall planned criteria. This also applies to the design of gameplay aspects, for example, planning out a level layout on paper beforehand so that everyone has a clear idea of what the systems they're developing feedback into. This could be reflected by detailing discussions on these mechanics in their respective issue page.

A smaller but still important issue was planning for meetings. In general, we were casual about meeting up however there was an instance where a few of us wanted all of the group to meet at a certain time but were not completely specific on the time to meet, resulting in some of the members meeting an hour later. In the future to avoid this issue for group meetings we will specify a time and date that's fixed and ensure that everyone can turn up for that time by responding.

### Code

Since this project is the first time a lot of the members of the group have worked together we were all getting familiar with each person's coding and development style. In future projects, we have agreed on a couple of rules, in general, to follow to make life easier for each other.

Firstly, ensure that functions and variables are very clear in what they do and if there is any ambiguity on what something does or how it works add comments so that anyone else working with it can learn what something does with ease.

Secondly, ensure that you are always working in the appropriate branch for the work you're doing, even if the task is small make sure you change branches to keep all changes related to a Git Issue in one branch or one of its sub-branches if it's split up into smaller tasks.

Finally, if systems need to work in conjunction with each other ensure the developers of those systems spend time together explaining how that system works so that if a feature is missing that another person needs it can be added by the developer of that system as opposed to the person needing that feature have to implement a work-around later. It's a bit of extra upfront time spent to save a lot more wasted on confusion down the line.

##
## The Game

### Core Mechanics and Features

The player state system is one of the main things that allowed so many systems that took control of the player in different ways to work together. We had clear defined states for each type of movement and the scripts related to each state would only update if that state was active. This was very important as the calculations on velocity for web-swinging were vastly different to player movement so by using this, various members could each develop a system and have them work together without having to worry about conflicts and bugs.

The player movement was the first port of call. Without it, navigation of the level would be near impossible. For the system to feel good to play however we added a lot of extra tweakable parameters so that we could get the micro loop play to feel and flow the best possible.

The same went for the web swing mechanic, which is arguably what makes a Spiderman game. This was implemented by utilising Unity's physics 2D spring joint and a line renderer combined with the player states. The player's rigid body gravity scale was increased to make the swing faster, and drag was disabled so the swing speed was solely dependent on the player's input. The origin point (spring joint position) was offset from the player in the direction they are looking by a set number pixels in the x-axis with a multiplier based on the player's velocity which increased this offset. The swing state also defined the horizontal and vertical inputs to control the swing speed (horizontal) and moving up and down the web (vertical). These features were not initially in place however after external playtesting we noticed players naturally trying to do it so it was added. Extra consideration what put in place for how the player would collide with other objects in the scene whilst web-swinging to further improve the web micro loop. It was decided that objects on the 'Building' layer would cause the player to collide, and objects with the 'Ground' layer would also detach the player from the web.

The wall-crawling mechanic, also key in the Spiderman universe, was also added to the game. Similarly to the web-swinging, it was only active once the player was in that state (Wall Crawling state). The player could only climb up objects that were tagged as "Building", so it didn't conflict with any other movement systems. To activate wall-crawling, the player presses up on the vertical axis whilst in the trigger box placed around the building to attach themselves to the building. A constant velocity was applied when moving by pressing the buttons on the d-pad. The player could also fall off the wall by moving away from it and exiting the trigger box as well as jump off the wall by pressing space. When space is pressed, a ray cast is fired in front of the player and behind. If this ray cast hits a building then the player will automatically jump off in the opposite direction. Originally the player had to press a direction then press space but we found that they wouldn't have enough time before being out of the trigger box and fall. This is why automatically detecting the wall was implemented. This was done by increasing the velocity of the player to a high amount and setting the player state back to airborne as the correct horizontal and vertical drag was then applied.

Player combat was an interesting challenge due to the limited available inputs and visual feedback given the nature of trying to de-make to the Atari 2600. The player could punch to deal damage in the immediate area in front of them or web-shoot to stun enemies at range, allowing them to be easily knocked out by punching or avoided entirely. However, since one of our two inputs had already been bound to jump, this meant that both web-shooting and punching were assigned to a single button while pressing down on the d-pad allowed you to swap modes. Since this wasn't visually shown/taught to the player, testing showed that it is not immediately obvious that you can web-shoot at first.

While web swinging the player can only web-shoot which reduced the complexity by allowing players to focus on the swinging mechanic. In the final area, web-shooting locks on-to the boss allowing for more tense moments while the player desperately tries to avoid incoming projectiles.

Collectables were implemented to provide an alternate means of obtaining a higher score, as well as providing an incentive for the player to explore. Issues arose, however, due to their late implementation. Due to this fact, synergising the collectables intended purpose into the level was challenging, and implemented poorly. Greater thought into the diversity of collectables could also have been undertaken. Collectables such as keys, or player upgrades (for example granting the player the ability to learn web-swinging/attacking at a specific section of the game), could have been implemented to affect the progression mechanic in a positive way.

High score was implemented at the end of the game and saved in a persistent, locally stored format. Scores were implemented well with the game’s UI allowing for customizing of player name with three characters at the end of the playthrough. This customization style aligns with the styling other games of the time period. The final score was also calculated relative to the time taken for completion, the number of enemies killed, and the number of collectables obtained. More thought could have been put into the visual and auditory feedback and prompts regarding the obtainable score. Collectables had alternate colours representing their point value, however little else was considered beyond this. In the future, differing enemy colours and enemy difficulties could grant varying scores. However, this issue ties into a previous issue with regards to a desire for more enemy types.

### Improvements

Web swinging could have fed into the macro loop more by having sections of the level which required skill to traverse rather than the web feeling like an option rather than a requirement. On the macro level, momentum added to the player when they begin swinging when stationary would be a nice improvement.

The player movement could also be made to feel nicer if the friction was stronger between them and the surfaces to reduce the feeling of slipperiness which doesn't 100% reflect the Atari 2600 playstyle.

The level did not spend enough time developing each of the player's mechanics. There are some moves that the player has access to that are never encouraged by the level design due to its brevity. Expanding the level to take each player mechanic to its furthest, i.e. web swinging, would create a smoother difficulty curve and allow the player to better play with and explore their options. This could be attained by having multiple people working on the level design.

Combat could have been improved by having a larger variety of enemies and/or changing the punch into a combo that included the web into it. It was not clear to the player when they successfully hit an enemy which could have been reflected better through audio cues. Some of the enemy placements could have been improved where they obstruct a clear path of movement that added an unreasonable spike of difficulty. Additionally, the difficulty of the boss fight could have been more adaptive: based on the player's score or time rather than a fixed difficulty.

Finally, we would have liked to factor the time into the game more for speed runs for the meta loop, although this may have required more than one level for leaderboards for each.

### Playtest

Only a small group was used for external playtesting. Most of the feedback from this were the same issues that were known but weren’t fixed in time. This is because most of the playtesting was done right before the deadline, as there wasn’t a playable level to test the mechanics. Next time, having a level to test mechanics earlier in the development cycle is important next time in order to find bug and fix them.

##
## Gameplay

### Video
https://www.youtube.com/watch?v=9CEdEyLf21U&feature=youtu.be
[![Watch the video](https://img.youtube.com/vi/9CEdEyLf21U/maxresdefault.jpg)](https://www.youtube.com/watch?v=9CEdEyLf21U&feature=youtu.be)

### Screenshots
<p float="left">
  <img src="/images/mainmenu.PNG" width="400" />
  <img src="/images/gameplay1.PNG" width="400" />
  <img src="/images/gameplay2.PNG" width="400" /> 
  <img src="/images/gameplay3.PNG" width="400" />
  <img src="/images/gameplay4.PNG" width="400" />
  <img src="/images/gameplay5.PNG" width="400" />
  <img src="/images/gameover.PNG" width="400" />
  <img src="/images/highscores.PNG" width="400" />
</p>
