using System.Net;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using PORECT.Helper;
using Tes.Domain;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PORECT.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ParentController
    {
        #region View
        // GET: api/<UserController>
        [HttpGet]
        [Route("List")]
        public async Task<IActionResult> GetList([FromQuery] int? id, string? username)
        {
            try
            {
                var data = (await _processor.GetListUser()).Where(x=>
                            (id == null || x.ID == id) && 
                            (string.IsNullOrEmpty(username?.Trim()) || x.Username?.Trim().ToLower() == username.Trim().ToLower())
                           ).ToList();
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
        public async Task<IActionResult> Submit([FromBody] MsUserRequest data)
        {
            try
            {
                var result = await _processor.SubmitUser(data);
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
