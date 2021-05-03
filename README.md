TownTest - a small village for testing AIs.

WASD to move camera around  
Q drops toward ground  
E raises camera  
LMB to change camera direction  
  
Original town made by my professor, some AI stuff added by me. 

4/29/2021

Behavior Tree Update!

Now we have three people in town, guided by different behavior trees.
A miner, a priest, and a farmer. They have some things in common-
they all go home at night to sleep and wake up to do things in the morning
(by which I mean they stand in front of their house to sleep but you get 
the idea.) They also all get hungry and go to the marketplace to eat every
so often. 

The priest spends most of his day outside the church preaching. The other two
AI have a morale score that deteriorates throughout the day, and when they get
too down they go watch him preach. 

The farmer spends most of his day patrolling his farms, and occasionally going
to the mill to drop off a harvest. The miner is similar except he mines and drops
ore off at the blacksmith every so often.

All the behavior trees were implemented using Panda BT.

5/2/2021

GOAP update!!!

This update provides almost zero change to the behavior of the townspeople, but
it does provide a lot of potential for their AI going forward. 

2 of the 3 townspeople are thinking normally, but something is off about the miner...
he's scheming. Instead of his behavior tree explicitly telling him what to do, it's 
only telling him when he has a need that should be met. Using different actions stored
in his PandaBT script, he considers his position and comes up with a plan of action
to satisfy his goal. His behaviors look about the same, but if you look under the hood
into his C# file and his behavior tree you can see how he's making decisions now.
