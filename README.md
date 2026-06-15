This is a model of an AI traffic control in a game traffic simulation.
The AI is integrated within the intersection traffic light control.
This model are built to simulate whether AI DQN model can beat a static traffic light control in relieving the simulated congestion.
In the experimental example, the agent can optimize and shorten the waiting time for the red light by up to 20 seconds when the training model reaches convergence.
It uses Unity ML-agents to interact with the game environment while collecting the observation data as an input to the DQN then calculate to decide the most reasonable action through it's overall experiences.
Currently the model observation data are waited time, traffic volume, traffic density.
