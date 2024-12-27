using System.Net;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManagementSystem.Models;
using TaskManagementSystem.Models.DTOs;
using TaskManagementSystem.Models.Repository;

namespace TaskManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "adminToken", Roles ="Admin")]
    public class adminController : ControllerBase
    {
        private readonly ITaskRepo _taskRepo;
        private readonly IUserRepo _userRepo;
        private readonly IMapper _mapper;
        private readonly ResponseAPI _responseAPI;

        public adminController(IMapper mapper, ITaskRepo taskRepo, IUserRepo userRepo)
        {
            _mapper = mapper;
            _taskRepo = taskRepo;
            _responseAPI = new();
            _userRepo = userRepo;
        }

        [HttpGet]
        [Route("GetALLUsers")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<ResponseAPI>> GetAllUsers()
        {
            try
            {
                var users = await _userRepo.getAll();
                var uuser = _mapper.Map<List<UserDTO>>(users);
                _responseAPI.Status = true;
                _responseAPI.StatusCode = HttpStatusCode.OK;
                _responseAPI.Data = uuser;
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

        [HttpPost]
        [Route("createTask")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<ResponseAPI>> CreateTask([FromBody] TaskDTO model)
        {
            try
            {
                if (model == null)
                    return BadRequest("Please provide valid data.");

                var task = _mapper.Map<TasksTable>(model);

                var assignedUser = await _userRepo.getData(x => x.Id == task.AssignedTo);
                if (assignedUser == null)
                    return BadRequest("Assigned user not found.");

                var createdUser = await _userRepo.getData(x => x.Id == task.CreatedBy);
                if (createdUser == null)
                    return BadRequest("Created user not found.");

                task.AssignedUser = assignedUser;
                task.CreatedUser = createdUser;

                await _taskRepo.CreateAsync(task);

                var response = _mapper.Map<TaskResponseDTO>(task);

                _responseAPI.Status = true;
                _responseAPI.StatusCode = HttpStatusCode.OK;
                _responseAPI.Data = response;

                return Ok(_responseAPI);
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
                return _responseAPI;
            }
        }

        [HttpGet]
        [Route("GetUser/{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<ResponseAPI>> GetUserbyID(int id)
        {
            try
            {
                if (id <= 0)
                    return BadRequest("Provide Valid ID ");
                var user = await _userRepo.getData(x => x.Id == id);
                if (user == null)
                    return NotFound("User not exsist with this ID ");
                var uuser = _mapper.Map<UserDTO>(user);
                _responseAPI.Status = true;
                _responseAPI.StatusCode = HttpStatusCode.OK;
                _responseAPI.Data = uuser;
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
                return _responseAPI;
            }
        }

        [HttpDelete]
        [Route("DelteUser/{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<ResponseAPI>> DelteUserbyID(int id)
        {
            try
            {
                if (id <= 0)
                    return BadRequest("Please Put valid ID");
                var user = await _userRepo.getData(x => x.Id == id);
                if (user == null)
                    return NotFound("User not found with this ID");
                var tasks = await _taskRepo.findAll(t => t.AssignedTo == id);
                foreach (var task in tasks)
                {
                    await _taskRepo.Delte(task);
                }
                await _userRepo.Delte(user);
                _responseAPI.Status = true;
                _responseAPI.StatusCode = HttpStatusCode.OK;
                _responseAPI.Data = "Delte Sucessful";
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
                return _responseAPI;
            }
        }

        [HttpGet]
        [Route("GetAllTasks")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<ResponseAPI>> GetAllTasks()
        {
            try
            {
                var tasks = await _taskRepo.getAll();
                if (tasks == null)
                    return NotFound("No Tasks Exsist");
                _responseAPI.Status = true;
                _responseAPI.StatusCode = HttpStatusCode.OK;
                _responseAPI.Data = tasks;
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
                return _responseAPI;
            }
        }

        [HttpDelete]
        [Route("DelteTask/{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<ResponseAPI>> DelteTaskbyID(int id)
        {
            try
            {
                if (id <= 0)
                    return BadRequest("Please Put valid ID");

                var tasks = await _taskRepo.getData(x => x.Id == id);
                if (tasks == null)
                    return NotFound("Task not found with this ID");

                await _taskRepo.Delte(tasks);
                _responseAPI.Status = true;
                _responseAPI.StatusCode = HttpStatusCode.OK;
                _responseAPI.Data = "Delte Sucessful";
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
                return _responseAPI;
            }
        }
    }
}
