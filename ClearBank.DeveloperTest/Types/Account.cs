namespace ClearBank.DeveloperTest.Types
{
    public sealed record Account
    {
        /*
         * Updated Account to be a record
         * Added immutabllity to the class
         */
        public string AccountNumber { get; init; }
        // Locked down modifications to Balance outside of the main Project
        // Init-only to force concious overwrites
        public decimal Balance { get; internal init; }
        public AccountStatus Status { get; internal set; }
        public AllowedPaymentSchemes AllowedPaymentSchemes { get; internal init; }
    }
}
