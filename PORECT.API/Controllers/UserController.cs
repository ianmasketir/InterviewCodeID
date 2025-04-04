﻿using System.Net;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using PORECT.Helper;
using Tes.Business;
using Tes.Domain;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PORECT.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ParentController
    {
        private readonly IUserRepository _repository;
        public UserController(IUserRepository userRepository)
        {
            _repository = userRepository;
        }
        //public UserController(IServiceProvider serviceProvider) : base(serviceProvider)
        //{

        //}

        #region View
        // GET: api/<UserController>
        [HttpGet]
        [Route("List")]
        public IActionResult GetList([FromQuery] int? id = null, string? username = null)
        {
            try
            {
                var data = _repository.GetListUser(id, username);
                return Ok(data);
            }
            catch (System.Exception ex)
            {
                logger.WriteErrorToLog(ex, "User", "GetList");
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
        }
        #endregion View

        #region Transaction
        /// <summary>
        /// This API for insert/update/disable/enable user.
        /// </summary>
        /// <param name="data"></param>
        /// <returns>Response model.</returns>
        /// <remarks>
        /// Sample request:
        /// 
        ///     POST /Product/Submit
        ///     {
        ///         "Username": "Tes-001",
        ///         "TransactionType": "Insert/Update/Delete/Disable/Enable"
        ///     }
        /// </remarks>
        [HttpPost]
        [Route("Submit")]
        [ProducesResponseType(typeof(TransactionResponse), (int)HttpStatusCode.OK)]
        public IActionResult Submit([FromBody] MsUserRequest data)
        {
            try
            {
                if(data == null)
                {
                    return BadRequest("Parameter is empty!");
                }

                var result = _repository.SubmitUser(data);
                return Ok(result);
            }
            catch (System.Exception ex)
            {
                logger.WriteErrorToLog(ex, "User", "Submit");
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
        }
        #endregion Transaction

    }
}
