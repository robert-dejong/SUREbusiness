using System.ComponentModel.DataAnnotations;

namespace SUREbusiness.FleetManagement.DAL.Entities
{
    public class VehicleEntity
    {
        [Key]
        public int Id { get; set; }
        public string LicensePlate { get; set; }
        public string Color { get; set; }
        public int? YearOfManufacture { get; set; }
        public string LoanedTo { get; set; }
        public string Status { get; set; }
        public string Remarks { get; set; }
    }
}
