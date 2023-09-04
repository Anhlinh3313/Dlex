using Core.Data.Core.Utils;
using Core.Entity.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Core.Business.ViewModels
{
    public class WeightViewModel : SimpleViewModel, IValidatableObject
    {
        public WeightViewModel() { }

        public double? WeightFrom { set; get; }
        public double? WeightTo { set; get; }
        public double? WeightPlus { set; get; }
        public int FormulaId { set; get; }
        public int? PricingTypeId { set; get; }
        public int? WeightGroupId { set; get; }
        public bool IsAuto { get; set; }
        public bool IsWeightCal { get; set; }
        public int? StructureId { get; set; }
        public double? ValueFrom { get; set; }
        public double? ValueTo { get; set; }
        //public int? PricingTypeId { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var validator = new WeightViewModelValidator(EntityUtil.GetUnitOfWork(validationContext));
            var result = validator.Validate(this);
            return result.Errors.Select(item => new ValidationResult(item.ErrorMessage, new[] { item.PropertyName }));
        }
    }
}
