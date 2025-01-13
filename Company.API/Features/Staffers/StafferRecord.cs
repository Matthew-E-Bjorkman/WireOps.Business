using WireOps.Company.Application.Staffers;

namespace WireOps.Company.API.Features.Staffers
{
    public class StafferRecord
    {
        public required Guid Id { get; set; }
        public required string Name { get; set; }

        public static StafferRecord MapFromModel(StafferModel model)
        {
            return new StafferRecord {
                Id = model.Id,
                Name = model.Name
            };
        }
    }
}
