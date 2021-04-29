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
