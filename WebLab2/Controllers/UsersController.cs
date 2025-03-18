using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebLab2.Models;
using WebLab2.Services;

namespace WebLab2.Controllers;


[ApiController]
[Route("api/users")]
public class UsersController : Controller
{
    private readonly IUnitOfWork _unitOfWork;

    public UsersController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    [Authorize(Roles = "Admin")]
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var users = await _unitOfWork.Users.GetAllAsync();
        var userDTOs = users.Select(u => new UserDto
        {
            Username = u.Username,
            Role = u.Role
        });
        return Ok(userDTOs);
    }
}
