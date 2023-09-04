using Core.Business.ViewModels.TruckSchedules;
using Core.Infrastructure.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Business.Services.Abstract
{
    public interface ITruckScheduleService
    {
        ResponseViewModel Search(TruckScheduleFilterViewModel model);
    }
}
