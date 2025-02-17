namespace ASPNETMaker2023.Models;

// Partial class
public partial class defa {
    /// <summary>
    /// cctv2Edit
    /// </summary>
    public static Cctv2Edit cctv2Edit
    {
        get => HttpData.Get<Cctv2Edit>("cctv2Edit")!;
        set => HttpData["cctv2Edit"] = value;
    }

    /// <summary>
    /// Page class for CCTV
    /// </summary>
    public class Cctv2Edit : Cctv2EditBase
    {
        // Constructor
        public Cctv2Edit(Controller controller) : base(controller)
        {
        }

        // Constructor
        public Cctv2Edit() : base()
        {
        }
    }

    /// <summary>
    /// Page base class
    /// </summary>
    public class Cctv2EditBase : Cctv2
    {
        // Page ID
        public string PageID = "edit";

        // Project ID
        public string ProjectID = "{A82BC683-C15E-40D7-94E1-955AFED823D6}";

        // Table name
        public string TableName { get; set; } = "CCTV";

        // Page object name
        public string PageObjName = "cctv2Edit";

        // Title
        public string? Title = null; // Title for <title> tag

        // Page headings
        public string Heading = "";

        public string Subheading = "";

        public string PageHeader = "";

        public string PageFooter = "";

        // Token
        public string? Token = null; // DN

        public bool CheckToken = Config.CheckToken;

        // Action result // DN
        public IActionResult? ActionResult;

        // Cache // DN
        public IMemoryCache? Cache;

        // Page layout
        public bool UseLayout = true;

        // Page terminated // DN
        private bool _terminated = false;

        // Is terminated
        public bool IsTerminated => _terminated;

        // Is lookup
        public bool IsLookup => IsApi() && RouteValues.TryGetValue("controller", out object? name) && SameText(name, Config.ApiLookupAction);

        // Is AutoFill
        public bool IsAutoFill => IsLookup && SameText(Post("ajax"), "autofill");

        // Is AutoSuggest
        public bool IsAutoSuggest => IsLookup && SameText(Post("ajax"), "autosuggest");

        // Is modal lookup
        public bool IsModalLookup => IsLookup && SameText(Post("ajax"), "modal");

        // Page URL
        private string _pageUrl = "";

        // Constructor
        public Cctv2EditBase()
        {
            // Initialize
            CurrentPage = this;

            // Table CSS class
            TableClass = "table table-striped table-bordered table-hover table-sm ew-desktop-table ew-edit-table";

            // Language object
            Language = ResolveLanguage();

            // Table object (cctv2)
            if (cctv2 == null || cctv2 is Cctv2)
                cctv2 = this;

            // Start time
            StartTime = Environment.TickCount;

            // Debug message
            LoadDebugMessage();

            // Open connection
            Conn = Connection; // DN

            // User table object (User2)
            UserTable = Resolve("usertable")!;
            UserTableConn = GetConnection(UserTable.DbId);
        }

        // Page action result
        public IActionResult PageResult()
        {
            if (ActionResult != null)
                return ActionResult;
            SetupMenus();
            return Controller.View();
        }

        // Page heading
        public string PageHeading
        {
            get {
                if (!Empty(Heading))
                    return Heading;
                else if (!Empty(Caption))
                    return Caption;
                else
                    return "";
            }
        }

        // Page subheading
        public string PageSubheading
        {
            get {
                if (!Empty(Subheading))
                    return Subheading;
                if (!Empty(TableName))
                    return Language.Phrase(PageID);
                return "";
            }
        }

        // Page name
        public string PageName => "Cctv2Edit";

        // Page URL
        public string PageUrl
        {
            get {
                if (_pageUrl == "") {
                    _pageUrl = PageName + "?";
                }
                return _pageUrl;
            }
        }

        // Show Page Header
        public IHtmlContent ShowPageHeader()
        {
            string header = PageHeader;
            PageDataRendering(ref header);
            if (!Empty(header)) // Header exists, display
                return new HtmlString("<p id=\"ew-page-header\">" + header + "</p>");
            return HtmlString.Empty;
        }

        // Show Page Footer
        public IHtmlContent ShowPageFooter()
        {
            string footer = PageFooter;
            PageDataRendered(ref footer);
            if (!Empty(footer)) // Footer exists, display
                return new HtmlString("<p id=\"ew-page-footer\">" + footer + "</p>");
            return HtmlString.Empty;
        }

        // Valid post
        protected async Task<bool> ValidPost() => !CheckToken || !IsPost() || IsApi() || Antiforgery != null && HttpContext != null && await Antiforgery.IsRequestValidAsync(HttpContext);

        // Create token
        public void CreateToken()
        {
            Token ??= HttpContext != null ? Antiforgery?.GetAndStoreTokens(HttpContext).RequestToken : null;
            CurrentToken = Token ?? ""; // Save to global variable
        }

        // Set field visibility
        public void SetVisibility()
        {
            ID.SetVisibility();
            Code.SetVisibility();
            _Name.SetVisibility();
            Loc.SetVisibility();
            Status.SetVisibility();
            Incident.SetVisibility();
            CreatedDateTime.SetVisibility();
            CreatedByUserID.SetVisibility();
            UpdatedDateTime.SetVisibility();
            UpdatedByUserID.SetVisibility();
            Perbaikan.SetVisibility();
        }

        // Constructor
        public Cctv2EditBase(Controller? controller = null): this() { // DN
            if (controller != null)
                Controller = controller;
        }

        /// <summary>
        /// Terminate page
        /// </summary>
        /// <param name="url">URL to rediect to</param>
        /// <returns>Page result</returns>
        public override IActionResult Terminate(string url = "") { // DN
            if (_terminated) // DN
                return new EmptyResult();

            // Page Unload event
            PageUnload();

            // Global Page Unloaded event
            PageUnloaded();
            if (!IsApi())
                PageRedirecting(ref url);

            // Gargage collection
            Collect(); // DN

            // Terminate
            _terminated = true; // DN

            // Return for API
            if (IsApi()) {
                var result = new Dictionary<string, string> { { "version", Config.ProductVersion } };
                if (!Empty(url)) // Add url
                    result.Add("url", GetUrl(url));
                foreach (var (key, value) in GetMessages()) // Add messages
                    result.Add(key, value);
                return Controller.Json(result);
            } else if (ActionResult != null) { // Check action result
                return ActionResult;
            }

            // Go to URL if specified
            if (!Empty(url)) {
                if (!Config.Debug)
                    ResponseClear();
                if (Response != null && !Response.HasStarted) {
                    // Handle modal response (Assume return to modal for simplicity)
                    if (IsModal) { // Show as modal
                        var result = new Dictionary<string, string> { {"url", GetUrl(url)}, {"modal", "1"} };
                        string pageName = GetPageName(url);
                        if (pageName != ListUrl) { // Not List page
                            result.Add("caption", GetModalCaption(pageName));
                            result.Add("view", pageName == "Cctv2View" ? "1" : "0"); // If View page, no primary button
                        } else { // List page
                            // result.Add("list", PageID == "search" ? "1" : "0"); // Refresh List page if current page is Search page
                            result.Add("error", FailureMessage); // List page should not be shown as modal => error
                            ClearFailureMessage();
                        }
                        return Controller.Json(result);
                    } else {
                        SaveDebugMessage();
                        return Controller.LocalRedirect(AppPath(url));
                    }
                }
            }
            return new EmptyResult();
        }

