using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FinClever.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinClever.Controllers
{
    [Authorize]
    [ApiController]
    [Route("/api/users")]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository userRepository;

        public UserController(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        [HttpPost]
        [Route("login")]
        public async Task<ActionResult<User>> Login()
        {
            var id = User.GetId();
            var existingUser = await userRepository.Get(id);
            if (existingUser != null)
            {
                return existingUser;
            }
            else
            {
                var user = new User(User.GetId(), User.GetName(), User.GetEmail(), User.GetImage());
                return await userRepository.Create(user);
            }
        }

        [HttpGet]
        public async Task<IEnumerable<User>> Users()
        {
            return await userRepository.Get();
        }

        [HttpGet]
        [Route("me")]
        public async Task<ActionResult<User>> Me()
        {
            return await userRepository.Get(User.GetId());
        }
    }
}
