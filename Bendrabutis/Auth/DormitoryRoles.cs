namespace Bendrabutis.Auth
{
    public static class DormitoryRoles
    {
        public const string Admin = nameof(Admin);
        public const string Resident = nameof(Resident);
        public const string Visitor = nameof(Visitor);

        public static readonly IReadOnlyCollection<string> All = new[] {Admin, Resident, Visitor};
    }
}
