using Business.API.Common.Records;
using WireOps.Business.Application.Companies;

namespace WireOps.Business.API.Features.Companies
{
    public class CompanyRecord
    {
        public required Guid Id { get; set; }
        public required string Name { get; set; }
        public required AddressRecord? Address { get; set; }

        public static CompanyRecord FromModel(CompanyModel model)
        {
            return new CompanyRecord {
                Id = model.Id,
                Name = model.Name,
                Address = AddressRecord.FromModel(model.Address)
            };
        }
    }
}
