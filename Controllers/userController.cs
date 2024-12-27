using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.IdentityModel.Tokens;
using TaskManagementSystem.Models;
using TaskManagementSystem.Models.DTOs;
using TaskManagementSystem.Models.Repository;
using TaskManagementSystem.Services;

namespace TaskManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class userController : ControllerBase
    {
        private readonly ResponseAPI _responseAPI;
        private readonly IMapper _mapper;
        private readonly IUserRepo _userRepo;
        private readonly ITaskRepo _takRepo;
        private readonly UserService _userService;
        private readonly IConfiguration _configuration;
        public userController(IMapper mapper, IUserRepo userRepo, UserService userService, IConfiguration configuration, ITaskRepo takRepo)
        {
            _userRepo = userRepo;
            _userService = userService;
            _mapper = mapper;
            _configuration = configuration;
            _takRepo = takRepo;
            _responseAPI = new();
        }

        [HttpPost]
        [Route("Register")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ResponseAPI>> Regitser([FromBody]UserDTO model)
        {
            try
            {
                if(model == null)
                    return BadRequest("Please Provide Valid Data");
                var hashedPassword = _userService.HashPassword(model.Password);
                var user = new UsersTable
                {
                    Name = model.Name,
                    Email = model.Email,
                    Password = hashedPassword,
                    Role = "User",
                    CreatedAt = model.CreatedAt
                };
                var createdUser = await _userRepo.CreateAsync(user);
                _responseAPI.Status = true;
                _responseAPI.StatusCode = HttpStatusCode.OK;
                _responseAPI.Data = createdUser;
                return _responseAPI;
            }
            catch (Exception ex)
            {
                _responseAPI.Errors.Add(ex.Message);
                _responseAPI.StatusCode = HttpStatusCode.InternalServerError;
                _responseAPI.Status = false;
                return _responseAPI;
            }
        }

        [HttpPost]
        [Route("Login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ResponseAPI>> Login([FromBody]LoginDTO model)
        {
            try
            {
                var base64Key = _configuration.GetValue<string>("Jwt:Key");
                var key = Encoding.UTF8.GetBytes(base64Key);
                if (model == null)
                    return BadRequest("Fill all Fields");
                var user = await _userRepo.getData(x => x.Email == model.Email);
                if (user == null)
                {
                    return Unauthorized(new { Message = "Invalid email or password." });
                }
                var isPasswordValid = _userService.VerifyPassword(user.Password, model.Password);
                if (!isPasswordValid)
                {
                    return Unauthorized(new { Message = "Invalid email or password." });
                }

                var tokenHandler = new JwtSecurityTokenHandler();
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new System.Security.Claims.ClaimsIdentity(new[]
                    {
                        new Claim(ClaimTypes.Name, user.Name),
                        new Claim(ClaimTypes.Role, user.Role)
                    }),
                    Expires = DateTime.UtcNow.AddHours(4), // Use UTC for expiration
                    SigningCredentials = new SigningCredentials(
                     new SymmetricSecurityKey(key),
                        SecurityAlgorithms.HmacSha512Signature
                    )
                };
                var token = tokenHandler.CreateToken(tokenDescriptor);
                var ttoken = tokenHandler.WriteToken(token);

                var response = new tokenResponse { 
                    Token = ttoken,
                    Line = "Login Sucessful"
                };
                _responseAPI.Status = true;
                _responseAPI.StatusCode = HttpStatusCode.OK;
                _responseAPI.Data = response;
                return _responseAPI;

            }
            catch(Exception ex)
            {
                if (_responseAPI.Errors == null)
                {
                    _responseAPI.Errors = new List<string>();
                }
                _responseAPI.Errors.Add(ex.Message);
                _responseAPI.StatusCode = HttpStatusCode.InternalServerError;
                _responseAPI.Status = false;
                return StatusCode((int)HttpStatusCode.InternalServerError, _responseAPI);
            }
        }

        [HttpGet]
        [Route("GetTask/{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(AuthenticationSchemes = "userToken", Roles ="User")]
        public async Task<ActionResult<ResponseAPI>> GetTask(int id)
        {
            try
            {
                if (id <= 0)
                    return BadRequest("Put valid ID ");
                var user = await _userRepo.getData(x=>x.Id == id);
                if (user == null)
                    return NotFound("User not Found with this ID ");
                var task = await _takRepo.getData(x => x.AssignedTo == id);
                _responseAPI.Status = true;
                _responseAPI.StatusCode = HttpStatusCode.OK;
                _responseAPI.Data = task;
                return _responseAPI;
            }
            catch (Exception ex) 
            {
                if (_responseAPI.Errors == null)
                {
                    _responseAPI.Errors = new List<string>();
                }
                _responseAPI.Errors.Add(ex.Message);
                _responseAPI.StatusCode = HttpStatusCode.InternalServerError;
                _responseAPI.Status = false;
                return StatusCode((int)HttpStatusCode.InternalServerError, _responseAPI);
            }
        }
    }
}