        // Get all records from datareader
        [return: NotNullIfNotNull("dr")]
        protected async Task<List<Dictionary<string, object>>> GetRecordsFromRecordset(DbDataReader? dr)
        {
            var rows = new List<Dictionary<string, object>>();
            while (dr != null && await dr.ReadAsync()) {
                await LoadRowValues(dr); // Set up DbValue/CurrentValue
                if (GetRecordFromDictionary(GetDictionary(dr)) is Dictionary<string, object> row)
                    rows.Add(row);
            }
            return rows;
        }

        // Get all records from the list of records
        #pragma warning disable 1998

        protected async Task<List<Dictionary<string, object>>> GetRecordsFromRecordset(List<Dictionary<string, object>>? list)
        {
            var rows = new List<Dictionary<string, object>>();
            if (list != null) {
                foreach (var row in list) {
                    if (GetRecordFromDictionary(row) is Dictionary<string, object> d)
                       rows.Add(row);
                }
            }
            return rows;
        }
        #pragma warning restore 1998

        // Get the first record from datareader
        [return: NotNullIfNotNull("dr")]
        protected async Task<Dictionary<string, object>?> GetRecordFromRecordset(DbDataReader? dr)
        {
            if (dr != null) {
                await LoadRowValues(dr); // Set up DbValue/CurrentValue
                return GetRecordFromDictionary(GetDictionary(dr));
            }
            return null;
        }

        // Get the first record from the list of records
        protected Dictionary<string, object>? GetRecordFromRecordset(List<Dictionary<string, object>>? list) =>
            list != null && list.Count > 0 ? GetRecordFromDictionary(list[0]) : null;

        // Get record from Dictionary
        protected Dictionary<string, object>? GetRecordFromDictionary(Dictionary<string, object>? dict) {
            if (dict == null)
                return null;
            var row = new Dictionary<string, object>();
            foreach (var (key, value) in dict) {
                if (Fields.TryGetValue(key, out DbField? fld)) {
                    if (fld.Visible || fld.IsPrimaryKey) { // Primary key or Visible
                        if (fld.HtmlTag == "FILE") { // Upload field
                            if (Empty(value)) {
                                // row[key] = null;
                            } else {
                                if (fld.DataType == DataType.Blob) {
                                    string url = FullUrl(GetPageName(Config.ApiUrl) + "/" + Config.ApiFileAction + "/" + fld.TableVar + "/" + fld.Param + "/" + GetRecordKeyValue(dict)); // Query string format
                                    row[key] = new Dictionary<string, object> { { "type", ContentType((byte[])value) }, { "url", url }, { "name", fld.Param + ContentExtension((byte[])value) } };
                                } else if (!fld.UploadMultiple || !ConvertToString(value).Contains(Config.MultipleUploadSeparator)) { // Single file
                                    string url = FullUrl(GetPageName(Config.ApiUrl) + "/" + Config.ApiFileAction + "/" + fld.TableVar + "/" + Encrypt(fld.PhysicalUploadPath + ConvertToString(value))); // Query string format
                                    row[key] = new Dictionary<string, object> { { "type", ContentType(ConvertToString(value)) }, { "url", url }, { "name", ConvertToString(value) } };
                                } else { // Multiple files
                                    var files = ConvertToString(value).Split(Config.MultipleUploadSeparator);
                                    row[key] = files.Where(file => !Empty(file)).Select(file => new Dictionary<string, object> { { "type", ContentType(file) }, { "url", FullUrl(GetPageName(Config.ApiUrl) + "/" + Config.ApiFileAction + "/" + fld.TableVar + "/" + Encrypt(fld.PhysicalUploadPath + file)) }, { "name", file } });
                                }
                            }
                        } else {
                            string val = ConvertToString(value);
                            if (fld.DataType == DataType.Date && value is DateTime dt)
                                val = dt.ToString("s");
                            row[key] = ConvertToString(val);
                        }
                    }
                }
            }
            return row;
        }

        // Get record key value from array
        protected string GetRecordKeyValue(Dictionary<string, object> dict) {
            string key = "";
            key += UrlEncode(ConvertToString(dict.ContainsKey("ID") ? dict["ID"] : ID.CurrentValue));
            return key;
        }

        // Hide fields for Add/Edit
        protected void HideFieldsForAddEdit() {
            if (IsAdd || IsCopy || IsGridAdd)
                ID.Visible = false;
        }

        #pragma warning disable 219
        /// <summary>
        /// Lookup data from table
        /// </summary>
        public async Task<Dictionary<string, object>> Lookup(Dictionary<string, string>? dict = null)
        {
            Language = ResolveLanguage();
            Security = ResolveSecurity();
            string? v;

            // Get lookup object
            string fieldName = IsDictionary(dict) && dict.TryGetValue("field", out v) && v != null ? v : Post("field");
            var lookupField = FieldByName(fieldName);
            var lookup = lookupField?.Lookup;
            if (lookup == null) // DN
                return new Dictionary<string, object>();
            string lookupType = IsDictionary(dict) && dict.TryGetValue("ajax", out v) && v != null ? v : (Post("ajax") ?? "unknown");
            int pageSize = -1;
            int offset = -1;
            string searchValue = "";
            if (SameText(lookupType, "modal") || SameText(lookupType, "filter")) {
                searchValue = IsDictionary(dict) && (dict.TryGetValue("q", out v) && v != null || dict.TryGetValue("sv", out v) && v != null)
                    ? v
                    : (Param("q") ?? Post("sv"));
                pageSize = IsDictionary(dict) && (dict.TryGetValue("n", out v) || dict.TryGetValue("recperpage", out v))
                    ? ConvertToInt(v)
                    : (IsNumeric(Param("n")) ? Param<int>("n") : (Post("recperpage", out StringValues rpp) ? ConvertToInt(rpp.ToString()) : 10));
            } else if (SameText(lookupType, "autosuggest")) {
                searchValue = IsDictionary(dict) && dict.TryGetValue("q", out v) && v != null ? v : Param("q");
                pageSize = IsDictionary(dict) && dict.TryGetValue("n", out v) ? ConvertToInt(v) : (IsNumeric(Param("n")) ? Param<int>("n") : -1);
                if (pageSize <= 0)
                    pageSize = Config.AutoSuggestMaxEntries;
            }
            int start = IsDictionary(dict) && dict.TryGetValue("start", out v) ? ConvertToInt(v) : (IsNumeric(Param("start")) ? Param<int>("start") : -1);
            int page = IsDictionary(dict) && dict.TryGetValue("page", out v) ? ConvertToInt(v) : (IsNumeric(Param("page")) ? Param<int>("page") : -1);
            offset = start >= 0 ? start : (page > 0 && pageSize > 0 ? (page - 1) * pageSize : 0);
            string userSelect = Decrypt(IsDictionary(dict) && dict.TryGetValue("s", out v) && v != null ? v : Post("s"));
            string userFilter = Decrypt(IsDictionary(dict) && dict.TryGetValue("f", out v) && v != null ? v : Post("f"));
            string userOrderBy = Decrypt(IsDictionary(dict) && dict.TryGetValue("o", out v) && v != null ? v : Post("o"));

            // Selected records from modal, skip parent/filter fields and show all records
            lookup.LookupType = lookupType; // Lookup type
            lookup.FilterValues.Clear(); // Clear filter values first
            StringValues keys = IsDictionary(dict) && dict.TryGetValue("keys", out v) && !Empty(v)
                ? (StringValues)v
                : (Post("keys[]", out StringValues k) ? (StringValues)k : StringValues.Empty);
            if (!Empty(keys)) { // Selected records from modal
                lookup.FilterFields = new (); // Skip parent fields if any
                pageSize = -1; // Show all records
                lookup.FilterValues.Add(String.Join(",", keys.ToArray()));
            } else { // Lookup values
                string lookupValue = IsDictionary(dict) && (dict.TryGetValue("v0", out v) && v != null || dict.TryGetValue("lookupValue", out v) && v != null)
                    ? v
                    : (Post<string>("v0") ?? Post("lookupValue"));
                lookup.FilterValues.Add(lookupValue);
            }
            int cnt = IsDictionary(lookup.FilterFields) ? lookup.FilterFields.Count : 0;
            for (int i = 1; i <= cnt; i++) {
                var val = UrlDecode(IsDictionary(dict) && dict.TryGetValue("v" + i, out v) ? v : Post("v" + i));
                if (val != null) // DN
                    lookup.FilterValues.Add(val);
            }
            lookup.SearchValue = searchValue;
            lookup.PageSize = pageSize;
            lookup.Offset = offset;
            if (userSelect != "")
                lookup.UserSelect = userSelect;
            if (userFilter != "")
                lookup.UserFilter = userFilter;
            if (userOrderBy != "")
                lookup.UserOrderBy = userOrderBy;
            return await lookup.ToJson(this);
        }
        #pragma warning restore 219

