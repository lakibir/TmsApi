using System.ComponentModel.DataAnnotations;

namespace TmsApi.Configuration
{
    public class PaymentOptions
    {
        [Required(ErrorMessage = "The GatewayUrl field is strictly required.")]
        public required string GatewayUrl { get; init; }

        [Range(100, 100000, ErrorMessage = "Deposit limit must be between 100 and 100000 Birr.")]
        public decimal MaxDepositBirr { get; init; }
    }
}