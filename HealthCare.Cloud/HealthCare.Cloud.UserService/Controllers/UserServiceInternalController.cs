using HealthCare.Cloud.UserService.Models;
using HealthCare.Cloud.UserService.Repository;
using HealthCare.Common.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HealthCare.Cloud.UserService.Controllers
{
    /// <summary>
    /// The APIs will be used by the services
    /// </summary>
    [ApiController]
    [Route("users/internal")]
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
        [InternalAuth]
        public async Task<Guid> AddUser([FromBody] CreateUser user)
        {
            return await _userDataRepo.AddUserToDatabase(user);
        }
    }
}
