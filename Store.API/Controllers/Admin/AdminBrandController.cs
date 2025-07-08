using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Store.Application.Abstractions.Admin;
using Store.Contracts.AdminContracts.Request.BrandDTO;
using Store.Contracts.AdminContracts.Response.AdminBrandDTO;

namespace Store.API.Controllers.Admin
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminBrandController : ControllerBase
    {
        private readonly IAdminBrandService _adminBrandService;

        public AdminBrandController(IAdminBrandService adminBrandService)
        {
            _adminBrandService = adminBrandService;
        }

        [HttpGet("get-all")]
        public async Task<ActionResult<ICollection<AdminReadBrandDTO>>> GetAllBrands()
        {
            try
            {
                var brands = await _adminBrandService.GetAllBrands();

                return Ok(brands);
            }
            catch(Exception ex)
            {
                return NotFound(new { message = ex.Message }); 
            }
        }

        [HttpGet("get-{id:guid}")]
        public async Task<ActionResult<AdminReadBrandDTO>> GetBrandById(Guid id)
        {
            try
            {
                var brand = await _adminBrandService.GetBrandById(id);

                return Ok(brand);
            }
            catch(Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpPost("create")]
        public async Task<ActionResult<Guid>> CreateBrand([FromBody] AdminCreateBrandDTO brandDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest("Bad data");

            try
            {
                var result = await _adminBrandService.CreateBrand(brandDTO);

                return Ok(result);
            }
            catch(Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("update/{id:guid}")]
        public async Task<ActionResult<Guid>> UpdateBrand(Guid id, [FromBody] AdminUpdateBrandDTO brandDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest("Bad data");

            try
            {
                var result = await _adminBrandService.UpdateBrand(id, brandDTO);

                return Ok(result);
            }
            catch(Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("delete/{id:guid}")]
        public async Task<ActionResult<Guid>> DeleteBrand(Guid id)
        {
            try
            {
                var result = await _adminBrandService.DeleteBrand(id);

                return Ok(result);
            }
            catch(Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
