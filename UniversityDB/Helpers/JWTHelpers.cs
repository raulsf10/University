using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using UniversityDB.Models;

namespace UniversityDB.Helpers
{
    public static class JWTHelpers
    {
        public static IEnumerable<Claim> GetClaims(this UserTokens userAccount, Guid Id)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim("Id", userAccount.Id.ToString()),
                new Claim(ClaimTypes.Name, userAccount.UserName),
                new Claim(ClaimTypes.Email, userAccount.EmailId),
                new Claim(ClaimTypes.NameIdentifier, Id.ToString()),
                new Claim(ClaimTypes.Expiration, DateTime.UtcNow.AddMinutes(30).ToString("MMM ddd dd yyyy HH:mm:ss tt"))
            };

            if(userAccount.UserName == "Admin")
            {
                claims.Add(new Claim(ClaimTypes.Role, "Administrator"));
            }
            else if(userAccount.UserName == "User 1")
            {
                claims.Add(new Claim(ClaimTypes.Role, "User1"));
                claims.Add(new Claim("UserOnly", "User1"));
            }

            return claims;

        }

        public static IEnumerable<Claim> GetClaims(this UserTokens userAccount, out Guid Id)
        {
            Id = Guid.NewGuid();
            return GetClaims(userAccount, Id);
        }

        public static UserTokens GenTokenKey(UserTokens model, JWTSettings jwtSettings)
        {
            try
            {
                var userToken = new UserTokens();
                if(model == null)
                {
                    throw new ArgumentNullException(nameof(model));
                }

                // Obtain secret key
                var key = System.Text.Encoding.ASCII.GetBytes(jwtSettings.IssuerSingingKey);

                Guid id;

                // Expires in 1 day
                DateTime expireTime = DateTime.UtcNow.AddDays(1);

                // Validity of our Token
                userToken.Validity = expireTime.TimeOfDay;

                // Generate our JWT
                var jwToken = new JwtSecurityToken(
                    issuer: jwtSettings.ValidIssuer,
                    audience: jwtSettings.ValidAudience,
                    claims: GetClaims(model, out id),
                    notBefore: new DateTimeOffset(DateTime.Now).DateTime,
                    expires: new DateTimeOffset(expireTime).DateTime,
                    signingCredentials: new SigningCredentials(
                        new SymmetricSecurityKey(key),
                        SecurityAlgorithms.HmacSha256
                    )
                );

                userToken.Token = new JwtSecurityTokenHandler().WriteToken(jwToken);
                userToken.UserName = model.UserName;
                userToken.Id = model.Id;
                userToken.GuidId = id;

                return userToken;

            }
            catch (Exception ex)
            {
                throw new Exception("Error Generating the JWT", ex);
            }
        }



    }
}