        public int DisplayRecords = 1; // Number of display records

        public int StartRecord;

        public int StopRecord;

        public int TotalRecords = -1;

        public int RecordRange = 10;

        public int RecordCount;

        public Dictionary<string, string> RecordKeys = new ();

        public string FormClassName = "ew-form ew-edit-form overlay-wrapper";

        public bool IsModal = false;

        public bool IsMobileOrModal = false;

        public string DbMasterFilter = "";

        public string DbDetailFilter = "";

        public DbDataReader? Recordset; // DN

        #pragma warning disable 219
        /// <summary>
        /// Page run
        /// </summary>
        /// <returns>Page result</returns>
        public override async Task<IActionResult> Run()
        {
            // Is modal
            IsModal = Param<bool>("modal");
            UseLayout = UseLayout && !IsModal;

            // Use layout
            if (!Empty(Param("layout")) && !Param<bool>("layout"))
                UseLayout = false;

            // User profile
            Profile = ResolveProfile();

            // Security
            Security = ResolveSecurity();
            if (TableVar != "")
                Security.LoadTablePermissions(TableVar);

            // Create form object
            CurrentForm ??= new ();
            await CurrentForm.Init();
            CurrentAction = Param("action"); // Set up current action
            SetVisibility();

            // Do not use lookup cache
            if (!Config.LookupCachePageIds.Contains(PageID))
                SetUseLookupCache(false);

            // Global Page Loading event
            PageLoading();

            // Page Load event
            PageLoad();

            // Check token
            if (!await ValidPost())
                End(Language.Phrase("InvalidPostRequest"));

            // Check action result
            if (ActionResult != null) // Action result set by server event // DN
                return ActionResult;

            // Create token
            CreateToken();

            // Hide fields for add/edit
            if (!UseAjaxActions)
                HideFieldsForAddEdit();
            // Use inline delete
            if (UseAjaxActions)
                InlineDelete = true;

            // Set up lookup cache
            await SetupLookupOptions(Status);
            await SetupLookupOptions(Perbaikan);

            // Check modal
            if (IsModal)
                SkipHeaderFooter = true;
            IsMobileOrModal = IsMobile() || IsModal;
            bool loaded = false;
            bool postBack = false;
            StringValues sv;
            object? rv;

            // Set up current action and primary key
            if (IsApi()) {
                loaded = true;

                // Load key from form
                string[] keyValues = {};
                if (RouteValues.TryGetValue("key", out object? k))
                    keyValues = ConvertToString(k).Split('/');
                if (RouteValues.TryGetValue("ID", out rv)) { // DN
                    ID.FormValue = UrlDecode(rv); // DN
                    ID.OldValue = ID.FormValue;
                } else if (CurrentForm.HasValue("x_ID")) {
                    ID.FormValue = CurrentForm.GetValue("x_ID");
                    ID.OldValue = ID.FormValue;
                } else if (!Empty(keyValues)) {
                    ID.OldValue = ConvertToString(keyValues[0]);
                } else {
                    loaded = false; // Unable to load key
                }

                // Load record
                if (loaded)
                    loaded = await LoadRow();
                if (!loaded) {
                    FailureMessage = Language.Phrase("NoRecord"); // Set no record message
                    return Terminate();
                }
                CurrentAction = "update"; // Update record directly
                OldKey = GetKey(true); // Get from CurrentValue
                postBack = true;
            } else {
                if (!Empty(Post("action"))) {
                    CurrentAction = Post("action"); // Get action code
                    if (!IsShow) // Not reload record, handle as postback
                        postBack = true;

                    // Get key from Form
                    if (Post(OldKeyName, out sv))
                        SetKey(sv.ToString(), IsShow);
                } else {
                    CurrentAction = "show"; // Default action is display

                    // Load key from QueryString
                    bool loadByQuery = false;
                    if (RouteValues.TryGetValue("ID", out rv)) { // DN
                        ID.QueryValue = UrlDecode(rv); // DN
                        loadByQuery = true;
                    } else if (Get("ID", out sv)) {
                        ID.QueryValue = sv.ToString();
                        loadByQuery = true;
                    } else {
                        ID.CurrentValue = DbNullValue;
                    }
                }

                // Load recordset
                if (IsShow) {
                    // Load current record
                    loaded = await LoadRow();
                OldKey = loaded ? GetKey(true) : ""; // Get from CurrentValue
            }
        }

        // Process form if post back
        if (postBack) {
            await LoadFormValues(); // Get form values
            if (IsApi() && RouteValues.TryGetValue("key", out object? k)) {
                var keyValues = ConvertToString(k).Split('/');
                ID.FormValue = ConvertToString(keyValues[0]);
            }
        }

        // Validate form if post back
        if (postBack) {
            if (!await ValidateForm()) {
                EventCancelled = true; // Event cancelled
                RestoreFormValues();
                if (IsApi())
                    return Terminate();
                else
                    CurrentAction = ""; // Form error, reset action
            }
        }

        // Perform current action
        switch (CurrentAction) {
                case "show": // Get a record to display
                        if (!loaded) { // Load record based on key
                            if (Empty(FailureMessage))
                                FailureMessage = Language.Phrase("NoRecord"); // No record found
                            return Terminate("Cctv2List"); // No matching record, return to list
                        }
                    break;
                case "update": // Update // DN
                    CloseRecordset(); // DN
                    string returnUrl = ReturnUrl;
                    if (GetPageName(returnUrl) == "Cctv2List")
                        returnUrl = AddMasterUrl(ListUrl); // List page, return to List page with correct master key if necessary
                    SendEmail = true; // Send email on update success
                    var res = await EditRow();
                    if (res) { // Update record based on key
                        if (Empty(SuccessMessage))
                            SuccessMessage = Language.Phrase("UpdateSuccess"); // Update success

                        // Handle UseAjaxActions with return page
                        if (IsModal && UseAjaxActions) {
                            IsModal = false;
                            if (GetPageName(returnUrl) != "Cctv2List") {
                                TempData["Return-Url"] = returnUrl; // Save return URL
                                returnUrl = "Cctv2List"; // Return list page content
                            }
                        }
                        if (IsJsonResponse()) {
                            ClearMessages(); // Clear messages for Json response
                            return res;
                        } else {
                            return Terminate(returnUrl); // Return to caller
                        }
                    } else if (IsApi()) { // API request, return
                        return Terminate();
                    } else if (IsModal && UseAjaxActions) { // Return JSON error message
                        return Controller.Json(new { success = false, error = GetFailureMessage() });
                    } else if (FailureMessage == Language.Phrase("NoRecord")) {
                        return Terminate(returnUrl); // Return to caller
                    } else {
                        EventCancelled = true; // Event cancelled
                        RestoreFormValues(); // Restore form values if update failed
                    }
                    break;
            }

            // Set up Breadcrumb
            SetupBreadcrumb();

            // Render the record
            RowType = RowType.Edit; // Render as Edit
            ResetAttributes();
            await RenderRow();

            // Set LoginStatus, Page Rendering and Page Render
            if (!IsApi() && !IsTerminated) {
                SetupLoginStatus(); // Setup login status

                // Pass login status to client side
                SetClientVar("login", LoginStatus);

                // Global Page Rendering event
                PageRendering();

                // Page Render event
                cctv2Edit?.PageRender();
            }
            return PageResult();
        }
        #pragma warning restore 219

