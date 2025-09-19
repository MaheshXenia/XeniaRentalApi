using XeniaRentalApi.DTOs;

namespace XeniaRentalApi.Repositories.Charges
{
    public interface IChargesRepository
    {
        Task<IEnumerable<Models.Charges>> GetCharges();
        Task<PagedResultDto<Models.Charges>> GetChargesByCompanyId(int companyId, int pageNumber, int pageSize);

        Task<Models.Charges> CreateCharges(DTOs.CreateCharges charges);

        Task<bool> DeleteCharges(int id);

        Task<IEnumerable<Models.Charges>> GetChargesbyId(int chargeId);

        Task<bool> UpdateCharges(int id, Models.Charges charges);
        Task<PagedResultDto<Models.Charges>> GetChargesAsync(string? chargeName, string? propertyName, int pageNumber, int pageSize);
    }
}
