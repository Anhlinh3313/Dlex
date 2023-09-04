using Core.Data.Core.Utils;
using Core.Entity.Abstract;
using Core.Entity.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Linq;

namespace Core.Business.ViewModels
{

    public class AreaViewModel : SimpleViewModel, IValidatableObject
    {
        public AreaViewModel() { }

        public int? AreaGroupId { set; get; }
        public bool IsAuto { get; set; }
        public double ExpectedTime { get; set; }
        public int[] DistrictIds { set; get; }
        public int[] ProvinceIds { get; set; }
        public int[] MultiSelectProvinces { get; set; }
        public int[] MultiSelectDistrict { get; set; }
        public int[] MultiSelectFromProvince { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var validator = new AreaViewModelValidator(EntityUtil.GetUnitOfWork(validationContext));
            var result = validator.Validate(this);
            return result.Errors.Select(item => new ValidationResult(item.ErrorMessage, new[] { item.PropertyName }));
        }
    }
}