        // Confirm page
        public bool ConfirmPage = false; // DN

        #pragma warning disable 1998
        // Get upload files
        public async Task GetUploadFiles()
        {
            // Get upload data
        }
        #pragma warning restore 1998

        #pragma warning disable 1998
        // Load form values
        protected async Task LoadFormValues() {
            if (CurrentForm == null)
                return;
            bool validate = !Config.ServerValidate;
            string val;

            // Check field name 'ID' before field var 'x_ID'
            val = CurrentForm.HasValue("ID") ? CurrentForm.GetValue("ID") : CurrentForm.GetValue("x_ID");
            if (!ID.IsDetailKey)
                ID.SetFormValue(val);

            // Check field name 'Code' before field var 'x_Code'
            val = CurrentForm.HasValue("Code") ? CurrentForm.GetValue("Code") : CurrentForm.GetValue("x_Code");
            if (!Code.IsDetailKey) {
                if (IsApi() && !CurrentForm.HasValue("Code") && !CurrentForm.HasValue("x_Code")) // DN
                    Code.Visible = false; // Disable update for API request
                else
                    Code.SetFormValue(val);
            }

            // Check field name 'Name' before field var 'x__Name'
            val = CurrentForm.HasValue("Name") ? CurrentForm.GetValue("Name") : CurrentForm.GetValue("x__Name");
            if (!_Name.IsDetailKey) {
                if (IsApi() && !CurrentForm.HasValue("Name") && !CurrentForm.HasValue("x__Name")) // DN
                    _Name.Visible = false; // Disable update for API request
                else
                    _Name.SetFormValue(val);
            }

            // Check field name 'Loc' before field var 'x_Loc'
            val = CurrentForm.HasValue("Loc") ? CurrentForm.GetValue("Loc") : CurrentForm.GetValue("x_Loc");
            if (!Loc.IsDetailKey) {
                if (IsApi() && !CurrentForm.HasValue("Loc") && !CurrentForm.HasValue("x_Loc")) // DN
                    Loc.Visible = false; // Disable update for API request
                else
                    Loc.SetFormValue(val);
            }

            // Check field name 'Status' before field var 'x_Status'
            val = CurrentForm.HasValue("Status") ? CurrentForm.GetValue("Status") : CurrentForm.GetValue("x_Status");
            if (!Status.IsDetailKey) {
                if (IsApi() && !CurrentForm.HasValue("Status") && !CurrentForm.HasValue("x_Status")) // DN
                    Status.Visible = false; // Disable update for API request
                else
                    Status.SetFormValue(val);
            }

            // Check field name 'Incident' before field var 'x_Incident'
            val = CurrentForm.HasValue("Incident") ? CurrentForm.GetValue("Incident") : CurrentForm.GetValue("x_Incident");
            if (!Incident.IsDetailKey) {
                if (IsApi() && !CurrentForm.HasValue("Incident") && !CurrentForm.HasValue("x_Incident")) // DN
                    Incident.Visible = false; // Disable update for API request
                else
                    Incident.SetFormValue(val, true, validate);
            }

            // Check field name 'CreatedDateTime' before field var 'x_CreatedDateTime'
            val = CurrentForm.HasValue("CreatedDateTime") ? CurrentForm.GetValue("CreatedDateTime") : CurrentForm.GetValue("x_CreatedDateTime");
            if (!CreatedDateTime.IsDetailKey) {
                if (IsApi() && !CurrentForm.HasValue("CreatedDateTime") && !CurrentForm.HasValue("x_CreatedDateTime")) // DN
                    CreatedDateTime.Visible = false; // Disable update for API request
                else
                    CreatedDateTime.SetFormValue(val, true, validate);
                CreatedDateTime.CurrentValue = UnformatDateTime(CreatedDateTime.CurrentValue, CreatedDateTime.FormatPattern);
            }

            // Check field name 'CreatedByUserID' before field var 'x_CreatedByUserID'
            val = CurrentForm.HasValue("CreatedByUserID") ? CurrentForm.GetValue("CreatedByUserID") : CurrentForm.GetValue("x_CreatedByUserID");
            if (!CreatedByUserID.IsDetailKey) {
                if (IsApi() && !CurrentForm.HasValue("CreatedByUserID") && !CurrentForm.HasValue("x_CreatedByUserID")) // DN
                    CreatedByUserID.Visible = false; // Disable update for API request
                else
                    CreatedByUserID.SetFormValue(val, true, validate);
            }

            // Check field name 'UpdatedDateTime' before field var 'x_UpdatedDateTime'
            val = CurrentForm.HasValue("UpdatedDateTime") ? CurrentForm.GetValue("UpdatedDateTime") : CurrentForm.GetValue("x_UpdatedDateTime");
            if (!UpdatedDateTime.IsDetailKey) {
                if (IsApi() && !CurrentForm.HasValue("UpdatedDateTime") && !CurrentForm.HasValue("x_UpdatedDateTime")) // DN
                    UpdatedDateTime.Visible = false; // Disable update for API request
                else
                    UpdatedDateTime.SetFormValue(val, true, validate);
                UpdatedDateTime.CurrentValue = UnformatDateTime(UpdatedDateTime.CurrentValue, UpdatedDateTime.FormatPattern);
            }

            // Check field name 'UpdatedByUserID' before field var 'x_UpdatedByUserID'
            val = CurrentForm.HasValue("UpdatedByUserID") ? CurrentForm.GetValue("UpdatedByUserID") : CurrentForm.GetValue("x_UpdatedByUserID");
            if (!UpdatedByUserID.IsDetailKey) {
                if (IsApi() && !CurrentForm.HasValue("UpdatedByUserID") && !CurrentForm.HasValue("x_UpdatedByUserID")) // DN
                    UpdatedByUserID.Visible = false; // Disable update for API request
                else
                    UpdatedByUserID.SetFormValue(val, true, validate);
            }

            // Check field name 'Perbaikan' before field var 'x_Perbaikan'
            val = CurrentForm.HasValue("Perbaikan") ? CurrentForm.GetValue("Perbaikan") : CurrentForm.GetValue("x_Perbaikan");
            if (!Perbaikan.IsDetailKey) {
                if (IsApi() && !CurrentForm.HasValue("Perbaikan") && !CurrentForm.HasValue("x_Perbaikan")) // DN
                    Perbaikan.Visible = false; // Disable update for API request
                else
                    Perbaikan.SetFormValue(val);
            }
        }
        #pragma warning restore 1998

