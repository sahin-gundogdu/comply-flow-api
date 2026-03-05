using System;
using System.Security.Claims;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;

var secretKey = "ComplyFlow-Super-Gizli-Anahtar-Kelimesi-Gelecek-Buraya-32-Karakter";
var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

var claims = new[]
{
    new Claim(ClaimTypes.NameIdentifier, "1"), // User ID 1
    new Claim(ClaimTypes.Name, "Ahmet Yılmaz"), // User Name
    new Claim(ClaimTypes.Role, "Avukat") // User Role
};

var token = new JwtSecurityToken(
    issuer: "http://localhost:5000",
    audience: "http://localhost:3000",
    claims: claims,
    expires: DateTime.Now.AddHours(2),
    signingCredentials: credentials);

var tokenStr = new JwtSecurityTokenHandler().WriteToken(token);
Console.WriteLine(tokenStr);
