namespace XeniaRentalApi.DTOs
{
    public class CreateTenant
    {
        public int unitID { get; set; }

        /// <summary>
        /// UnitId
        /// required
        /// </summary>
        public int propID { get; set; }



        /// <summary>
        /// ComapnyId
        /// Required
        /// </summary>
        public int companyID { get; set; }

        /// <summary>
        /// securityAmt
        /// optional
        /// </summary>
        public string tenantName { get; set; }

        /// <summary>
        /// rentamt
        /// Optional
        /// </summary>
        public string phoneNumber { get; set; }

        /// <summary>
        /// collection type
        /// Optional
        /// </summary>
        public string email { get; set; }

        /// <summary>
        /// agreement start date
        /// </summary>
        public string emergencyContactNo { get; set; }

        /// <summary>
        /// agreement End date        
        /// </summary>
        public decimal concessionper { get; set; }

        /// <summary>
        /// rent Collection
        /// </summary>
        public string note { get; set; }

        /// <summary>
        /// escalationper
        /// </summary>
        public string address { get; set; }


        /// <summary>
        /// isactive
        /// </summary>
        public bool isActive { get; set; }
    }
}