        // Restore form values
        public void RestoreFormValues()
        {
            ID.CurrentValue = ID.FormValue;
            Code.CurrentValue = Code.FormValue;
            _Name.CurrentValue = _Name.FormValue;
            Loc.CurrentValue = Loc.FormValue;
            Status.CurrentValue = Status.FormValue;
            Incident.CurrentValue = Incident.FormValue;
            CreatedDateTime.CurrentValue = CreatedDateTime.FormValue;
            CreatedDateTime.CurrentValue = UnformatDateTime(CreatedDateTime.CurrentValue, CreatedDateTime.FormatPattern);
            CreatedByUserID.CurrentValue = CreatedByUserID.FormValue;
            UpdatedDateTime.CurrentValue = UpdatedDateTime.FormValue;
            UpdatedDateTime.CurrentValue = UnformatDateTime(UpdatedDateTime.CurrentValue, UpdatedDateTime.FormatPattern);
            UpdatedByUserID.CurrentValue = UpdatedByUserID.FormValue;
            Perbaikan.CurrentValue = Perbaikan.FormValue;
        }

        // Load row based on key values
        public async Task<bool> LoadRow()
        {
            string filter = GetRecordFilter();

            // Call Row Selecting event
            RowSelecting(ref filter);

            // Load SQL based on filter
            CurrentFilter = filter;
            string sql = CurrentSql;
            bool res = false;
            try {
                var row = await Connection.GetRowAsync(sql);
                if (row != null) {
                    await LoadRowValues(row);
                    res = true;
                } else {
                    return false;
                }
            } catch {
                if (Config.Debug)
                    throw;
            }
            return res;
        }

        #pragma warning disable 162, 168, 1998, 4014
        // Load row values from data reader
        public async Task LoadRowValues(DbDataReader? dr = null) => await LoadRowValues(GetDictionary(dr));

        // Load row values from recordset
        public async Task LoadRowValues(Dictionary<string, object>? row)
        {
            row ??= NewRow();

            // Call Row Selected event
            RowSelected(row);
            ID.SetDbValue(row["ID"]);
            Code.SetDbValue(row["Code"]);
            _Name.SetDbValue(row["Name"]);
            Loc.SetDbValue(row["Loc"]);
            Status.SetDbValue((ConvertToBool(row["Status"]) ? "1" : "0"));
            Incident.SetDbValue(row["Incident"]);
            CreatedDateTime.SetDbValue(row["CreatedDateTime"]);
            CreatedByUserID.SetDbValue(row["CreatedByUserID"]);
            UpdatedDateTime.SetDbValue(row["UpdatedDateTime"]);
            UpdatedByUserID.SetDbValue(row["UpdatedByUserID"]);
            Perbaikan.SetDbValue((ConvertToBool(row["Perbaikan"]) ? "1" : "0"));
        }
        #pragma warning restore 162, 168, 1998, 4014

        // Return a row with default values
        protected Dictionary<string, object> NewRow() {
            var row = new Dictionary<string, object>();
            row.Add("ID", ID.DefaultValue ?? DbNullValue); // DN
            row.Add("Code", Code.DefaultValue ?? DbNullValue); // DN
            row.Add("Name", _Name.DefaultValue ?? DbNullValue); // DN
            row.Add("Loc", Loc.DefaultValue ?? DbNullValue); // DN
            row.Add("Status", Status.DefaultValue ?? DbNullValue); // DN
            row.Add("Incident", Incident.DefaultValue ?? DbNullValue); // DN
            row.Add("CreatedDateTime", CreatedDateTime.DefaultValue ?? DbNullValue); // DN
            row.Add("CreatedByUserID", CreatedByUserID.DefaultValue ?? DbNullValue); // DN
            row.Add("UpdatedDateTime", UpdatedDateTime.DefaultValue ?? DbNullValue); // DN
            row.Add("UpdatedByUserID", UpdatedByUserID.DefaultValue ?? DbNullValue); // DN
            row.Add("Perbaikan", Perbaikan.DefaultValue ?? DbNullValue); // DN
            return row;
        }

        #pragma warning disable 618, 1998
        // Load old record
        protected async Task<Dictionary<string, object>?> LoadOldRecord(DatabaseConnectionBase<SqlConnection, SqlCommand, SqlDataReader, SqlDbType>? cnn = null) {
            // Load old record
            Dictionary<string, object>? row = null;
            bool validKey = !Empty(OldKey);
            if (validKey) {
                SetKey(OldKey);
                CurrentFilter = GetRecordFilter();
                string sql = CurrentSql;
                try {
                    row = await (cnn ?? Connection).GetRowAsync(sql);
                } catch {
                    row = null;
                }
            }
            await LoadRowValues(row); // Load row values
            return row;
        }
        #pragma warning restore 618, 1998

