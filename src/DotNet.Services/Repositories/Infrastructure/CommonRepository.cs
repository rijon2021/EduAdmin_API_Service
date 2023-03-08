using DotNet.ApplicationCore.DTOs;
using Microsoft.AspNetCore.Http;
using DotNet.ApplicationCore.Utils.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotNet.Infrastructure.Persistence.Contexts;
using AutoMapper;
using DotNet.ApplicationCore.Entities;
using Microsoft.Extensions.Logging;
using DotNet.Services.Repositories.Interfaces.Common;
using DotNet.Services.Repositories.Infrastructure;
using DotNet.Services.Repositories.Interfaces;
using DotNet.ApplicationCore.Utils.Enum;
using Microsoft.EntityFrameworkCore;
using System.Net.Http.Headers;
using System.Configuration;
using static DotNet.ApplicationCore.Utils.Enum.GlobalEnum;

namespace DotNet.Services.Repositories.Common
{
    //IGenericRepository<Common>,
    public class CommonRepository : ICommonRepository
    {
        public DotNetContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;

        public CommonRepository(
            DotNetContext context,
            IHttpContextAccessor httpContextAccessor,
            IMapper mapper
            )
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
        }

        //Task<bool> SendSMS(int UserID, string recipientMobileNo, string messageBody, NotificationArea notificationArea, string otp);

        public async Task<bool> SendSMS(int UserID, string RecipientMobileNo, string messageBody, NotificationArea notificationArea, string otp)
        {
            try
            {
                SMSNotification notification = new SMSNotification();
                notification.UserID = UserID;
                notification.RecipientMobileNo = RecipientMobileNo;
                notification.NotificationAreaID = notificationArea.NotificationAreaID;
                notification.OTP = otp;
                notification.ExpireDateTime = DateTime.Now.AddMinutes(notificationArea.ExpireTime);
                notification.MessageBody = messageBody;

                SendSMSToServer(notification);
                _context.SMSNotifications.Add(notification);
                _context.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        private void SendSMSToServer(SMSNotification notification)
        {
            try
            {
                var globalSettings = _context.GlobalSettings.SingleOrDefault(x => x.GlobalSettingID == (int)GlobalSettingsEnum.SMS_Base_Url);

                if (globalSettings != null && globalSettings.IsActive == true && !string.IsNullOrEmpty(globalSettings.ValueInString))
                {
                    using (var client = new System.Net.Http.HttpClient())
                    {
                        client.BaseAddress = new Uri(globalSettings.ValueInString);
                        client.DefaultRequestHeaders.ExpectContinue = false;
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                        var content = new FormUrlEncodedContent(new[]
                        {
                        new KeyValuePair<string, string>("USER_CODE", "swl001"),
                        new KeyValuePair<string, string>("User_Pass", "123456"),
                        new KeyValuePair<string, string>("MOBILE_NUMBER", notification.RecipientMobileNo),
                        new KeyValuePair<string, string>("DTL_SMS", notification.MessageBody),
                        new KeyValuePair<string, string>("MASKING", "SWL")
                    });

                        var response = client.PostAsync("/api/SMS/PostSMSData", content).Result;

                        if (!response.IsSuccessStatusCode)
                        {
                            //return "";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
                throw new Exception("SMS sending failed.");
            }
        }

    }
}

