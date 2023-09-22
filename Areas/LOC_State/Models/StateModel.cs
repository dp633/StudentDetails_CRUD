using System.ComponentModel.DataAnnotations;

namespace StudentDetails.Areas.LOC_State.Models
{
    public class StateModel
    {
        public int StateID { get; set; }
        public int CountryID { get; set; }

        [Required(ErrorMessage = "Name is Requried")]
        public string? StateName { get; set; }

        [Required(ErrorMessage = "Code is Requried")]
        public string? StateCode { get; set; }
    }


    public class LOC_StateDropDownModel
    {
        public int StateID { get; set; }

        public string? StateName { get; set; }
    }
}
