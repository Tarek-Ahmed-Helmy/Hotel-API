using Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Presentation.DTOs;

namespace Presentation.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AccountController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;

    public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }

    [HttpPost("Register")]
    public async Task<IActionResult> Register(string lang, RegisterDto model)
    {
        var user = new ApplicationUser
        {
            UserName = model.Email,
            Email = model.Email,
            FirstName = model.FirstName,
            LastName = model.LastName,
            PhoneNumber = model.PhoneNumber,
            DepartmentId = model.DepartmentId,
            HotelId = model.HotelId
        };

        var result = await _userManager.CreateAsync(user, model.Password);
        if (!result.Succeeded)
        {
            return BadRequest(lang == "EN" ? result.Errors : "عطل في الخادم، برجاء المحاولة لاحقاً");
        }

        return Ok(lang == "EN" ? "Registration successful" : "تم انشاء المستخدم بنجاح");
    }

    [HttpPost("Login")]
    public async Task<IActionResult> Login(string lang, LoginDto model)
    {
        var user = await _userManager.FindByEmailAsync(model.Email);
        if (user == null)
            return Unauthorized(lang == "EN" ? "Invalid credentials" : "اسم المستخدم او كلمة السر غير صحيحة");

        var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);
        if (!result.Succeeded)
            return Unauthorized(lang == "EN" ? "Invalid credentials" : "اسم المستخدم او كلمة السر غير صحيحة");

        // TODO: Generate JWT here and return
        return Ok(lang == "EN" ? "Login successful" : "تم تسجيل الدخول بنجاح");
    }

    [HttpPost("Logout")]
    public async Task<IActionResult> Logout(string lang)
    {
        await _signInManager.SignOutAsync();
        return Ok(lang == "EN" ? "Logout successful" : "تم تسجيل الخروج بنجاح");
    }

    [HttpGet("GetAllUsers")]
    public async Task<IActionResult> GetAllUsers(string lang)
    {
        var users = await _userManager.Users.ToListAsync();
        if (users == null || !users.Any())
            return NotFound(lang == "EN" ? "No users found" : "لا يوجد مستخدمين");
        var userDtos = users.Select(u => new UserDto
        {
            Id = u.Id,
            FirstName = u.FirstName,
            LastName = u.LastName,
            Email = u.Email,
            PhoneNumber = u.PhoneNumber
        }).ToList();
        return Ok(userDtos);
    }
}
