using XeniaRentalApi.DTOs;

namespace XeniaRentalApi.Repositories.BedSpace
{
    public interface IBedSpaceRepository
    {
        Task<IEnumerable<Models.BedSpace>> GetBedSpaces();
        Task<PagedResultDto<Models.BedSpace>> GetBedSpacebyCompanyId(int companyId, int pageNumber, int pageSize);

        Task<Models.BedSpace> CreateBedSpaces(DTOs.CreateBedSpace bedSpace);

        Task<bool> DeleteBedSpace(int id);

        Task<IEnumerable<Models.BedSpace>> GetBedSpacebyId(int Id);//UpdateBedSpace

        Task<bool> UpdateBedSpace(int id, Models.BedSpace bedSpace);
        Task<PagedResultDto<Models.BedSpace>> GetBedSpaceAsync(string? search, int pageNumber, int pageSize);

        Task<IEnumerable<Models.BedSpace>> GetBedSpacebyUnitId(int unitId);
    }
}
