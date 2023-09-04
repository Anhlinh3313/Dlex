using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Core.Data.Core.Utils;
using Core.Entity.Abstract;

namespace Core.Business.ViewModels
{
    public class OpenPackageViewModel : IEntityBase
    {
        public OpenPackageViewModel()
        {
        }

        public int Id { get; set; }
        public int? OpenPackageHubId { get; set; }
        public bool IsEnabled { get; set; }
        public int? CompanyId { get; set; }
    }
}
