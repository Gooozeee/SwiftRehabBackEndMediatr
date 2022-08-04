using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SwiftUserManagement.API.Entities;
using SwiftUserManagement.API.Repositories;
using System.IO;
using System.Net;

namespace SwiftUserManagement.API.Controllers
{
    // User management controller
    [Route("api/[controller]")]
    [ApiController]
    public class SwiftUserManagementController : ControllerBase
    {

        // Dependency injection for the user repository to bring in business logic
        private readonly IUserRepository _userRepository;
        private readonly IJWTManagementRepository _jwtMangementRepository;
        private readonly IRabbitMQRepository _rabbitMQRepository;
        private readonly ILogger<SwiftUserManagementController> _logger;
       
        public SwiftUserManagementController(IUserRepository userRepository, IJWTManagementRepository jwtMangementRepository, IRabbitMQRepository rabbitMQRepository, ILogger<SwiftUserManagementController> logger, IHostEnvironment environment)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _jwtMangementRepository = jwtMangementRepository ?? throw new ArgumentNullException(nameof(jwtMangementRepository));
            _rabbitMQRepository = rabbitMQRepository ?? throw new ArgumentNullException(nameof(rabbitMQRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }



        // Creating a user in the database
        [AllowAnonymous]
        [HttpPost("createUser", Name = "CreateUser")]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        public async Task<ActionResult> createUser([FromBody] User user)
        {
            // Validating the user
            if (string.IsNullOrEmpty(user.UserName) ^ string.IsNullOrEmpty(user.Password))
            {
                return BadRequest(new { Message = "User is invalid" });
            }

            // Creating the user
            var result = await _userRepository.CreateUser(user.Email, user.UserName, user.Password, user.Role);

            // Checking if the user exists or not
            if (result == false)
            {
                return BadRequest(new { Message = "User already exists" });
            }
            return CreatedAtRoute("CreateUser", new { userName = user.UserName });
        }

        // Retreiving a user from the database
        [HttpGet("{userName}", Name = "GetUser")]
        [Authorize]
        [ProducesResponseType(typeof(User), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public ActionResult<User> getUser(string userName)
        {
            var user = _userRepository.GetUser(userName);

            if (user.Result.Id == -1) return BadRequest(new { Message = "User not found" });

            return Ok(user);
        }

        // Letting the client know it is connected to the server
        [AllowAnonymous]
        [HttpGet("pingServer", Name = "Ping")]
        public string pingServer()
        {
            return "You are connected to the server";
        }

        // Authenticating a user and returning a JWT token
        [AllowAnonymous]
        [HttpPost("auth", Name = "Authenticate")]
        [ProducesResponseType(typeof(Tokens), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        public ActionResult<Tokens> Authenticate(string email, string password)
        {
            var token = _jwtMangementRepository.Authenticate(email, password);

            if (token == null)
            {
                return Unauthorized();
            }

            return Ok(token);
        }

        // Emitting the game results for analysis by the python file
        //[Authorize]
        [HttpPost("analyseGameScore", Name = "AnalyseGameScore")]
        [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public ActionResult<bool> AnalyseGameResults([FromBody] GameResults gameResults)
        {
            if(gameResults == null)
                return BadRequest(new { Message = "Invalid game data" });

            var result = _rabbitMQRepository.EmitGameAnalysis(gameResults);
            if (!result) 
                return BadRequest(new { Message = "User not found" });

            var receivedData = _rabbitMQRepository.ReceiveGameAnalysis();

            return Ok(receivedData);
        }

        // Receiving video data from the React client
        //[Authorize]
        [HttpPost("analyseVideo", Name = "AnalyseVideo")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public ActionResult<bool> AnalyseVideoResult([FromForm] List<IFormFile> videoData, string userName)
        {
            var user = _userRepository.GetUser(userName);

            if (user.Result.Id == -1 ) 
                return BadRequest(new { Message = "User not found" });

            if (videoData.Count < 1 || videoData.Count > 1)
                return BadRequest(new { Message = "Please only send one video file" });

            if (!videoData[0].ContentType.Contains("video"))
            {
                return BadRequest(new { Message = "Please upload a video" });
            }

            // Send the first file in the array for analysis
            _logger.LogInformation("Sending video file to python script for analysis");
            _rabbitMQRepository.EmitVideoAnalysis(videoData[0]);
            var fileName = videoData[0].FileName;

            _logger.LogInformation("Waiting for response from python script");

            string response = _rabbitMQRepository.ReceiveVideoAnalysis();

            _logger.LogInformation("Adding analysis results into database");
            if (!(_userRepository.AddVideoAnalysisData(fileName, user.Result.Id, response).Result))
                return BadRequest(new { Message = "Not able to add data into database" });

            if (response == "The request has timed out")
                return BadRequest(new { Message = "The request has timed out" });

            return Ok($"File analysed: " +
                $"{response}");
        }
    }
}
