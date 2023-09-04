using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CostumerBase.Presentation.Mvc.Models;

namespace CostumerBase.Presentation.Mvc.Views.Home
{
    public class HomeModel : PageModel
    {
        public ClientViewModel _clientViewModelClientViewModel;
        public HomeModel(ClientViewModel clientViewModel) 
        {
            _clientViewModelClientViewModel= clientViewModel;
        }
        public void OnGet()
        {
            _ = _clientViewModelClientViewModel.Id;
        }
    }
}
