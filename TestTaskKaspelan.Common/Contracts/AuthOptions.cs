namespace TestTaskKaspelan.Common.Contracts
{
    /// <summary>
    /// Authenticate option configuration.
    /// </summary>
    public class AuthOptions
    {
        /// <summary>
        /// Gets or sets the key.
        /// </summary>
        /// <value>
        /// The key.
        /// </value>
        public string Key { get; set; }

        /// <summary>
        /// Gets or sets the issuer.
        /// </summary>
        /// <value>
        /// The issuer.
        /// </value>
        public string Issuer { get; set; }

        /// <summary>
        /// Gets or sets the expire.
        /// </summary>
        /// <value>
        /// The expire.
        /// </value>
        public int Expire { get; set; }

        /// <summary>
        /// The configuration prefix.
        /// </summary>
        public const string PREFIX = "jwt";
    }
}
