using Microsoft.AspNetCore.Mvc;

using Stripe;
using Stripe.Checkout;
using Task10Field.Data;
using Task10Field.Models.Payment;
namespace Task10Field.Controllers
{
    

    public class PaymentController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PaymentController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }



        [HttpPost]
        public IActionResult CreateCheckoutSession([FromBody] Payment request)
        {
            // Debugging: Print received data to ensure it's coming correctly  
            Console.WriteLine($"Received Payment: Name={request.Name}, Email={request.Email}, Amount={request.Amount}");

            if (string.IsNullOrEmpty(request.Name) || string.IsNullOrEmpty(request.Email) || request.Amount <= 0)
            {
                return BadRequest(new { message = "Invalid payment details" });
            }

           
            string customerName = request.Name;
            string customerEmail = request.Email;
            decimal paymentAmount = request.Amount;

            Console.WriteLine($"Using for Stripe: Name={customerName}, Email={customerEmail}, Amount={paymentAmount}");

            var domain = $"{Request.Scheme}://{Request.Host}";

            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string> { "card" },
                LineItems = new List<SessionLineItemOptions>
        {
            new SessionLineItemOptions
            {
                PriceData = new SessionLineItemPriceDataOptions
                {
                    Currency = "usd",
                    UnitAmount = (long)(paymentAmount * 100), 
                    ProductData = new SessionLineItemPriceDataProductDataOptions
                    {
                        Name = "Payment for " + customerName
                    }
                },
                Quantity = 1
            }
        },
                Mode = "payment",
                SuccessUrl = domain + "/Payment/Success?session_id={CHECKOUT_SESSION_ID}",
                CancelUrl = domain + "/Payment/Cancel"
            };

            var service = new SessionService();
            var session = service.Create(options);

            Console.WriteLine($"Stripe Session Created: ID={session.Id}");

            return Json(new { sessionId = session.Id });
        }



        public IActionResult Success(string session_id)
        {
            var service = new SessionService();
            var session = service.Get(session_id);

            var payment = new Payment
            {
                Name = session.CustomerDetails.Name,
                Email = session.CustomerDetails.Email,
                Amount = (decimal)(session.AmountTotal / 100),
                TransactionId = session.PaymentIntentId
            };

            _context.Payments.Add(payment);
            _context.SaveChanges();

            return View();
        }

        public IActionResult Cancel()
        {
            return View();
        }
    }

}
