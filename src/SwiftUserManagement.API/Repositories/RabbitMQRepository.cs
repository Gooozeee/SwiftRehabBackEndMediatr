using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using SwiftUserManagement.API.Entities;
using System.IO;
using System.Text;
using System.Text.Json;

namespace SwiftUserManagement.API.Repositories
{
    // Concrete class for emitting tasks out to the rabbitMQ queue
    public class RabbitMQRepository : IRabbitMQRepository
    {
        private readonly ILogger<RabbitMQRepository> _Logger;

        public RabbitMQRepository(ILogger<RabbitMQRepository> ILogger)
        {
            _Logger = ILogger ?? throw new ArgumentNullException(nameof(ILogger));
        }

        // Sending out the game score analysis task to the queue
        public bool EmitGameAnalysis(GameResults gameResults)
        {
            if (gameResults.result1 == null)
            {
                return false;
            }

            // Connecting to the RabbitMQ queue
            var factory = new ConnectionFactory() { HostName = "rabbitmq" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                _Logger.LogInformation("Sending game results for analysis");
                // Setting up and sending the message
                channel.ExchangeDeclare(exchange: "swift_rehab_app",
                                        type: "topic");

                var routingKey = "game.score.fromApp";
                var message = JsonSerializer.Serialize(gameResults);
                var body = Encoding.UTF8.GetBytes(message);
                channel.BasicPublish(exchange: "swift_rehab_app",
                                     routingKey: routingKey,
                                     basicProperties: null,
                                     body: body);
                _Logger.LogInformation("Sent game results for analysis");

                return true;
            }
        }

        // Sending out the video file task to the queue
        public async Task<bool> EmitVideoAnalysis(IFormFile video)
        {
            if(video == null)
            {
                return false;
            }

            // Connecting to the RabbitMQ queue
            var factory = new ConnectionFactory() { HostName = "rabbitmq" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                _Logger.LogInformation("Sending video file for analysis");
                // Setting up and sending the message
                channel.ExchangeDeclare(exchange: "swift_rehab_app",
                                        type: "topic");

                var routingKey = "video.fromApp";



                MemoryStream ms = new MemoryStream(new byte[video.Length]);
                await video.CopyToAsync(ms);
                

                //var body = Encoding.UTF8.GetBytes(video.OpenReadStream());
                channel.BasicPublish(exchange: "swift_rehab_app",
                                     routingKey: routingKey,
                                     basicProperties: null,
                                     body: ms.ToArray());
                _Logger.LogInformation($"Sent video file for analysis + {ms.ToArray()}");

                return true;
            }
        }

        // Receiving the results from the game analysis
        public string ReceiveGameAnalysis()
        {
            string receivedMessage = "";

            var factory = new ConnectionFactory() { HostName = "rabbitmq" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.ExchangeDeclare(exchange: "swift_rehab_app", type: "topic");
                var queueName = channel.QueueDeclare().QueueName;

                channel.QueueBind(queue: queueName,
                                  exchange: "swift_rehab_app",
                                  routingKey: "game.toC#");

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body.ToArray();
                    receivedMessage = Encoding.UTF8.GetString(body);
                    var routingKey = ea.RoutingKey;

                    _Logger.LogInformation("Video analysis received '{0}':'{1}", routingKey, receivedMessage);
                };

                channel.BasicConsume(queue: queueName,
                                     autoAck: true,
                                     consumer: consumer);

                //_Logger.LogInformation("Video analysis received: '{0}'", receivedMessage);

                int logValue = 0;
                while (receivedMessage == "")
                {
                    logValue++;
                    if (logValue % 500 == 0)
                    {
                        _Logger.LogInformation("Haven't received result yet");
                    }
                    Thread.Sleep(10);
                    channel.BasicConsume(queue: queueName,
                                     autoAck: true,
                                     consumer: consumer);
                    if (logValue > 1000)
                    {
                        return "The request has timed out";
                    }
                }

                return receivedMessage;
            }
        }

        // Receiving the video analysis results from the python script
        public string ReceiveVideoAnalysis()
        {
            string receivedMessage = "";

            var factory = new ConnectionFactory() { HostName = "rabbitmq" };
            using (var connection = factory.CreateConnection()) 
            using (var channel = connection.CreateModel())
            {
                channel.ExchangeDeclare(exchange: "swift_rehab_app", type: "topic");
                var queueName = channel.QueueDeclare().QueueName;

                channel.QueueBind(queue: queueName,
                                  exchange: "swift_rehab_app",
                                  routingKey: "video.toC#");

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body.ToArray();
                    receivedMessage = Encoding.UTF8.GetString(body);
                    var routingKey = ea.RoutingKey;
                    
                    //_Logger.LogInformation("Video analysis received '{0}':'{1}", routingKey, receivedMessage);
                };

                channel.BasicConsume(queue: queueName,
                                     autoAck: true,
                                     consumer: consumer);

                int logValue = 0;
                while(receivedMessage == "")
                {
                    logValue++;
                    if(logValue % 500 == 0)
                    {
                        _Logger.LogInformation("Haven't received result yet");
                    }
                    Thread.Sleep(10);
                    channel.BasicConsume(queue: queueName,
                                     autoAck: true,
                                     consumer: consumer);
                    if(logValue > 1000)
                    {
                        return "The request has timed out";
                    }
                }

                return receivedMessage;

            }
        }
    }
}
