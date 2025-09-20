using XeniaRentalApi.DTOs;

namespace XeniaRentalApi.Repositories.Charges
{
    public interface IChargesRepository
    {
        Task<IEnumerable<ChargesDto>> GetCharges(int companyId, int? propertyId = null);
        Task<PagedResultDto<ChargesDto>> GetChargesByCompanyId(int companyId, int pageNumber, int pageSize);
        Task<ChargesDto?> GetChargesById(int chargeId);

        Task<Models.XRS_Charges> CreateCharges(DTOs.ChargesDto charges);

        Task<bool> DeleteCharges(int id);

       

        Task<bool> UpdateCharges(int id, Models.XRS_Charges charges);
        Task<PagedResultDto<Models.XRS_Charges>> GetChargesAsync(string? chargeName, string? propertyName, int pageNumber, int pageSize);
    }
}
