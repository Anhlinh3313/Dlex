using Core.Infrastructure.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Business.Services.Abstract
{
    public interface IProviderCodeService
    {
        ResponseViewModel ValidatorListCode(List<string> listCode);
    }
}
