namespace XeniaRentalApi.Service.Common
{
    public interface IGeneralService
    {

        decimal getRoundedValue(decimal price, int decimalPlaces);
        double MeterToKiloMeter(double meters);

    }
}
