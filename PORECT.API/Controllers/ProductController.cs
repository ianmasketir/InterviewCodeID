using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Tes.Business;
using Tes.Domain;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PORECT.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ParentController
    {
        private readonly IProductRepository _repository;
        public ProductController(IProductRepository productRepository)
        {
            _repository = productRepository;
        }
        //public ProductController(IServiceProvider serviceProvider) : base(serviceProvider)
        //{

        //}

        #region View
        // GET: api/<UserController>
        [HttpGet("List")]
        public IActionResult GetList([FromQuery] SearchProductRequest dto)
        {
            try
            {
                var data = _repository.GetListProduct(dto);
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
        public IActionResult Submit([FromBody] ProductRequest data)
        {
            try
            {
                if (data == null)
                {
                    return BadRequest("Parameter is empty!");
                }

                var result = _repository.SubmitProduct(data);
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
