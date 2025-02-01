using Task10Field.Interfaces;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace Task10Field.Models.Verification
{
    public class OTPService : IOTPService
    {
        public IConfiguration Configuration;
        public OTPService(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public string GenerateOTP()
        {
            Random random = new Random();
          var data=  random.Next(10000,99999).ToString();
            return data;
        }

        public async Task<bool> SendOTPAsync(string mobileNumber, string otp)
        {
            try
            {
                // Ensure the number is in E.164 format (prepend country code if needed)
                if (!mobileNumber.StartsWith("+"))
                {
                    mobileNumber = "+91" + mobileNumber;  
                }

               
                var accountSid = Configuration["Twilio:AccountSid"];
                var authToken = Configuration["Twilio:AuthToken"];
                var twilioPhoneNumber = Configuration["Twilio:PhoneNumber"];

               
                TwilioClient.Init(accountSid, authToken);

                
                var message = await MessageResource.CreateAsync(
                    body: $"Your OTP is: {otp}.And this is for Testing Ajay, It is valid for 5 minutes.",
                    from: new PhoneNumber(twilioPhoneNumber),
                    to: new PhoneNumber(mobileNumber)  
                );

                return message.ErrorCode == null; // Return true if sent successfully
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending OTP: {ex.Message}");
                return false;
            }
        }

    }
}
