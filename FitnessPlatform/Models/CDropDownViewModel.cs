using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FitnessPlatform.Models
{
    public class CDropDownViewModel
    {
        public int fTimeId { get; set; }
        public string fTimeName { get; set; }
        public List<NameDropDown> NameDropDownsProperty { get; set; }
    }

    public class NameDropDown
    {
        public int eatid { get; set; }
        public string eatname { get; set; }
    }
}