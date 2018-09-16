using Abp.Application.Services;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaawonMVC.Applications.DTO;
using TaawonMVC.Models;

namespace TaawonMVC.Applications
{
    public class ApplicationsAppService : ApplicationService, IApplicationsAppService
    {
        private readonly IApplicationsManager _applicationsManager;

        public ApplicationsAppService(IApplicationsManager applicationsManager)
        {
            _applicationsManager = applicationsManager;

        }
        public async Task Create(CreateApplicationsInput input)
        {
            var output = Mapper.Map<CreateApplicationsInput, Models.Applications>(input);
           await _applicationsManager.create(output);
        }

        public void Delete(DeleteApplicationsInput input)
        {
            _applicationsManager.delete(input.Id);
        }

        public IEnumerable<GetApplicationsOutput> getAllApplications()
        {
           var applications = _applicationsManager.getAllApplicationsList().ToList();
            var output = Mapper.Map<List<Models.Applications>, List<GetApplicationsOutput>>(applications);
            return output;
        }

        public GetApplicationsOutput GetApplicationById(GetApplicationsInput input)
        {
           var application = _applicationsManager.getApplicationById(input.Id);
            var output = Mapper.Map<Models.Applications, GetApplicationsOutput>(application);
            return output;
        }

        public void Update(UpdateApplicationsInput input)
        {
            var output = Mapper.Map<UpdateApplicationsInput, Models.Applications>(input);
            _applicationsManager.update(output);
        }
    }
}
