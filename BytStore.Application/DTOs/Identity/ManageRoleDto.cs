namespace BytStore.Application.DTOs.Identity
{
    public class ManageRolesDto
    {
        public string UserId { get; set; }
        public List<RolesDto> Roles { get; set; }
    }

    public class RolesDto
    {
        public string RoleName { get; set; }
        public bool IsSelected { get; set; }

    }
}
