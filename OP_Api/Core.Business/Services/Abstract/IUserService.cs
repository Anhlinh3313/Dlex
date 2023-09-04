using System;
using System.Collections.Generic;
using Core.Business.Services.Models;
using Core.Entity.Entities;

namespace Core.Business.Services.Abstract
{
    public interface IUserService
    {
        List<int> GetListHubFromUser(User user);
        List<int> GetListHubFromUserByProc(User user);
        List<int> GetListHubFromHubId(int? hubId);
        List<int> GetListAllHubByHubId(int hubId);
        object SendEmail(SendMail sendEmailOptions, EmailRecipient emailRecipient);
    }
}
