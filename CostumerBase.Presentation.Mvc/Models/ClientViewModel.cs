using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CostumerBase.Presentation.Mvc.Models
{
    public class ClientViewModel:PageModel
    {
        public Guid Id { get; set; }
        public string NameClient { get; set; }
        public string Logo { get; set; }
        public string Email { get; set; }
        public List<AddressViewModel> AddressClient { get; set; }
    }

}
