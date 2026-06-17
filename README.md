# About This Project
This is a model of an AI traffic control in a game traffic simulation.
The AI is integrated within the intersection traffic light control.<br>
This model are built to simulate whether AI DQN model can beat a static traffic light control in relieving the simulated congestion.<br>
It uses Unity ML-agents to interact with the game environment while collecting the observation data as an input to the DQN then calculate to decide the most reasonable action through it's overall experiences.<br>
In the experimental example, the agent can optimize and shorten the waiting time for the red light by up to 20 seconds faster than an automated traffic lights control when the training model reaches convergence.<br>

# Gameplay
The game settings comes with 2 type of intersection which is a 3 way and 4 way. The main gameplay is to operate the intersection traffic light passage of each lane, and then the environment will simulate the traffic from which we can built on the configuration scenario menu, see the UI section. <br>
We also bring an example model of AI operating the traffic light and a static rule based automated trafiic light control on the MODE button. <br>
Note that if we run the AI model we also train the model in real time. <br>
We can also see the parameter which is on the bottom left corner that shows the statistic of the overall simulated traffic (this parameter is used to feed the agent to make a decision btw). <br>
<img width="1151" height="624" alt="play1" src="https://github.com/user-attachments/assets/46c52cd1-becb-4abd-b2c1-d8fc694040fa" /><br>
<img width="1141" height="636" alt="play2" src="https://github.com/user-attachments/assets/bc28024f-87cb-4b0d-9cb6-7dc8ec4036f6" /><br>
<img width="1136" height="630" alt="play3" src="https://github.com/user-attachments/assets/20911ea1-ba35-401b-8ed9-57e61ef9620d" /><br>
<img width="1148" height="629" alt="play4" src="https://github.com/user-attachments/assets/285cb76f-7b1b-42f0-b265-90640809e8bd" /><br>

# UI
Here you can see the UI game menu that we built for this simulation game. we built it with short features only functional for the main purpose of the game.
<img width="633" height="353" alt="main menu" src="https://github.com/user-attachments/assets/b4b65ddf-db4b-40ca-9d35-1fd364b27e50" /><br>
<img width="620" height="347" alt="getstart" src="https://github.com/user-attachments/assets/11c42d20-c2a1-4dca-ac39-ec70b3b3767a" /><br>
<img width="593" height="328" alt="rundown" src="https://github.com/user-attachments/assets/9a4bd8cb-6f8f-4783-80dd-d053045fd65b" /><br>
<img width="594" height="328" alt="rundown2" src="https://github.com/user-attachments/assets/2502210c-861b-4b02-841a-9af153e19c2f" /><br>
<img width="589" height="329" alt="rundown3" src="https://github.com/user-attachments/assets/7eed3d22-9864-43b1-9d5c-dbb055ad489e" /><br>
<img width="310" height="174" alt="config" src="https://github.com/user-attachments/assets/c415eafc-0fde-4fe2-bf35-b68815e07862" /><br>
<img width="317" height="179" alt="result" src="https://github.com/user-attachments/assets/1942e013-be09-41f4-b7c8-147a2eaad54e" /><br>

# Network Architecture
The architecture is built with Deep Q-Network using with 2 hidden layer 156 neuron and reLU activation. Then we implement the input layer consist of total vehicles, lane density, wait time, phase status, and intersection density. After that we implement 2 action that is hold phase and continue phase. These phase is a way of we simplify the traffic lights control mechanism by only focusing on whether the intersection traffic lights should make the operation control to make the next lane can cross the intersection while the other lanes wait.  <br>
<img width="551" height="451" alt="network architecture" src="https://github.com/user-attachments/assets/8bd4a143-b4d5-499f-9ad2-a559dfa94a26" /><br>
