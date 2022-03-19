using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WorkforceManagementAPI.BLL.Contracts;
using WorkforceManagementAPI.BLL.Services;
using WorkforceManagementAPI.Common;
using WorkforceManagementAPI.DAL.Entities;
using WorkforceManagementAPI.DTO.Models.Requests;
using WorkforceManagementAPI.DTO.Models.Responses;

namespace ProjectManagementApp.WEB.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TimeOffCOntroller : ControllerBase
    {
        private readonly IUserService _userService;
        private IValidationService _validationService;
        private ITimeOffService _timeOffService;
        private readonly IMapper _mapper;

        public TimeOffCOntroller(IValidationService validationService, IUserService userService, ITimeOffService timeOffService, IMapper mapper) : base()
        {
            _validationService = validationService;
            _userService = userService;
            _timeOffService = timeOffService;
            _mapper = mapper;
        }

        /// <summary>
        /// List all the existing timeOff requests in the database.
        /// </summary>
        /// <returns></returns>
        /// <response code="200">OK - Request succeeded.</response>
        /// <response code="401">Unauthorized - Please check the provided credentials.</response>
        /// <response code="403">Forbidden - Your credentials don't meet the required authorization level to access the resource. 
        ///Please, contact your administrator to get desired permissions.</response>
        [Authorize]
        [HttpGet]
        public async Task<List<TimeOffResponseDTO>> GetAllRequestsAsync()
        {
            var requests = await _timeOffService.GetAllAsync();

            return _mapper.Map<List<TimeOffResponseDTO>>(requests);
        }

        /// <summary>
        /// List all timeoff requests, created by the  logged user.
        /// </summary>
        /// <returns></returns>
        /// <response code="200">OK - Request succeeded.</response>
        /// <response code="401">Unauthorized - Please check the provided credentials.</response>
        /// <response code="403">Forbidden - Your credentials don't meet the required authorization level to access the resource. 
        ///Please, contact your administrator to get desired permissions.</response>
        [Authorize]
        [HttpGet("MyRequests")]
        public async Task<List<TimeOffResponseDTO>> GetMyRequestsAsync()
        {
            User currentUser = await _userService.GetCurrentUser(User);
            _validationService.EnsureUserExist(currentUser);

            var requests = await _timeOffService.GetMyTimeOffs(currentUser.Id);

            return _mapper.Map<List<TimeOffResponseDTO>>(requests);
        }

        /// <summary>
        /// Find a timeoff request by timeoff Id.
        /// </summary>
        /// <param name="timeOffId"></param>
        /// <returns></returns>
        /// <response code="200">OK - Request succeeded.</response>
        /// <response code="400">BadRequest - Request could not be understood by the server.</response>
        /// <response code="401">Unauthorized - Please check the provided credentials.</response>
        /// <response code="403">Forbidden - Your credentials don't meet the required authorization level to access the resource. 
        ///Please, contact your administrator to get desired permissions.</response>
        /// <response code="404">NotFound - Requested information does not exist in the server.</response>
        /// <response code="500">InternalServerError - Generic error occured in the server.</response>
        [Authorize]
        [HttpGet("{timeOffId}")]
        public async Task<TimeOffResponseDTO> GetRequestByIdAsync(Guid timeOffId)
        {
            User currentUser = await _userService.GetCurrentUser(User);
            _validationService.EnsureUserExist(currentUser);

            TimeOff timeOff = await _timeOffService.GetTimeOffAsync(timeOffId);
            _validationService.EnsureTimeOffExist(timeOff);

            return _mapper.Map<TimeOffResponseDTO>(timeOff);
        }

        /// <summary>
        /// Create a timeOff request [Request types (enum) - NonPaid(0), Paid(1), SickLeave(2)].
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <response code="201">Created - Request resulted in new resource created.</response>
        /// <response code="400">BadRequest - Request could not be understood by the server.</response>
        /// <response code="401">Unauthorized - Please check the provided credentials.</response>
        /// <response code="403">Forbidden - Your credentials don't meet the required authorization level to access the resource. 
        ///Please, contact your administrator to get desired permissions.</response>
        /// <response code="404">NotFound - Requested information does not exist in the server.</response>
        /// <response code="409">Conflict - The submitted entity ran into a conflict with an existing one.</response>
        /// <response code="500">InternalServerError - Generic error occured in the server.</response>
        [Authorize(Policy = "TeamMember")]
        [HttpPost]
        public async Task<ActionResult> CreateTimeOffAsync(TimeOffRequestDTO model)
        {
            User currentUser = await _userService.GetCurrentUser(User);
            _validationService.EnsureUserExist(currentUser);

            bool isCreated = await _timeOffService.CreateTimeOffAsync(model, currentUser.Id);

            if (isCreated && ModelState.IsValid)
            {
                return Created(nameof(HttpPostAttribute), String.Format(Constants.Created, "TimeOff request"));
            }
            else
            {
                return BadRequest(Constants.OperationFailed);
            }
        }

        /// <summary>
        /// Edit a timeoff request by timeoff Id [Request types - NonPaid(0), Paid(1), SickLeave(2)].
        /// </summary>
        /// <param name="timeOffId"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <response code="200">OK - Request succeeded.</response>
        /// <response code="400">BadRequest - Request could not be understood by the server.</response>
        /// <response code="401">Unauthorized - Please check the provided credentials.</response>
        /// <response code="403">Forbidden - Your credentials don't meet the required authorization level to access the resource. 
        ///Please, contact your administrator to get desired permissions.</response>
        /// <response code="404">NotFound - Requested information does not exist in the server.</response>
        /// <response code="409">Conflict - The submitted entity ran into a conflict with an existing one.</response>
        /// <response code="500">InternalServerError - Generic error occured in the server.</response>
        [Authorize(Policy = "TimeOffCreatorOrAdmin")]
        [HttpPut("{timeOffId}")]
        public async Task<ActionResult<TimeOffResponseDTO>> EditTimeOffAsync(Guid timeOffId, TimeOffRequestDTO model)
        {
            User currentUser = await _userService.GetCurrentUser(User);
            _validationService.EnsureUserExist(currentUser);

            bool isEdited = await _timeOffService.EditTimeOffAsync(timeOffId, model, currentUser);

            if (!isEdited)
            {
                return BadRequest(Constants.OperationFailed);
            }

            TimeOff timeOff = await _timeOffService.GetTimeOffAsync(timeOffId);
            _validationService.EnsureTimeOffExist(timeOff);

            return _mapper.Map<TimeOffResponseDTO>(timeOff);
        }

        /// <summary>
        /// REMOVE a timeoff request by timeoff Id.
        /// </summary>
        /// <param name="timeOffId"></param>
        /// <returns></returns>
        /// <response code="200">OK - Request succeeded.</response>
        /// <response code="400">BadRequest - Request could not be understood by the server.</response>
        /// <response code="401">Unauthorized - Please check the provided credentials.</response>
        /// <response code="403">Forbidden - Your credentials don't meet the required authorization level to access the resource. 
        ///Please, contact your administrator to get desired permissions.</response>
        /// <response code="404">NotFound - Requested information does not exist in the server.</response>
        /// <response code="409">Conflict - The submitted entity ran into a conflict with an existing one.</response>
        /// <response code="500">InternalServerError - Generic error occured in the server.</response>
        [Authorize(Policy = "TimeOffCreatorOrAdmin")]
        [HttpDelete("{timeOffId}")]
        public async Task<ActionResult> DeleteTimeOffAsync(Guid timeOffId)
        {
            User currentUser = await _userService.GetCurrentUser(User);
            _validationService.EnsureUserExist(currentUser);

            bool isDeleted = await _timeOffService.DeleteTimeOffAsync(timeOffId);
            if (!isDeleted)
            {
                return BadRequest(Constants.OperationFailed);
            }

            return Ok(String.Format(Constants.Deleted, "TimeOff request"));
        }

        /// <summary>
        /// APPROVE or REJECT a timeoff request using timeoff Id [Status values (enum) - Rejected(0), Approved(3)].
        /// </summary>
        /// <param name="timeOffId"></param>
        /// <param name="vote"></param>
        /// <returns></returns>
        /// <response code="200">OK - Request succeeded.</response>
        /// <response code="400">BadRequest - Request could not be understood by the server.</response>
        /// <response code="401">Unauthorized - Please check the provided credentials.</response>
        /// <response code="403">Forbidden - Your credentials don't meet the required authorization level to access the resource. 
        ///Please, contact your administrator to get desired permissions.</response>
        /// <response code="404">NotFound - Requested information does not exist in the server.</response>
        /// <response code="409">Conflict - The submitted entity ran into a conflict with an existing one.</response>
        /// <response code="500">InternalServerError - Generic error occured in the server.</response>
        [Authorize(Policy = "TeamLeader")]
        [HttpPost("SubmitFeedback/{timeOffId}")]
        public async Task<ActionResult> SubmitFeedbackForTimeOffRequestAsync(Guid timeOffId, CreateVoteRequestDTO vote)
        {
            User currentUser = await _userService.GetCurrentUser(User);
            _validationService.EnsureUserExist(currentUser);

            bool isCompleted = await _timeOffService.SubmitFeedbackForTimeOffRequestAsync(currentUser, timeOffId, vote.Status);
            if (!isCompleted)
            {
                return BadRequest(Constants.OperationFailed);
            }

            return Ok(Constants.AnswerToRequest);
        }

        [Authorize]
        [HttpGet("OffDays")]
        public async Task<OffDaysDTO> GetOffDays()
        {
            User user = await _userService.GetCurrentUser(User);

            return _mapper.Map<OffDaysDTO>(_timeOffService.GetOffDays(user));
        }
    }
}

