namespace UniversityDB.Models
{
    public class JWTSettings
    {
        public bool ValidateIssuerSingingKey { get; set; }
        public string IssuerSingingKey { get; set; } = string.Empty;
        
        public bool ValidateIssuer { get; set; } = true;
        public string? ValidIssuer { get; set; }
        
        public bool ValidateAudience { get; set; } = true;
        public string? ValidAudience { get; set; }
        
        public bool RequireExpirationTime { get; set; }
        public bool ValidateLifetime { get; set; } = true;


    }
}
