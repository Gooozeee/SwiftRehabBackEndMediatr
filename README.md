# SwiftRehabAppBackEnd

ASP.NET Core web API made to work with the final year dissertation project Swift Rehab Application which was written in React Native.

The controller and main bulk of the code is written in ASP.NET Core, each service is containerised and there are two Python files used for analysis of the examination data from the front end. This data is sent over to the C# controller, and the controller then sends this data out to the Python files via MassTransit (RabbitMQ).
All data is stored on a PostgreSQL database which communicates with the ASP.NET Core API through a repository.
The code is organised using Clean Architecture and commands/queries are controlled and validated by using the CQRS and Mediator design patterns. (MediatR package)

------------ Hints and tips -------------

To see and manage all of the containers visit localhost:9000. Use username: admin and password adminadmin123

To see and manage the PostgreSQL database visit localhost:5050. Use username: admin@aspnetrun.com and password admin1234

To see the controller on Swagger visit localhost:5000/swagger/index.html. To authorise simply create a user, then input those details into the Authorise endpoint. Finally click the authorise button on the top right and input the token which you received from the authorise endpoint.

To see RabbitMQ mangement visit localhost:15672. Use username: guest and password guest

--------------- Running back end in docker ------------

Instructions for running the back end API (This will containerise all the code needed for the backend to work)

1. Install an IDE to see the C# code with .net 6 (Visual studio, VS code, rider)
2. Install docker desktop (Install WSL2 if prompted to)
3. Build the C# code
4. Open the docker-compose project in powershell (src folder in the code)
5. Run the command "docker-compose -f docker-compose.yml -f docker-compose.override.yml up --build"

------------------ Python local set up ----------------

For the python set up (Running the python scripts outside of docker containers)
1. Install python 3.9 (Use python 3.6.6 for video analysis)
2. Install pip
3. cd into the requirements folder
4. Run command pip install -r requirements.txt (This will install all required python packages)
5. Run the python scripts by cd'ing into the folder and doing 'python SCRIPT_NAME.py' (Make sure the rabbitmq container is running first)

To get the saved image from the video python container onto the host machine (To see if the video correctly got saved into the python container)
docker cp container_id:/src/VideoAnalysis/UploadedVideos/Video.mov filepath to where you would like the video to go to

----------------------- Kubernetes --------------------

1. First thing to do on kubernetes cluster to sign in (When using Azure Kubernetes Service)
   az aks get-credentials --resource-group "resource group" --name "cluster name"
   check with kubectl get pods

----- Removing pods -------
Delete them with kubectl delete deployments --all
kubectl delete services --all

-------- Installing rabbitmq on kubernetes ----------
kubectl apply -f "https://github.com/rabbitmq/cluster-operator/releases/latest/download/cluster-operator.yml"

------------------- Deploying app ----------------
1. Set up a key vault and secure all secrets
2. Set up a container registry and upload containers into registry
3. Set up a kubernetes cluster which uses above container registry
4. For azure login in azure cli run az aks get-credentials --resource-group "resource group" --name "cluster name"
5. Check you are logged in with kubectl get pods
6. Upload yaml files and run this command for all of the files kubectl apply -f "deployment_file.yml" 
7. Run kubectl get services
8. Get the external IP next to swiftusermanagementapi and call the service from that IP address
   
