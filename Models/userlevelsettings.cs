namespace ASPNETMaker2023.Models;

// Partial class
public partial class defa {
    // Configuration
    public static partial class Config
    {
        //
        // ASP.NET Maker 2023 user level settings
        //

        // User level info
        public static List<UserLevel> UserLevels = new ()
        {
            new () { Id = -2, Name = "Anonymous" }
        };

        // User level priv info
        public static List<UserLevelPermission> UserLevelPermissions = new ()
        {
            new () { Table = "{A82BC683-C15E-40D7-94E1-955AFED823D6}CCTV", Id = -2, Permission = 0 },
            new () { Table = "{A82BC683-C15E-40D7-94E1-955AFED823D6}User", Id = -2, Permission = 0 },
            new () { Table = "{A82BC683-C15E-40D7-94E1-955AFED823D6}Dashboard", Id = -2, Permission = 0 }
        };

        // User level table info // DN
        public static List<UserLevelTablePermission> UserLevelTablePermissions = new ()
        {
            new () { TableName = "CCTV", TableVar = "CCTV2", Caption = "CCTV 2", Allowed = true, ProjectId = "{A82BC683-C15E-40D7-94E1-955AFED823D6}", Url = "Cctv2List" },
            new () { TableName = "User", TableVar = "User2", Caption = "User", Allowed = true, ProjectId = "{A82BC683-C15E-40D7-94E1-955AFED823D6}", Url = "User2List" },
            new () { TableName = "Dashboard", TableVar = "Dashboard", Caption = "Dashboard", Allowed = true, ProjectId = "{A82BC683-C15E-40D7-94E1-955AFED823D6}", Url = "Dashboard" }
        };
    }
} // End Partial class
