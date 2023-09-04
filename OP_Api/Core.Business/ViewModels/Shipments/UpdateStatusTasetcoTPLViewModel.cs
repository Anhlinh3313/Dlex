using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Core.Data.Core.Utils;
using Core.Entity.Abstract;
using FluentValidation;

namespace Core.Business.ViewModels
{
    public class UpdateStatusTasetcoTPLViewModel
    {
        public UpdateStatusTasetcoTPLViewModel()
        {
        }

        public string P_CODE { get; set; }
        public string E_CODE { get; set; }
        public string CUSTOMERCODE { get; set; }
        public string STATUS { get; set; }
        public string NOTE { get; set; }
        public string CITY { get; set; }
        public string WEIGHT { get; set; }
        public string COLLECT { get; set; }
        public string DELIVERYDATE { get; set; }
    }
}
