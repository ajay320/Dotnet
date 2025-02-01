namespace Task10Field.Models.Verification
{
    public class OTPVerification
    {
        public int Id { get; set; }
        public string MobileNumber { get; set; }
        public string OTP { get; set; }
        public DateTime GeneratedAt { get; set; }
        public bool IsVerified { get; set; }
    }

}
