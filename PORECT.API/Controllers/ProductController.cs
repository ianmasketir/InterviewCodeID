using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Tes.Domain;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PORECT.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("corsapp")]
    public class ProductController : ParentController
    {
        #region View
        // GET: api/<UserController>
        [HttpGet("List")]
        public async Task<IActionResult> GetList([FromQuery] int? id, string? code, string? name)
        {
            try
            {
                var data = (await _processor.GetListProduct()).Where(x=>
                            (id == null || x.ID == id) && 
                            (string.IsNullOrEmpty(code?.Trim()) || x.ProductCode?.Trim().ToLower() == code.Trim().ToLower()) &&
                            (string.IsNullOrEmpty(name?.Trim()) || x.Name?.ToLower() == name.Trim().ToLower())
                           ).ToList();
                return Ok(data);
            }
            catch (System.Exception ex)
            {
                logger.WriteErrorToLog(ex, "Product", "GetList");
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
        }
        #endregion View

        #region Transaction
        /// <summary>
        /// This API for insert/update/delete product.
        /// </summary>
        /// <param name="data"></param>
        /// <returns>Response model.</returns>
        /// <remarks>
        /// Sample request:
        /// 
        ///     POST /Product/Submit
        ///     {
        ///         "ProductCode": "Tes-001",
        ///         "TransactionType": "Insert/Update/Delete"
        ///     }
        /// </remarks>
        [HttpPost("Submit")]
        public async Task<IActionResult> Submit([FromBody] ProductRequest data)
        {
            try
            {
                var result = await _processor.SubmitProduct(data);
                return Ok(result);
            }
            catch (System.Exception ex)
            {
                logger.WriteErrorToLog(ex, "Product", "Submit");
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
        }
        #endregion Transaction

    }
}
