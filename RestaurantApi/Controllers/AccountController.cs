using RestaurantApi.Core.Application.Dtos.Account;
using RestaurantApi.Core.Application.Enums;
using RestaurantApi.Core.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace RestaurantApi.Presentation.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AccountController : ControllerBase
{
    private readonly IAccountService _accountService;
    
    public AccountController(IAccountService accountService)
    {
        _accountService = accountService;
    }
    
    [HttpPost("authenticate")]
    public async Task<IActionResult> AuthenticateAsync(AuthRequest request)
    {
        return Ok(await _accountService.AuthenticateAsync(request));
    }
    
    [HttpPost("meseroRegister")]
    public async Task<IActionResult> ServerRegisterAsync(RegisterRequest request)
    {
        return Ok(await _accountService.Register(request, Roles.Mesero.ToString()));
    } 
    
   
    [HttpPost("adminRegister")]
    public async Task<IActionResult> AdminRegisterAsync(RegisterRequest request)
    {
        return Ok(await _accountService.Register(request, Roles.Mesero.ToString()));
    }
}