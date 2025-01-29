using WireOps.Business.Application.Staffers;

namespace WireOps.Business.API.Features.Staffers
{
    public class StafferRecord
    {
        public required Guid id { get; set; }
        public required string email { get; set; }
        public string? user_id { get; set; }
        public required string given_name { get; set; }
        public required string family_name { get; set; }
        public required bool is_owner { get; set; }

        public static StafferRecord FromModel(StafferModel model)
        {
            return new StafferRecord
            {
                id = model.Id,
                email = model.Email,
                user_id = model.UserId,
                given_name = model.GivenName,
                family_name = model.FamilyName,
                is_owner = model.IsOwner
            };
        }
    }
}
