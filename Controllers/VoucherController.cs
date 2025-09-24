using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using XeniaRentalApi.Dtos;
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


        [HttpPost]
        public async Task<ActionResult<XRS_Voucher>> CreateVoucher([FromBody] VoucherDto dto)
        {
            var voucher = await _voucherRepository.CreateVoucherAsync(dto);
            return CreatedAtAction(nameof(GetVoucherById), new { id = voucher.VoucherID }, voucher);
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<XRS_Voucher>> GetVoucherById(int id)
        {
            var voucher = await _voucherRepository.GetVoucherByIdAsync(id);
            if (voucher == null) return NotFound();
            return Ok(voucher);
        }


        [HttpGet("company/{companyId}")]
        public async Task<ActionResult<IEnumerable<XRS_Voucher>>> GetAllVouchers(int companyId)
        {
            var vouchers = await _voucherRepository.GetAllVouchersAsync(companyId);
            return Ok(vouchers);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<XRS_Voucher>> UpdateVoucher(int id, [FromBody] VoucherDto dto)
        {
            var updatedVoucher = await _voucherRepository.UpdateVoucherAsync(id, dto);
            if (updatedVoucher == null) return NotFound();
            return Ok(updatedVoucher);
        }


        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteVoucher(int id)
        {
            var deleted = await _voucherRepository.DeleteVoucherAsync(id);
            if (!deleted) return NotFound();
            return NoContent();
        }


        [HttpGet("{month:int}/{year:int}")]
        public async Task<IActionResult> GetTenantCharges(int month, int year)
        {
            var data = await _voucherRepository.GetTenantChargesByMonthAsync(month, year);
            return Ok(data);
        }



    }
}
