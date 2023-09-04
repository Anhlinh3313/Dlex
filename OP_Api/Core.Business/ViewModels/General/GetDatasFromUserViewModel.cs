using System;
using System.Collections.Generic;
using Core.Entity.Entities;
using Core.Entity.Procedures;

namespace Core.Business.ViewModels.General
{
    public class GetDatasFromUserViewModel
    {
        public GetDatasFromUserViewModel()
        {
        }

        public string Location { get; set; }
        public IEnumerable<User> User { get; set; }
    }
}
