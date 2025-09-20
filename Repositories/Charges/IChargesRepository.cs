using XeniaRentalApi.DTOs;

namespace XeniaRentalApi.Repositories.Charges
{
    public interface IChargesRepository
    {
        Task<IEnumerable<ChargesDto>> GetCharges();
        Task<PagedResultDto<Models.XRS_Charges>> GetChargesByCompanyId(int companyId, int pageNumber, int pageSize);

        Task<Models.XRS_Charges> CreateCharges(DTOs.ChargesDto charges);

        Task<bool> DeleteCharges(int id);

        Task<IEnumerable<Models.XRS_Charges>> GetChargesbyId(int chargeId);

        Task<bool> UpdateCharges(int id, Models.XRS_Charges charges);
        Task<PagedResultDto<Models.XRS_Charges>> GetChargesAsync(string? chargeName, string? propertyName, int pageNumber, int pageSize);
    }
}
