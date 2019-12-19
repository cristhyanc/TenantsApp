using Acr.UserDialogs;
using Microsoft.AppCenter.Crashes;
using System;
using System.Collections.Generic;
using System.Text;
using TenantsApp.Resources;
using TenantsApp.Shared.Exceptions;

namespace TenantsApp.Helpers
{
   public class ExceptionHelper
    {
        public static void ProcessException(Exception ex, IUserDialogs userDialogs, string pageTitle, string methodName = "")
        {

            if (ex is ValidationException appException && userDialogs != null)
            {
                userDialogs.Alert(new AlertConfig
                {
                    Message = ex.Message,
                    Title = StringResources.Validation,
                    OkText = StringResources.Ok
                }); ; ;
            }
            else
            {

                if (string.IsNullOrEmpty(methodName))
                {
                    methodName = StringResources.GeneralError;
                }
                Crashes.TrackError(ex);

                if (userDialogs != null)
                {
                    userDialogs.Alert(new AlertConfig
                    {
                        Message = StringResources.GeneralError,
                        Title = pageTitle,
                        OkText = StringResources.Ok
                    });
                }

            }
        }
    }
}
