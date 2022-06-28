using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Project_one.JwtAuthentication;
using Project_one.Model;
using Project_one.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Project_one.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IJwtAuthenticationManager jwtAuthenticationManager;

        public UserController(IUserRepository userRepository, IJwtAuthenticationManager jwtAuthenticationManager)
        {
            _userRepository = userRepository;
            this.jwtAuthenticationManager = jwtAuthenticationManager;
        }

        // GET: api/<UserController>
        
        [HttpGet]
        public IActionResult Get()
        {
            var users = _userRepository.GetUsers();
            return new OkObjectResult(users);
        }

        // GET api/<UserController>/5
        [HttpGet("{id}", Name ="Get")]
        public IActionResult Get(int id)
        {
            var user = _userRepository.GetUserByID(id);
            return new OkObjectResult(user);
        }

        // POST api/<UserController>
        
        [HttpPost("register")]
        public IActionResult RegisterUser([FromBody] User  user)
        {
            using (var scope = new TransactionScope())
            {
                var f=_userRepository.InsertUser(user);
                scope.Complete();
                //return CreatedAtAction(nameof(Get), new { id = user.Id }, user);
                return new OkObjectResult(f);
            }
        }

        // PUT 
        [Authorize]
        [HttpPut("UpdateUser")]
        public IActionResult UpdateUser([FromBody] User user)
        {
            if(user != null)
            {
                using (var scope = new TransactionScope())
                {
                    _userRepository.UpdateUser(user);
                    scope.Complete();
                    return new OkResult();
                }
            }
            return new NoContentResult();
        }

        // DELETE api/<UserController>/5
        [Authorize]
        [HttpDelete("Delete")]
        public IActionResult Delete(int id)
        {
            _userRepository.DeleteUser(id);
            return new OkObjectResult("user deleted!!!!");
        }

        [AllowAnonymous]
        [HttpPost("Userlogin")]
        public IActionResult Userlogin(string EmailId, string PassKey)
        {

           var token = jwtAuthenticationManager.Authenticate(EmailId, PassKey);
            var user = JsonConvert.SerializeObject(token);
            if (user == null)
            {
                return Unauthorized();
            }
            return Ok(user);
        }
    }
}
