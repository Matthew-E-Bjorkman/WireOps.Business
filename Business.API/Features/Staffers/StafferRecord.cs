using WireOps.Business.Application.Staffers;

namespace WireOps.Business.API.Features.Staffers
{
    public class StafferRecord
    {
        public required Guid Id { get; set; }
        public required string Email { get; set; }
        public string? UserId { get; set; }
        public required string GivenName { get; set; }
        public required string FamilyName { get; set; }

        public static StafferRecord FromModel(StafferModel model)
        {
            return new StafferRecord {
                Id = model.Id,
                Email = model.Email,
                UserId = model.UserId,
                GivenName = model.GivenName,
                FamilyName = model.FamilyName
            };
        }
    }
}