        #pragma warning disable 1998
        // Render row values based on field settings
        public async Task RenderRow()
        {
            // Call Row Rendering event
            RowRendering();

            // Common render codes for all row types

            // ID
            ID.RowCssClass = "row";

            // Code
            Code.RowCssClass = "row";

            // Name
            _Name.RowCssClass = "row";

            // Loc
            Loc.RowCssClass = "row";

            // Status
            Status.RowCssClass = "row";

            // Incident
            Incident.RowCssClass = "row";

            // CreatedDateTime
            CreatedDateTime.RowCssClass = "row";

            // CreatedByUserID
            CreatedByUserID.RowCssClass = "row";

            // UpdatedDateTime
            UpdatedDateTime.RowCssClass = "row";

            // UpdatedByUserID
            UpdatedByUserID.RowCssClass = "row";

            // Perbaikan
            Perbaikan.RowCssClass = "row";

            // View row
            if (RowType == RowType.View) {
                // ID
                ID.ViewValue = ID.CurrentValue;
                ID.ViewCustomAttributes = "";

                // Code
                Code.ViewValue = ConvertToString(Code.CurrentValue); // DN
                Code.ViewCustomAttributes = "";

                // Name
                _Name.ViewValue = ConvertToString(_Name.CurrentValue); // DN
                _Name.ViewCustomAttributes = "";

                // Loc
                Loc.ViewValue = ConvertToString(Loc.CurrentValue); // DN
                Loc.ViewCustomAttributes = "";

                // Status
                if (ConvertToBool(Status.CurrentValue)) {
                    Status.ViewValue = Status.TagCaption(1) != "" ? Status.TagCaption(1) : "Yes";
                } else {
                    Status.ViewValue = Status.TagCaption(2) != "" ? Status.TagCaption(2) : "No";
                }
                Status.ViewCustomAttributes = "";

                // Incident
                Incident.ViewValue = Incident.CurrentValue;
                Incident.ViewValue = FormatNumber(Incident.ViewValue, Incident.FormatPattern);
                Incident.ViewCustomAttributes = "";

                // CreatedDateTime
                CreatedDateTime.ViewValue = CreatedDateTime.CurrentValue;
                CreatedDateTime.ViewValue = FormatDateTime(CreatedDateTime.ViewValue, CreatedDateTime.FormatPattern);
                CreatedDateTime.ViewCustomAttributes = "";

                // CreatedByUserID
                CreatedByUserID.ViewValue = CreatedByUserID.CurrentValue;
                CreatedByUserID.ViewValue = FormatNumber(CreatedByUserID.ViewValue, CreatedByUserID.FormatPattern);
                CreatedByUserID.ViewCustomAttributes = "";

                // UpdatedDateTime
                UpdatedDateTime.ViewValue = UpdatedDateTime.CurrentValue;
                UpdatedDateTime.ViewValue = FormatDateTime(UpdatedDateTime.ViewValue, UpdatedDateTime.FormatPattern);
                UpdatedDateTime.ViewCustomAttributes = "";

                // UpdatedByUserID
                UpdatedByUserID.ViewValue = UpdatedByUserID.CurrentValue;
                UpdatedByUserID.ViewValue = FormatNumber(UpdatedByUserID.ViewValue, UpdatedByUserID.FormatPattern);
                UpdatedByUserID.ViewCustomAttributes = "";

                // Perbaikan
                if (ConvertToBool(Perbaikan.CurrentValue)) {
                    Perbaikan.ViewValue = Perbaikan.TagCaption(1) != "" ? Perbaikan.TagCaption(1) : "Yes";
                } else {
                    Perbaikan.ViewValue = Perbaikan.TagCaption(2) != "" ? Perbaikan.TagCaption(2) : "No";
                }
                Perbaikan.ViewCustomAttributes = "";

                // ID
                ID.HrefValue = "";

                // Code
                Code.HrefValue = "";

                // Name
                _Name.HrefValue = "";

                // Loc
                Loc.HrefValue = "";

                // Status
                Status.HrefValue = "";

                // Incident
                Incident.HrefValue = "";

                // CreatedDateTime
                CreatedDateTime.HrefValue = "";

                // CreatedByUserID
                CreatedByUserID.HrefValue = "";

                // UpdatedDateTime
                UpdatedDateTime.HrefValue = "";

                // UpdatedByUserID
                UpdatedByUserID.HrefValue = "";

                // Perbaikan
                Perbaikan.HrefValue = "";
            } else if (RowType == RowType.Edit) {
                // ID
                ID.SetupEditAttributes();
                ID.EditValue = ID.CurrentValue;
                ID.ViewCustomAttributes = "";

                // Code
                Code.SetupEditAttributes();
                if (!Code.Raw)
                    Code.CurrentValue = HtmlDecode(Code.CurrentValue);
                Code.EditValue = HtmlEncode(Code.CurrentValue);
                Code.PlaceHolder = RemoveHtml(Code.Caption);

                // Name
                _Name.SetupEditAttributes();
                if (!_Name.Raw)
                    _Name.CurrentValue = HtmlDecode(_Name.CurrentValue);
                _Name.EditValue = HtmlEncode(_Name.CurrentValue);
                _Name.PlaceHolder = RemoveHtml(_Name.Caption);

                // Loc
                Loc.SetupEditAttributes();
                if (!Loc.Raw)
                    Loc.CurrentValue = HtmlDecode(Loc.CurrentValue);
                Loc.EditValue = HtmlEncode(Loc.CurrentValue);
                Loc.PlaceHolder = RemoveHtml(Loc.Caption);

                // Status
                Status.EditValue = Status.Options(false);
                Status.PlaceHolder = RemoveHtml(Status.Caption);

                // Incident
                Incident.SetupEditAttributes();
                Incident.EditValue = Incident.CurrentValue; // DN
                Incident.PlaceHolder = RemoveHtml(Incident.Caption);
                if (!Empty(Incident.EditValue) && IsNumeric(Incident.EditValue))
                    Incident.EditValue = FormatNumber(Incident.EditValue, null);

                // CreatedDateTime
                CreatedDateTime.SetupEditAttributes();
                CreatedDateTime.EditValue = FormatDateTime(CreatedDateTime.CurrentValue, CreatedDateTime.FormatPattern); // DN
                CreatedDateTime.PlaceHolder = RemoveHtml(CreatedDateTime.Caption);

                // CreatedByUserID
                CreatedByUserID.SetupEditAttributes();
                CreatedByUserID.EditValue = CreatedByUserID.CurrentValue; // DN
                CreatedByUserID.PlaceHolder = RemoveHtml(CreatedByUserID.Caption);
                if (!Empty(CreatedByUserID.EditValue) && IsNumeric(CreatedByUserID.EditValue))
                    CreatedByUserID.EditValue = FormatNumber(CreatedByUserID.EditValue, null);

                // UpdatedDateTime
                UpdatedDateTime.SetupEditAttributes();
                UpdatedDateTime.EditValue = FormatDateTime(UpdatedDateTime.CurrentValue, UpdatedDateTime.FormatPattern); // DN
                UpdatedDateTime.PlaceHolder = RemoveHtml(UpdatedDateTime.Caption);

                // UpdatedByUserID
                UpdatedByUserID.SetupEditAttributes();
                UpdatedByUserID.EditValue = UpdatedByUserID.CurrentValue; // DN
                UpdatedByUserID.PlaceHolder = RemoveHtml(UpdatedByUserID.Caption);
                if (!Empty(UpdatedByUserID.EditValue) && IsNumeric(UpdatedByUserID.EditValue))
                    UpdatedByUserID.EditValue = FormatNumber(UpdatedByUserID.EditValue, null);

                // Perbaikan
                Perbaikan.EditValue = Perbaikan.Options(false);
                Perbaikan.PlaceHolder = RemoveHtml(Perbaikan.Caption);

                // Edit refer script

                // ID
                ID.HrefValue = "";

                // Code
                Code.HrefValue = "";

                // Name
                _Name.HrefValue = "";

                // Loc
                Loc.HrefValue = "";

                // Status
                Status.HrefValue = "";

                // Incident
                Incident.HrefValue = "";

                // CreatedDateTime
                CreatedDateTime.HrefValue = "";

                // CreatedByUserID
                CreatedByUserID.HrefValue = "";

                // UpdatedDateTime
                UpdatedDateTime.HrefValue = "";

                // UpdatedByUserID
                UpdatedByUserID.HrefValue = "";

                // Perbaikan
                Perbaikan.HrefValue = "";
            }
            if (RowType == RowType.Add || RowType == RowType.Edit || RowType == RowType.Search) // Add/Edit/Search row
                SetupFieldTitles();

            // Call Row Rendered event
            if (RowType != RowType.AggregateInit)
                RowRendered();
        }
        #pragma warning restore 1998

