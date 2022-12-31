using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ChessStore.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;

namespace ChessStore.Controllers;

public class HomeController : Controller
{
    private MyContext _context;
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger, MyContext context)
    {
        _logger = logger;
        _context = context;
    }

    // Login & Registration

    [HttpGet("")]
    public IActionResult Index()
    {
        return View();
    }

    [HttpPost("users/create")]
    public IActionResult CreateUser(User newUser)
    {
        if (ModelState.IsValid)
        {
            // Hash Passwords
            PasswordHasher<User> Hasher = new PasswordHasher<User>();
            newUser.Password = Hasher.HashPassword(newUser, newUser.Password);
            _context.Add(newUser);
            _context.SaveChanges();
            HttpContext.Session.SetInt32("UserId", newUser.UserId);
            // HttpContext.Session.SetString("FirstName", newUser.FirstName);
            return RedirectToAction("Dashboard");
        }
        else
        {
            return View("Index");
        }
    }


    [HttpPost("users/login")]
    public IActionResult UserLogin(LoginUser LogUser)
    {
        if (ModelState.IsValid)
        {
            // look up user in DB
            User? userInDb = _context.Users.FirstOrDefault(u => u.Email == LogUser.LEmail);
            // Check to see if user exists
            if (userInDb == null)
            {
                ModelState.AddModelError("LEmail", "Invalid Email/Password");
                return View("Index");
            }
            // Verify password matches whats in the DB
            PasswordHasher<LoginUser> hasher = new PasswordHasher<LoginUser>();
            var result = hasher.VerifyHashedPassword(LogUser, userInDb.Password, LogUser.LPassword);
            if (result == 0)
            {
                // Failure
                ModelState.AddModelError("LEmail", "Invalid Email/Password");
                return View("Index");
            }
            else
            {
                // Set session and redirect to dashboard
                HttpContext.Session.SetInt32("UserId", userInDb.UserId);
                // HttpContext.Session.SetString("FirstName", userInDb.FirstName);
                return RedirectToAction("DashBoard");
            }
        }
        else
        {
            return View("Index");
        }
    }

    [HttpPost("logout")]
    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Index");
    }

    [SessionCheck]
    [HttpGet("users/dashboard")]
    public IActionResult Dashboard()
    {
        return View();
    }

    [SessionCheck]
    [HttpGet("boards")]
    public IActionResult Boards()
    {
        List<Board> AllBoards = _context.Boards.Include(a => a.Creator).ToList();
        return View(AllBoards);
    }

    [SessionCheck]
    [HttpGet("boards/new")]
    public IActionResult NewBoard()
    {
        return View();
    }

    [SessionCheck]
    [HttpPost("boards/create")]
    public IActionResult CreateBoard(Board newBoard)
    {
        if (ModelState.IsValid)
        {
            newBoard.UserId = (int)HttpContext.Session.GetInt32("UserId");
            _context.Add(newBoard);
            _context.SaveChanges();
            return RedirectToAction("Boards");
        }
        else
        {
            return View("NewBoard");
        }
    }

    [SessionCheck]
    [HttpGet("boards/{boardId}")]
    public IActionResult OneBoard(int boardId)
    {
        Board? One = _context.Boards.Include(s => s.Creator).FirstOrDefault(a => a.BoardId == boardId);
        return View(One);
    }

    [SessionCheck]
    [HttpGet("orderhistory")]
    public IActionResult OrderHistory()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}

public class SessionCheckAttribute : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        int? userId = context.HttpContext.Session.GetInt32("UserId");
        if (userId == null)
        {
            context.Result = new RedirectToActionResult("Index", "Home", null);
        }
    }
}