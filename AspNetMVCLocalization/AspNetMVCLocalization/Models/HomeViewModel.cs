using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace AspNetMVCLocalization.Models
{
    [BindProperties()]
    public class HomeViewModel
    {
        [Display(Name = "Name")]
        public string Name { get; set; }
    }
}
