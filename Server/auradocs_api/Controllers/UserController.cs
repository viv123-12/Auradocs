namespace auradocs_api.Controllers;
using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using auradocs_api.Contexts;
using auradocs_api.Models;
using auradocs_api.Services;
using System.Transactions;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private readonly AuradocsContext _auradocsContext;
    private readonly JWTService _jwtService;
    private readonly IConfiguration _config;
    private readonly IEmailService _emailService;

    public UserController(AuradocsContext auradocsContext, JWTService jwtService, IConfiguration config, IEmailService emailService)
    {
        _auradocsContext = auradocsContext;
        _jwtService = jwtService;
        _config = config;
        _emailService = emailService;
    }

    [HttpPost("register-user")]
    public async Task<IActionResult> RegisterUserAsync(RegisterUserDto registerUserDto)
    {
        using var transaction = await _auradocsContext.Database.BeginTransactionAsync();
        try
        {
            string password = RandomStringGenerationService.GenerateRandomString();
            System.Console.WriteLine(password);
            if(await this.IsUserExists(registerUserDto.userId, registerUserDto.phoneNumber))
            {
                return BadRequest("User Exists");
            }
            User newUser = new User
            {
                strGuid = Guid.NewGuid().ToString(),
                strUserId = registerUserDto.userId,
                strphoneNumber = registerUserDto.phoneNumber,
                strUserRole = UserRole.END_USER,
                strAccountType = registerUserDto.AccountType,
                uDomainType = registerUserDto.DomainType,
                uPracticeArea = registerUserDto.PracticeArea,
                boolIsUserActivated = true,
                dtAdded = DateTime.UtcNow,
                dtLastLogin = DateTime.UtcNow
            };
            _auradocsContext.Users.Add(newUser);
            await _auradocsContext.SaveChangesAsync();
            switch(registerUserDto.AccountType)
            {
                case AppConstants.IndividualUser:
                    {
                        if(!await sendPasswordTokenAsync(newUser.strGuid, newUser.strAccountType))
                        {
                            return BadRequest();
                        }
                    }
                break;
                case AppConstants.OrganizationalUser:
                    {
                        
                    }
                break;
            }
            await transaction.CommitAsync();
            return Ok("User registered successfully!!");
        }
        catch(Exception ex)
        {
            await transaction.RollbackAsync();
            Console.WriteLine(ex.Message);
            return StatusCode(500,ex.Message);
        }
    }

    [HttpPost("login")]
    public async Task<IActionResult> LoginAsync([FromBody] LoginDto logindto)
    {
        string accessToken;

        if(!await IsUserActive(logindto.userId))
        {
            return NotFound("User doesn't exst");
        }
        User? user = await _auradocsContext.Users.Where(u => u.strUserId == logindto.userId && u.strAccountType == logindto.userType && u.boolIsUserActivated).FirstOrDefaultAsync();
        bool verifiedUser = HashingService.VerifyPassword(logindto.Password, user.strPassword, user.strPasswordSalt); 
        if(!verifiedUser)
        {
            return BadRequest("Incorrect Password");
        }
        user.dtLastLogin = DateTime.UtcNow;
        accessToken = _jwtService.GenerateAccessToken(user.strUserId, user.strGuid, user.strUserRole);
        await _auradocsContext.SaveChangesAsync();
        Response.Cookies.Append(
            "auradocs_access_token",
            accessToken,
            new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddMinutes(_config.GetValue<int>("CookieExpirationTime"))
            }
        );
        return Ok(true);
    }

    [HttpGet("logout")]
    public IActionResult LogoutAsync()
    {
        Response.Cookies.Delete("auradocs_access_token");
        return Ok();
    }

    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPasswordAsync([FromBody] ForgotPasswordDto forgotPasswordDto)
    {
        ArgumentNullException.ThrowIfNull(forgotPasswordDto);
        using var transaction = await _auradocsContext.Database.BeginTransactionAsync();
        try
        {
            
            ResetPasswordToken? resetPasswordToken = await _auradocsContext.ResetPasswordTokens.Where(r => r.strToken == forgotPasswordDto.token && !r.boolIsVerified && r.dtExpiresAt > DateTime.UtcNow).FirstOrDefaultAsync();
            if(resetPasswordToken == null)
            {
                return BadRequest("Token has expired or being resued!!");
            }

            User? user = await _auradocsContext.Users.Where(o => o.strGuid == resetPasswordToken.strUserId && o.boolIsUserActivated).FirstOrDefaultAsync();
            if(user == null)
            {
                return NotFound("user not found!");
            }

            resetPasswordToken.boolIsVerified = true;
            await _auradocsContext.SaveChangesAsync();

            
            (string hashedPassword,string hashedPasswordSalt) = HashingService.HashPassword(forgotPasswordDto.password);
            user.strPassword = hashedPassword;
            user.strPasswordSalt = hashedPasswordSalt; 
            await _auradocsContext.SaveChangesAsync(); 
            await transaction.CommitAsync();
            return Ok("Password has updated");
        }catch(Exception ex)
        {
            await transaction.RollbackAsync();
            Console.WriteLine(ex.Message);
            throw;
        }
    }

    [HttpGet("verify-token")]
    public async Task<IActionResult> VerifyResetPasswordTokenAsync([FromQuery] string token)
    {
        ArgumentNullException.ThrowIfNull(token);
        ResetPasswordToken? resetPasswordToken = await _auradocsContext.ResetPasswordTokens.Where(e => e.strToken == token && e.dtExpiresAt > DateTime.UtcNow && !e.boolIsUsed).FirstOrDefaultAsync();
        if(resetPasswordToken == null)
        {
            return Redirect(AppConstants.verifyResultFailedUrl);
        }
        resetPasswordToken.boolIsUsed = true;
        await _auradocsContext.SaveChangesAsync();
        return Redirect(string.Format(AppConstants.verifyResultSuccessUrl,token));
    }

    [HttpGet("verify-account")]
    public async Task<IActionResult> VerifyAccountAsync(string userType,string identifier)
    {
        
        string? userId = await _auradocsContext.Users.Where(u => u.strUserId == identifier && u.strAccountType == userType && u.boolIsUserActivated).Select(u => u.strGuid).FirstOrDefaultAsync();
        if(userId == null)
        {
            return NotFound();
        }
        if(!await sendPasswordTokenAsync(userId, userType))
        {
            return BadRequest();
        }
        return Ok("verification Link sent");
    }

    private async Task<bool> sendPasswordTokenAsync(string userId, string userType)
    {
        string token = RandomStringGenerationService.GenerateRandomString();
        string verificationUrl = string.Format(AppConstants.resetPasswordVerificationUrl,token);
        try
        {
            ResetPasswordToken? resetPasswordTokens = await _auradocsContext.ResetPasswordTokens.Where(r => r.strUserId == userId && !r.boolIsUsed && r.dtExpiresAt >= DateTime.UtcNow).FirstOrDefaultAsync();
            if(resetPasswordTokens != null)
            {
                _auradocsContext.RemoveRange(resetPasswordTokens);
                await _auradocsContext.SaveChangesAsync();
            }
            _auradocsContext.ResetPasswordTokens.Add(new ResetPasswordToken
            {
                strGuid = Guid.NewGuid().ToString(),
                strUserId = userId,
                strToken = token,
                dtExpiresAt = DateTime.UtcNow.AddMinutes(_config.GetValue<int>("ForgotPassword:TokenExpirationDurationInMin")),
                dtCreatedAt = DateTime.UtcNow,
                boolIsUsed = false,
                boolIsVerified = false
            });
            await _auradocsContext.SaveChangesAsync();
            string body = string.Format(AppConstants.HtmlBody,verificationUrl);
            string? userEmail = await _auradocsContext.Users.Where(u => u.strGuid == userId).Select(e => e.strUserId).FirstOrDefaultAsync(); 
            switch(userType)
            {
                case AppConstants.IndividualUser:
                    {   
                        
                        if(!await _emailService.SendEmailAsync(userEmail, AppConstants.EmailSubject, body))
                        {
                            return false;
                        }
                    }
                break;
                case AppConstants.OrganizationalUser:
                    {
                        
                    }
                break;
            }
        }
        catch(Exception ex)
        {
            Console.WriteLine(ex.Message);
            return false;
        }
        return true;
    }

    private async Task<bool> IsUserExists(string email, string phoneNumber)
    {
        User? user = await _auradocsContext.Users.Where(u => u.strUserId == email || u.strphoneNumber == phoneNumber).FirstOrDefaultAsync();
        if(user == null)
        {
            return false;
        }
        return true;
    }

    private async Task<bool> IsUserActive(string userId)
    {
        User? user = await _auradocsContext.Users.Where(u => u.strUserId == userId && u.boolIsUserActivated).FirstOrDefaultAsync();
        if(user == null)
        {
            return false;
        }
        return true;
    }
}