namespace SUREbusiness.FleetManagement.BLL.Models
{
    public class Vehicle
    {
        public int Id { get; set; }
        public string LicensePlate { get; set; }
        public string Color { get; set; }
        public int? YearOfManufacture { get; set; }
        public string LoanedTo { get; set; }
        public string Status { get; set; }
        public string Remarks { get; set; }
    }
}