        #pragma warning disable 1998
        // Validate form
        protected async Task<bool> ValidateForm() {
            // Check if validation required
            if (!Config.ServerValidate)
                return true;
            bool validateForm = true;
            if (ID.Required) {
                if (!ID.IsDetailKey && Empty(ID.FormValue)) {
                    ID.AddErrorMessage(ConvertToString(ID.RequiredErrorMessage).Replace("%s", ID.Caption));
                }
            }
            if (Code.Required) {
                if (!Code.IsDetailKey && Empty(Code.FormValue)) {
                    Code.AddErrorMessage(ConvertToString(Code.RequiredErrorMessage).Replace("%s", Code.Caption));
                }
            }
            if (_Name.Required) {
                if (!_Name.IsDetailKey && Empty(_Name.FormValue)) {
                    _Name.AddErrorMessage(ConvertToString(_Name.RequiredErrorMessage).Replace("%s", _Name.Caption));
                }
            }
            if (Loc.Required) {
                if (!Loc.IsDetailKey && Empty(Loc.FormValue)) {
                    Loc.AddErrorMessage(ConvertToString(Loc.RequiredErrorMessage).Replace("%s", Loc.Caption));
                }
            }
            if (Status.Required) {
                if (Empty(Status.FormValue)) {
                    Status.AddErrorMessage(ConvertToString(Status.RequiredErrorMessage).Replace("%s", Status.Caption));
                }
            }
            if (Incident.Required) {
                if (!Incident.IsDetailKey && Empty(Incident.FormValue)) {
                    Incident.AddErrorMessage(ConvertToString(Incident.RequiredErrorMessage).Replace("%s", Incident.Caption));
                }
            }
            if (!CheckInteger(Incident.FormValue)) {
                Incident.AddErrorMessage(Incident.GetErrorMessage(false));
            }
            if (CreatedDateTime.Required) {
                if (!CreatedDateTime.IsDetailKey && Empty(CreatedDateTime.FormValue)) {
                    CreatedDateTime.AddErrorMessage(ConvertToString(CreatedDateTime.RequiredErrorMessage).Replace("%s", CreatedDateTime.Caption));
                }
            }
            if (!CheckDate(CreatedDateTime.FormValue, CreatedDateTime.FormatPattern)) {
                CreatedDateTime.AddErrorMessage(CreatedDateTime.GetErrorMessage(false));
            }
            if (CreatedByUserID.Required) {
                if (!CreatedByUserID.IsDetailKey && Empty(CreatedByUserID.FormValue)) {
                    CreatedByUserID.AddErrorMessage(ConvertToString(CreatedByUserID.RequiredErrorMessage).Replace("%s", CreatedByUserID.Caption));
                }
            }
            if (!CheckInteger(CreatedByUserID.FormValue)) {
                CreatedByUserID.AddErrorMessage(CreatedByUserID.GetErrorMessage(false));
            }
            if (UpdatedDateTime.Required) {
                if (!UpdatedDateTime.IsDetailKey && Empty(UpdatedDateTime.FormValue)) {
                    UpdatedDateTime.AddErrorMessage(ConvertToString(UpdatedDateTime.RequiredErrorMessage).Replace("%s", UpdatedDateTime.Caption));
                }
            }
            if (!CheckDate(UpdatedDateTime.FormValue, UpdatedDateTime.FormatPattern)) {
                UpdatedDateTime.AddErrorMessage(UpdatedDateTime.GetErrorMessage(false));
            }
            if (UpdatedByUserID.Required) {
                if (!UpdatedByUserID.IsDetailKey && Empty(UpdatedByUserID.FormValue)) {
                    UpdatedByUserID.AddErrorMessage(ConvertToString(UpdatedByUserID.RequiredErrorMessage).Replace("%s", UpdatedByUserID.Caption));
                }
            }
            if (!CheckInteger(UpdatedByUserID.FormValue)) {
                UpdatedByUserID.AddErrorMessage(UpdatedByUserID.GetErrorMessage(false));
            }
            if (Perbaikan.Required) {
                if (Empty(Perbaikan.FormValue)) {
                    Perbaikan.AddErrorMessage(ConvertToString(Perbaikan.RequiredErrorMessage).Replace("%s", Perbaikan.Caption));
                }
            }

            // Return validate result
            validateForm = validateForm && !HasInvalidFields();

            // Call Form CustomValidate event
            string formCustomError = "";
            validateForm = validateForm && FormCustomValidate(ref formCustomError);
            if (!Empty(formCustomError))
                FailureMessage = formCustomError;
            return validateForm;
        }
        #pragma warning restore 1998

        // Update record based on key values
        #pragma warning disable 168, 219

