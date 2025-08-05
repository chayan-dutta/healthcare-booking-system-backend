namespace HealthCare.Common.Persistence.Configuration
{
    public class DbConnectionOptions
    {
        /// <summary>
        /// 
        /// </summary>
        public string Host { get; set; } = "localhost";
        
        /// <summary>
        /// 
        /// </summary>
        public string Port { get; set; } = "5432";
        
        /// <summary>
        /// 
        /// </summary>
        public string Database { get; set; } = "HealthCareBookingSystemDB";
        
        /// <summary>
        /// 
        /// </summary>
        public string Username { get; set; } = "postgres";
        
        /// <summary>
        /// 
        /// </summary>
        public string Password { get; set; } = "postgres";
    }
}
