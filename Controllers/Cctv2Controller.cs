namespace ASPNETMaker2023.Controllers;

// Partial class
public partial class HomeController : Controller
{
    // list
    [Route("Cctv2List/{ID?}", Name = "Cctv2List-CCTV2-list")]
    [Route("Home/Cctv2List/{ID?}", Name = "Cctv2List-CCTV2-list-2")]
    public async Task<IActionResult> Cctv2List()
    {
        // Create page object
        cctv2List = new GLOBALS.Cctv2List(this);
        cctv2List.Cache = _cache;

        // Run the page
        return await cctv2List.Run();
    }

    // add
    [Route("Cctv2Add/{ID?}", Name = "Cctv2Add-CCTV2-add")]
    [Route("Home/Cctv2Add/{ID?}", Name = "Cctv2Add-CCTV2-add-2")]
    public async Task<IActionResult> Cctv2Add()
    {
        // Create page object
        cctv2Add = new GLOBALS.Cctv2Add(this);

        // Run the page
        return await cctv2Add.Run();
    }

    // view
    [Route("Cctv2View/{ID?}", Name = "Cctv2View-CCTV2-view")]
    [Route("Home/Cctv2View/{ID?}", Name = "Cctv2View-CCTV2-view-2")]
    public async Task<IActionResult> Cctv2View()
    {
        // Create page object
        cctv2View = new GLOBALS.Cctv2View(this);

        // Run the page
        return await cctv2View.Run();
    }

    // edit
    [Route("Cctv2Edit/{ID?}", Name = "Cctv2Edit-CCTV2-edit")]
    [Route("Home/Cctv2Edit/{ID?}", Name = "Cctv2Edit-CCTV2-edit-2")]
    public async Task<IActionResult> Cctv2Edit()
    {
        // Create page object
        cctv2Edit = new GLOBALS.Cctv2Edit(this);

        // Run the page
        return await cctv2Edit.Run();
    }

    // delete
    [Route("Cctv2Delete/{ID?}", Name = "Cctv2Delete-CCTV2-delete")]
    [Route("Home/Cctv2Delete/{ID?}", Name = "Cctv2Delete-CCTV2-delete-2")]
    public async Task<IActionResult> Cctv2Delete()
    {
        // Create page object
        cctv2Delete = new GLOBALS.Cctv2Delete(this);

        // Run the page
        return await cctv2Delete.Run();
    }
}
