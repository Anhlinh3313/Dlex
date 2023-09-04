using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Mail;
using Core.Business.Services.Abstract;
using Core.Business.Services.Models;
using Core.Data.Abstract;
using Core.Entity.Entities;
using Core.Entity.Procedures;
using Core.Infrastructure.Utils;
using Core.Infrastructure.ViewModels;
using LinqKit;
using Microsoft.Extensions.Options;

namespace Core.Business.Services
{
    public class UserService : BaseService, IUserService
    {
        public UserService(Microsoft.Extensions.Logging.ILogger<dynamic> logger, IOptions<AppSettings> optionsAccessor, IUnitOfWork unitOfWork) : base(logger, optionsAccessor, unitOfWork)
        {
        }

        public List<int> GetListHubFromUser(User user)
        {
            var listHub = new List<int>();

            if (user.Hub.CenterHubId.HasValue && !user.Hub.PoHubId.HasValue)
            {
                listHub = _unitOfWork.RepositoryR<Hub>().FindBy(x => x.PoHubId == user.HubId).Select(x => x.Id).ToList();
            }
            else if (!user.Hub.CenterHubId.HasValue && !user.Hub.PoHubId.HasValue)
            {
                listHub = _unitOfWork.RepositoryR<Hub>().FindBy(x => x.CenterHubId == user.HubId).Select(x => x.Id).ToList();
            }

            //if (user.Hub.PoHubId.HasValue)
            //{

            //}
            //else 

            listHub.Add((int)user.HubId);

            return listHub;
        }

        public List<int> GetListHubFromUserByProc(User user)
        {
            var listHub = new List<int>();
            var data = _unitOfWork.Repository<Proc_GetListHubFromUser>()
                      .ExecProcedure(Proc_GetListHubFromUser.GetEntityProc(user.Id));

            listHub = data.Select(p => p.Id).ToList();
            //if (user.Hub.PoHubId.HasValue)
            //{

            //}
            //else 
            listHub.Add((int)user.HubId);
            return listHub;
        }

        public List<int> GetListHubFromHubId(int? hubId)
        {
            Expression<Func<Hub, bool>> predicate = x => x.Id > 0;
            if (!Util.IsNull(hubId) && hubId != 0)
            {
                predicate = predicate.And(f => f.Id == hubId);
            }
            var hub = _unitOfWork.RepositoryR<Hub>().GetSingle(predicate);
            var listHub = new List<int>();

            if (hub.CenterHubId.HasValue && !hub.PoHubId.HasValue)
            {
                listHub = _unitOfWork.RepositoryR<Hub>().FindBy(x => x.PoHubId == hubId).Select(x => x.Id).ToList();
            }
            else if (!hub.CenterHubId.HasValue && !hub.PoHubId.HasValue)
            {
                listHub = _unitOfWork.RepositoryR<Hub>().FindBy(x => x.CenterHubId == hubId).Select(x => x.Id).ToList();
            }

            listHub.Add((int)hubId);

            return listHub;
        }

        public List<int> GetListAllHubByHubId(int hubId)
        {
            var hub = _unitOfWork.RepositoryR<Hub>().GetSingle(hubId);
            var listHub = new List<int>();
            listHub = _unitOfWork.RepositoryR<Hub>().FindBy(x => x.CenterHubId == hub.Id || x.PoHubId == hub.Id || x.Id == hub.Id).Select(x => x.Id).ToList();
            return listHub;
        }

        public object SendEmail(SendMail sendEmailOptions, EmailRecipient emailRecipient)
        {
            SmtpClient client = new SmtpClient();
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.EnableSsl = sendEmailOptions.EnableSsl;
            client.Host = sendEmailOptions.Host;
            var credentials = new System.Net.NetworkCredential(sendEmailOptions.MailFrom, sendEmailOptions.PassWordMailFrom);
            client.UseDefaultCredentials = true;
            client.Credentials = credentials;
            MailMessage mess = new MailMessage();
            mess.From = new MailAddress(sendEmailOptions.MailFrom, sendEmailOptions.MailFromDisplayName + " [Reset PassWord]");
            mess.To.Add(emailRecipient.Email);
            mess.Subject = "Đăng nhập " + sendEmailOptions.Website;
            mess.IsBodyHtml = true;
            string content = System.IO.File.ReadAllText(sendEmailOptions.Path);
            content = content.Replace("{{Email}}", emailRecipient.Email);
            content = content.Replace("{{PassWowrd}}", emailRecipient.PasswordHash);
            content = content.Replace("{{Link}}", sendEmailOptions.Website);

            mess.Body = content;
            try
            {
                client.Send(mess);
                return true;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}
