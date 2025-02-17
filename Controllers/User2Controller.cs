namespace ASPNETMaker2023.Controllers;

// Partial class
public partial class HomeController : Controller
{
    // list
    [Route("User2List", Name = "User2List-User2-list")]
    [Route("Home/User2List", Name = "User2List-User2-list-2")]
    public async Task<IActionResult> User2List()
    {
        // Create page object
        user2List = new GLOBALS.User2List(this);
        user2List.Cache = _cache;

        // Run the page
        return await user2List.Run();
    }
}
