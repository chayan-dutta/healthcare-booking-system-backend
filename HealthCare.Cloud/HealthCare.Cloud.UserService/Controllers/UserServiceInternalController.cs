using HealthCare.Cloud.UserService.Models;
using HealthCare.Cloud.UserService.Repository;
using HealthCare.Common.Authorization;
using HealthCare.Common.Models;
using Microsoft.AspNetCore.Mvc;

namespace HealthCare.Cloud.UserService.Controllers
{
    /// <summary>
    /// The APIs will be used by the services
    /// </summary>
    [ApiController]
    [Route("api/users/internal")]
    [Tags("Internal APIs of User Service")]
    public partial class UserServiceInternalController : ControllerBase
    {
        private readonly IUserDataRepository _userDataRepo;
        private readonly ILogger<UserServiceInternalController> _logger;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="userDataRepository"></param>
        /// <param name="logger"></param>
        public UserServiceInternalController(IUserDataRepository userDataRepository
            , ILogger<UserServiceInternalController> logger)
        {
            _logger = logger;
            _userDataRepo = userDataRepository;
        }

        /// <summary>
        /// Internal API for Creating user. Should be used by other services only 
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("createuser")]
     //   [InternalAuth]
        public async Task<ApiResponse<AddUserResponse>> AddUser([FromBody] CreateUser user)
        {           
            Guid userId =  await _userDataRepo.AddUserToDatabase(user);

            var addUserResponse = new AddUserResponse
            {
                UserId = userId,
                Email = user.Email,
            };

            return new ApiResponse<AddUserResponse>
            {
                Data = addUserResponse,
                IsSuccess = true,
                Message = "User added successfully",
                Status = System.Net.HttpStatusCode.Created
            };
        }
    }
}
