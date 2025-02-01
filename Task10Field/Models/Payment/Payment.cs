namespace Task10Field.Models.Payment
{
    using System.ComponentModel.DataAnnotations;

    public class Payment
    {
        public int Id { get; set; }

     
        public string Name { get; set; }

        
        public string Email { get; set; }

      
        public decimal Amount { get; set; }

        public string TransactionId { get; set; }

        public DateTime PaymentDate { get; set; } 
    }

}
