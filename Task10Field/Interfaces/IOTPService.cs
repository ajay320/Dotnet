namespace Task10Field.Interfaces
{
    public interface IOTPService
    {
        string GenerateOTP();
        Task<bool> SendOTPAsync(string mobileNumber, string otp);
    }

}
