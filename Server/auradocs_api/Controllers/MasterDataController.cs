using auradocs_api.Contexts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace auradocs_api.Controllers;

[ApiController]
[Route("[controller]")]
public class MasterDataController : ControllerBase
{
    private readonly AuradocsContext _auradocsContext;
    public MasterDataController(AuradocsContext auradocsContext)
    {
        _auradocsContext = auradocsContext;
    }

    [HttpGet("get-domainname-dropdown")]
    public async Task<IActionResult> getDomainNameDropDownOptions()
    {
        List<DropdownOption> domainNameList = await (from d in _auradocsContext.RegisterPageDomainNames
                                                    select new DropdownOption{
                                                                    Label = d.strOption,
                                                                    Value = d.uId
                                                                }).ToListAsync();
        return Ok(domainNameList);
    }

    [HttpGet("get-domainPracticearea-dropdown")]
    public async Task<IActionResult> getDomainPracticeAreaDropDownOptions()
    {
        List<PracticeAreaResponse> practiceAreaResponse = await (from dn in _auradocsContext.RegisterPageDomainNames
                                                    join dpn in _auradocsContext.RegisterPageDomainPracticeAreas
                                                    on dn.uId equals dpn.uKey 
                                                    select new PracticeAreaResponse
                                                    {
                                                        domainName = dn.strOption,
                                                        practiceArea = dpn.strValue,
                                                        value = dpn.uId
                                                    }).ToListAsync();
        Dictionary<string,List<DropdownOption>> practiceAreaObject = practiceAreaResponse.GroupBy(x => x.domainName).ToDictionary(
            u => u.Key,
            u => u.Select(x => new DropdownOption {
                 Label = x.practiceArea,
                 Value = x.value
                }).ToList()
        );
        return Ok(practiceAreaObject);
    }
}