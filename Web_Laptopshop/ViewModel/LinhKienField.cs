using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web_Laptopshop.ViewModel
{
    public class LinhKienField
    {
        public string FieldName { get; set; }
        public string PlaceHolder { get; set; }
        public string InputName { get; set; }

        public LinhKienField(string fieldName, string placeHolder, string inputName)
        {
            FieldName = fieldName;
            PlaceHolder = placeHolder;
            InputName = inputName;
        }
    }
}