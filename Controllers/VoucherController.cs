using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using XeniaRentalApi.DTOs;
using XeniaRentalApi.Models;
using XeniaRentalApi.Repositories.Voucher;


namespace XeniaRentalApi.Controllers
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]
    public class VoucherController : ControllerBase
    {
        private readonly IVoucherRepository _voucherRepository;


        public VoucherController(IVoucherRepository voucherRepository)
        {
            _voucherRepository = voucherRepository;
        }


        [HttpGet("all/accounts")]
        public async Task<ActionResult<IEnumerable<XRS_Voucher>>> Get()
        {
            var vouchers = await _voucherRepository.GetVouchers();
            if (vouchers == null || !vouchers.Any())
            {
                return NotFound(new { Status = "Error", Message = "No Vouchers found." });
            }
            return Ok(new { Status = "Success", Data = vouchers });
        }

        [HttpGet("all/paymentStatus")]
        public async Task<ActionResult<IEnumerable<XRS_Voucher>>> GetPaymentStatus()
        {
            var vouchers = await _voucherRepository.GetPaymentStatus();
            if (vouchers == null || !vouchers.Any())
            {
                return NotFound(new { Status = "Error", Message = "No Vouchers found." });
            }
            return Ok(new { Status = "Success", Data = vouchers });
        }


        [HttpGet("accounts/{companyId}")]
        public async Task<ActionResult<IEnumerable<XRS_Voucher>>> GetVouchersByCompanyId(int companyId)
        {

            var vouchers = await _voucherRepository.GetVoucherByCompanyId(companyId);
            if (vouchers == null || !vouchers.Any())
            {
                return NotFound(new { Status = "Error", Message = "No vouchers found the given Company ID." });
            }
            return Ok(new { Status = "Success", Data = vouchers });
        }


        [HttpPost]
        public async Task<IActionResult> CreateVouchers([FromBody] CreateVoucher voucher)
        {
            if (voucher == null)
            {
                return BadRequest(new { Status = "Error", Message = "Invalid voucher." });
            }

            var createdVoucher   = await _voucherRepository.CreateVoucher(voucher);
            return CreatedAtAction(nameof(GetVoucher), new { id = createdVoucher }, new { Status = "Success", Data = createdVoucher });
        }

        //[Authorize(Roles = "Admin,SuperAdmin")]
        [HttpGet("{id}")]
        public async Task<ActionResult<XRS_Voucher>> GetVoucher(int id)
        {
            var vouchers = await _voucherRepository.GetVoucherIdById(id);
            if (vouchers == null)
            {
                return NotFound(new { Status = "Error", Message = "Voucher not found." });
            }
            return Ok(new { Status = "Success", Data = vouchers });
        }



        [HttpPut("UpdateVoucher/{id}")]
        public async Task<IActionResult> UpdateVoucher(int id, [FromBody] Models.XRS_Voucher voucher)
        {
            if (voucher == null)
            {
                return BadRequest(new { Status = "Error", Message = "Invalid voucher data" });
            }

            var updated = await _voucherRepository.UpdateVoucher(id, voucher);
            if (!updated)
            {
                return NotFound(new { Status = "Error", Message = "voucher not found or update failed." });
            }

            return Ok(new { Status = "Success", Message = "Voucher updated successfully." });
        }

        [HttpGet("Vouchers/search")]
        public async Task<ActionResult<PagedResultDto<XRS_Voucher>>> Get(
         string? search,
         int pageNumber = 1,
         int pageSize = 10)
        {
            var result = await _voucherRepository.GetVochersAsync(search, pageNumber, pageSize);
            return Ok(result);
        }
    }
}
