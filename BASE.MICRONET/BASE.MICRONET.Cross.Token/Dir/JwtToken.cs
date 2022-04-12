using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace BASE.MICRONET.Cross.Token.Dir
{
    public static class JwtToken
    {
        public static string Create(JwtOptions configuration)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.Key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(configuration.Issuer,
                configuration.Audience,
                expires: DateTime.Now.AddMinutes(Convert.ToDouble(configuration.Expiration)),
                signingCredentials: creds);

            string _token = new JwtSecurityTokenHandler().WriteToken(token);

            return _token;
        }
    }
}
