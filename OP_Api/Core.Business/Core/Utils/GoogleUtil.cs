using Core.Business.ViewModels;
using Core.Infrastructure.Api;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Business.Core.Utils
{
    public static class GoogleUtil
    {
        public static double GetDistancs(GoogleDistnaceModel viewModel, string apiKey)
        {
            double distance = 0;
            try
            {
                ApiGoogle apiGoogle = new ApiGoogle();
                var aResult = apiGoogle.GetDistance(apiKey, viewModel.AddressFrom, viewModel.AddressTo, viewModel.LatFrom, viewModel.LngFrom, viewModel.LatTo, viewModel.LngTo);
                var result = aResult.Result;
                if(result.status == "OK")
                {
                    foreach(var row in result.rows)
                    {
                        foreach(var element in row.elements)
                        {
                            if(element.status == "OK")
                            {
                                distance += element.distance.value;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return distance / 1000; // convert m => km
        }
    }
}
