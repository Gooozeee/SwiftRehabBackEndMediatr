FROM python:3.9
WORKDIR /src
COPY ["GameAnalysis/GameAnalysisRabbitMQClient.py", "GameAnalysis/"]
RUN pip install Pika
CMD [ "python", "-u", "GameAnalysis/GameAnalysisRabbitMQClient.py"]
