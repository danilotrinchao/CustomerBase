namespace CostumerBase.Presentation.Mvc.Models
{
    public class AddressViewModel
    {
        public Guid Id { get; set; }
        public string Country { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string Neighborhood { get; set; }
        public string Road { get; set; }
        public string Number { get; set; }
        public string Complement { get; set; }
        public Guid? ClientId { get; set; }
    }
}
