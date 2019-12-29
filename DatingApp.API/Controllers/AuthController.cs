using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DatingApp.API.Data;
using DatingApp.API.DTOs;
using DatingApp.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _repo;

        public AuthController(IAuthRepository repo)
        {
            _repo = repo;
        }
        // GET api/values
        [HttpPost("register")]
        public async Task<IActionResult> Register(UserToRegisterDTO userToRegisterDTO)
        {
            userToRegisterDTO.UserName = userToRegisterDTO.UserName.ToLower();

            if (await _repo.UserExist(userToRegisterDTO.UserName))
                return BadRequest("user name already exists");

            var userToCreate = new User()
            {
                UserName = userToRegisterDTO.UserName
            };

            var createdUser = await _repo.Register(userToCreate, userToRegisterDTO.Password);

            return StatusCode(201);
        }


    }
}