        protected async Task<JsonBoolResult> EditRow() { // DN
            bool result = false;
            Dictionary<string, object> rsold;
            string oldKeyFilter = GetRecordFilter();
            string filter = ApplyUserIDFilters(oldKeyFilter);

            // Load old row
            CurrentFilter = filter;
            string sql = CurrentSql;
            try {
                using var rsedit = await Connection.GetDataReaderAsync(sql);
                if (rsedit == null || !await rsedit.ReadAsync()) {
                    FailureMessage = Language.Phrase("NoRecord"); // Set no record message
                    return JsonBoolResult.FalseResult;
                }
                rsold = Connection.GetRow(rsedit);
                LoadDbValues(rsold);
            } catch (Exception e) {
                if (Config.Debug)
                    throw;
                FailureMessage = e.Message;
                return JsonBoolResult.FalseResult;
            }

            // Set new row
            Dictionary<string, object> rsnew = new ();

            // Code
            Code.SetDbValue(rsnew, Code.CurrentValue, Code.ReadOnly);

            // Name
            _Name.SetDbValue(rsnew, _Name.CurrentValue, _Name.ReadOnly);

            // Loc
            Loc.SetDbValue(rsnew, Loc.CurrentValue, Loc.ReadOnly);

            // Status
            Status.SetDbValue(rsnew, ConvertToBool(Status.CurrentValue), Status.ReadOnly);

            // Incident
            Incident.SetDbValue(rsnew, Incident.CurrentValue, Incident.ReadOnly);

            // CreatedDateTime
            CreatedDateTime.SetDbValue(rsnew, ConvertToDateTime(CreatedDateTime.CurrentValue, CreatedDateTime.FormatPattern), CreatedDateTime.ReadOnly);

            // CreatedByUserID
            CreatedByUserID.SetDbValue(rsnew, CreatedByUserID.CurrentValue, CreatedByUserID.ReadOnly);

            // UpdatedDateTime
            UpdatedDateTime.SetDbValue(rsnew, ConvertToDateTime(UpdatedDateTime.CurrentValue, UpdatedDateTime.FormatPattern), UpdatedDateTime.ReadOnly);

            // UpdatedByUserID
            UpdatedByUserID.SetDbValue(rsnew, UpdatedByUserID.CurrentValue, UpdatedByUserID.ReadOnly);

            // Perbaikan
            Perbaikan.SetDbValue(rsnew, ConvertToBool(Perbaikan.CurrentValue), Perbaikan.ReadOnly);

            // Update current values
            SetCurrentValues(rsnew);

            // Call Row Updating event
            bool updateRow = RowUpdating(rsold, rsnew);
            if (updateRow) {
                try {
                    if (rsnew.Count > 0)
                        result = await UpdateAsync(rsnew, null, rsold) > 0;
                    else
                        result = true;
                    if (result) {
                    }
                } catch (Exception e) {
                    if (Config.Debug)
                        throw;
                    FailureMessage = e.Message;
                    return JsonBoolResult.FalseResult;
                }
            } else {
                if (!Empty(SuccessMessage) || !Empty(FailureMessage)) {
                    // Use the message, do nothing
                } else if (!Empty(CancelMessage)) {
                    FailureMessage = CancelMessage;
                    CancelMessage = "";
                } else {
                    FailureMessage = Language.Phrase("UpdateCancelled");
                }
                result = false;
            }

            // Call Row Updated event
            if (result)
                RowUpdated(rsold, rsnew);

            // Write JSON for API request
            Dictionary<string, object> d = new ();
            d.Add("success", result);
            if (IsJsonResponse() && result) {
                if (GetRecordFromDictionary(rsnew) is var row && row != null) {
                    string table = TableVar;
                    d.Add(table, row);
                }
                d.Add("action", Config.ApiEditAction);
                d.Add("version", Config.ProductVersion);
                return new JsonBoolResult(d, true);
            }
            return new JsonBoolResult(d, result);
        }

        // Set up Breadcrumb
        protected void SetupBreadcrumb() {
            var breadcrumb = new Breadcrumb();
            string url = CurrentUrl();
            breadcrumb.Add("list", TableVar, AppPath(AddMasterUrl("Cctv2List")), "", TableVar, true);
            string pageId = "edit";
            breadcrumb.Add("edit", pageId, url);
            CurrentBreadcrumb = breadcrumb;
        }

        // Setup lookup options
        public async Task SetupLookupOptions(DbField fld)
        {
            if (fld.Lookup == null)
                return;
            Func<string>? lookupFilter = null;
            dynamic conn = Connection;
            if (fld.Lookup.Options.Count is int c && c == 0) {
                // Always call to Lookup.GetSql so that user can setup Lookup.Options in Lookup Selecting server event
                var sql = fld.Lookup.GetSql(false, "", lookupFilter, this);

                // Set up lookup cache
                if (!fld.HasLookupOptions && fld.UseLookupCache && !Empty(sql) && fld.Lookup.ParentFields.Count == 0 && fld.Lookup.Options.Count == 0) {
                    int totalCnt = await TryGetRecordCountAsync(sql, conn);
                    if (totalCnt > fld.LookupCacheCount) // Total count > cache count, do not cache
                        return;
                    var dict = new Dictionary<string, Dictionary<string, object>>();
                    var values = new List<object>();
                    List<Dictionary<string, object>> rs = await conn.GetRowsAsync(sql);
                    if (rs != null) {
                        for (int i = 0; i < rs.Count; i++) {
                            var row = rs[i];
                            row = fld.Lookup?.RenderViewRow(row, Resolve(fld.Lookup.LinkTable));
                            string key = row?.Values.First()?.ToString() ?? String.Empty;
                            if (!dict.ContainsKey(key) && row != null)
                                dict.Add(key, row);
                        }
                    }
                    fld.Lookup?.SetOptions(dict);
                }
            }
        }

        // Close recordset
        public void CloseRecordset()
        {
            using (Recordset) {} // Dispose
        }

        // Set up starting record parameters
        public void SetupStartRecord()
        {
            // Exit if DisplayRecords = 0
            if (DisplayRecords == 0)
                return;
            string pageNo = Get(Config.TablePageNumber);
            string startRec = Get(Config.TableStartRec);
            bool infiniteScroll = false;
            string recordNo = !Empty(pageNo) ? pageNo : startRec; // Record number = page number or start record
            if (!Empty(recordNo) && IsNumeric(recordNo))
                StartRecord = ConvertToInt(recordNo);
            else
                StartRecord = StartRecordNumber;

            // Check if correct start record counter
            if (StartRecord <= 0) // Avoid invalid start record counter
                StartRecord = 1; // Reset start record counter
            else if (StartRecord > TotalRecords) // Avoid starting record > total records
                StartRecord = ((TotalRecords - 1) / DisplayRecords) * DisplayRecords + 1; // Point to last page first record
            else if ((StartRecord - 1) % DisplayRecords != 0)
                StartRecord = ((StartRecord - 1) / DisplayRecords) * DisplayRecords + 1; // Point to page boundary
            if (!infiniteScroll)
                StartRecordNumber = StartRecord;
        }

        // Get page count
        public int PageCount
        {
            get {
                return ConvertToInt(Math.Ceiling((double)TotalRecords / DisplayRecords));
            }
        }

        // Page Load event
        public virtual void PageLoad() {
            //Log("Page Load");
        }

        // Page Unload event
        public virtual void PageUnload() {
            //Log("Page Unload");
        }

        // Page Redirecting event
        public virtual void PageRedirecting(ref string url) {
            //url = newurl;
        }

        // Message Showing event
        // type = ""|"success"|"failure"|"warning"
        public virtual void MessageShowing(ref string msg, string type) {
            // Note: Do not change msg outside the following 4 cases.
            if (type == "success") {
                //msg = "your success message";
            } else if (type == "failure") {
                //msg = "your failure message";
            } else if (type == "warning") {
                //msg = "your warning message";
            } else {
                //msg = "your message";
            }
        }

        // Page Load event
        public virtual void PageRender() {
            //Log("Page Render");
        }

        // Page Data Rendering event
        public virtual void PageDataRendering(ref string header) {
            // Example:
            //header = "your header";
        }

        // Page Data Rendered event
        public virtual void PageDataRendered(ref string footer) {
            // Example:
            //footer = "your footer";
        }

        // Page Breaking event
        public void PageBreaking(ref bool brk, ref string content) {
            // Example:
            //	brk = false; // Skip page break, or
            //	content = "<div style=\"page-break-after:always;\">&nbsp;</div>"; // Modify page break content
        }

        // Form Custom Validate event
        public virtual bool FormCustomValidate(ref string customError) {
            //Return error message in customError
            return true;
        }
    } // End page class
} // End Partial class
