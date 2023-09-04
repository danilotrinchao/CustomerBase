using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CostumerBase.Presentation.Mvc.Models
{
    public class ClientViewModel:PageModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Logotipo { get; set; }
        public string Email { get; set; }
        public List<AddressViewModel> Addresses { get; set; }
    }

}
