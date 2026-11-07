namespace API.Domain.Common.Constants
{
    public static class DefaultRoles
    {
        public const string SuperAdmin = "SuperAdmin";
        public const string Admin = "Admin";
        public const string Manager = "Manager";
        public const string Customer = "Customer";
        public const string Employee = "Employee";

        public static readonly IReadOnlyList<(string Name, string Description)> All =
        [
            (SuperAdmin, "Has full access to all system features and administrative functions."),
            (Admin, "Manages restaurant operations, users, branches, products, inventory, and system settings."),
            (Manager, "Manages daily operations of assigned restaurant branches, including employees, inventory, orders, and reports."),
            (Customer, "Can browse products, place orders, manage their profile, and make payments."),
            (Employee, "General role for all employee.")
        ];
    }
}
