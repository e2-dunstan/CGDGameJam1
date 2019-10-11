# Spiderman Post Mortem

#### The Boomers: Ellie, Alex, Dan, Ben, Sol, Lewis.

##
## Teamwork and Project Development

### GitHub

Github was a big focus in our project. Due to the size of the group we needed to make sure we followed a base set of rules when using Git so people wouldn't lose work from bad merges or the like. We leveraged the Git issue system to break down the game into smaller components with labels to identify what aspect of the development they affected then assign these components to a group member. We then worked in seperate branches under the names of the issues and made sure to use the # of the issue in the commits to reference back to them. The projects board was semi-automated to assign these issues to their current state.

For the next project we will also use the Git Projects board to further break down issues into more granular tasks so that a clear sense of progression can be seen whilst working on the project. We also need to make more use of setting and paying attention to priorities of features and ensure that we get high priority tasks completed before starting lower priority ones. However in the same vain as this if priorities needs to be adjusted mid project then communcation for this should be used to ensure everyone is on the same page because in the end a game development is iterative process and changes are going to be made throughout development.
    
![Example Issue on GitHub](/images/Git_issues_example.PNG)

### Communication

As a whole commuication for the group was pretty strong, there was times where some weren't as prompt at replying to messages as others would've liked but we never had an instance where someone just completely dropped off the radar. Everyone was reasonably good at messaging and responding but in the future there are some things we want to hammer home. 

Primarily in the future we need to make sure we discuss in detail mechanics and exactly how we want them to work in the game because in certain cases through development people would interpret things differently and systems needed to be redeveloped because of the lack of specifics. By going into detail with each mechanic we can take away any ambiguity and waste less time needed to adapt systems that didn't fit the overall planned criteria. This also applies to the design of gameplay aspects also, for example, planning out a level layout on paper beforehand so that everyone has a clear idea what the systems they're developing feed back into. This could be reflected by detailing discussions on these mechanics in their respective issue page.

A smaller but still important issue was planning for meetings. In general we were casual about meeting up however there was an instance where a few of us wanted all of the group to meet at a certain time but were not completely specific on the time to meet, resulting in some of the members meeting an hour later. In future to avoid this issue for group meetings we will specify a time and date thats fixed and ensure that everyone can turn up for that time by responding.

### Code

Since this project is the first time a lot of the members of group have worked together we were all getting familiar with each person's coding and development style. In future projects we have agreed on a couple of rules in general to follow to make life easier for eachother.
    
Firstly, ensure that functions and variables are very clear in what they do and if there is any ambiguity on what something does or how it works add comments so that anyone else working with it can learn what something does with ease.

Secondly, ensure that you are always working in the appropirate branch for the work your doing, even if the task is small make sure you change branches to keep all changes related to a Git Issue in one branch or one of its sub branches if its split up into smaller tasks.

Finally, if systems need to work in conjunction with each other ensure the developers of those systems spend time together explaining how that system works so that if a feature is missing that another person needs it can be added by the developer of that system as opposed to the person needing that feature have to implement a work-around later. It's a bit of extra up front time spent to save a lot more wasted on confusion down the line.

##
## The Game

### Core Mechanics and Features

The player state system is one of the main things that allowed so many systems that took control of the player in different ways to work together. We had clear defined states for each type of movement and the scripts related to each state would only update if that state was active. This was very important as the calculations on velocity for web swinging were vastly different to player movement so by using this various members could each develop a system and have them work together without having to worry about conflicts and bugs.

The player movement was the first port of call. Without it navigation of the level would be near impossible. For the system to feel good to play however we added a lot of extra tweakable parameters so that we could get the micro loop play to feel and flow the best possible.

The same went for the web swing mechanic, which is arguably what makes a Spiderman game. This was implemented by utilising Unity's physics 2D spring joint and a line renderer combined with the player states. The player's rigid body gravity scale was increased to make the swing faster, and drag was disabled so the swing speed was solely dependent on the player's input. The origin point (spring joint position) was offset from the player in the direction they are looking by a set number pixels in the x-axis with a multiplier based on the player's velocity which increased this offset. The swing state also defined the horizontal and vertical inputs to control the swing speed (horizontal) and moving up and down the web (vertical). These features were not initially in place however after external playtesting we noticed players naturally trying to do it so it was added. Extra consideration what put in place for how the player would collide with other objects in the scene whilst web swinging to further improve the web micro loop. It was decided that objects on the 'Building' layer would cause the player to collide, and objects with the 'Ground' layer would also detach the player from the web.

- Wall crawling
- Combat - how this worked with web swing and shooting webs could have been improved.
- Collectibles
- Highscore system (is this local or online? Possible improvement there)

### Difficulty Loop

Web swinging could have fed into the macro loop more by having sections of the level which required skill to traverse rather than the web feeling like an option rather than a requirement.

- Level - More web swinging sections. More people on the level next time as the level is core to the gameplay.
- Combat - Different variety of enemies. Maybe change punch into a combo that included the web into it. Limitations hurt feedback to player aka when hitting the enemies
- Boss fight - Boss fight couldve been more adaptive maybe based on player score / time.

### Feedback

- Playtesting, feedback form
- Controls

### Micro macro meta improvements

Micro: Maybe add a slight bit of momentum to swinging if your stationary. Player felt slippery. Webbing to wall climbing felt a bit clunky at times. 

Macro: Variation of enemies could've been better and couldve been placed in better places, Some enemies obstructed clear path of movement which made an unreasonable spike of difficulty. Didn't have time to implement the swinging gauntlet 

Meta: Reasonbly well with the restriction had. We shouldve also saved times as well as scores to encourage speed runs.

### Playtest
Small sample size for playtesting, in the future have more people play.


` insert screenshots here `
