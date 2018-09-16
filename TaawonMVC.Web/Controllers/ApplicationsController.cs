using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TaawonMVC.Applications;
using TaawonMVC.Applications.DTO;
using TaawonMVC.Buildings;
using TaawonMVC.Buildings.DTO;
using TaawonMVC.BuildingType;
using TaawonMVC.BuildingUnits;
using TaawonMVC.BuildingUnits.DTO;
using TaawonMVC.BuildingUses;
using TaawonMVC.InterventionType;
using TaawonMVC.Neighborhood;
using TaawonMVC.PropertyOwnership;
using TaawonMVC.RestorationType;
using TaawonMVC.Web.Models.Applications;

namespace TaawonMVC.Web.Controllers
{
    public class ApplicationsController : Controller
    {
        private readonly IApplicationsAppService _applicationsAppService;
        private readonly IBuildingsAppService _buildingsAppService;
        private readonly IBuildingUnitsAppService _buildingUnitsAppService;
        private readonly IPropertyOwnershipAppService _propertyOwnershipAppService;
        private readonly IInterventionTypeAppService _interventionTypeAppService;
        private readonly IRestorationTypeAppService _restorationTypeAppService;
        private readonly INeighborhoodAppService _neighborhoodAppService;
        private readonly IBuildingTypeAppService _buildingTypeAppService;
        private readonly IBuildingUsesAppService _buildingUsesAppService;

        public ApplicationsController(IApplicationsAppService applicationsAppServic,
            IBuildingsAppService buildingsAppService, 
            IBuildingUnitsAppService buildingUnitsAppService, 
            IPropertyOwnershipAppService propertyOwnershipAppService, 
            IInterventionTypeAppService interventionTypeAppService, 
            IRestorationTypeAppService restorationTypeAppService,
            INeighborhoodAppService neighborhoodAppService,
            IBuildingTypeAppService buildingTypeAppService,
            IBuildingUsesAppService buildingUsesAppService)
        {
            _applicationsAppService = applicationsAppServic;
            _buildingsAppService = buildingsAppService;
            _buildingUnitsAppService = buildingUnitsAppService;
            _propertyOwnershipAppService = propertyOwnershipAppService;
            _interventionTypeAppService = interventionTypeAppService;
            _restorationTypeAppService = restorationTypeAppService;
            _neighborhoodAppService = neighborhoodAppService;
            _buildingTypeAppService = buildingTypeAppService;
            _buildingUsesAppService = buildingUsesAppService;



        }
        // GET: Applications
        public ActionResult Index()
        {
            var applications = _applicationsAppService.getAllApplications();
            var applicationsViewModel = new ApplicationsViewModel()
            {
             Applications = applications
            };
            return View("Applications", applicationsViewModel);
        }

        public ActionResult ApplicationForm()
        {
            // get the list of building uses
            var buildingUses = _buildingUsesAppService.getAllBuildingUses();
            //get the list of buildingTypes
            var buildingTypes = _buildingTypeAppService.getAllBuildingtype().ToList();
            // get the list of neighborhoods
            var neighborhoods = _neighborhoodAppService.GetAllNeighborhood().ToList();
            // get all of buildings 
            var buildings = _buildingsAppService.getAllBuildings();
            // get all of restoration types 
            var restorationTypes = _restorationTypeAppService.getAllResorationTypes();
            // get all of intervention types 
            var interventionTypes = _interventionTypeAppService.getAllInterventionTypes();
            // get all applications 
            var applications = _applicationsAppService.getAllApplications().ToList();
            // get all property ownerships 
            var propertyOwnerships = _propertyOwnershipAppService.getAllPropertyOwnerships();
            // populate yes no drop down list 
            var yesOrNo = new List<string>
            {
                "True",
                "False"
            };
            var fullNameList = new List<string>();
            foreach(var application in applications)
            {
                if (!String.IsNullOrWhiteSpace(application.fullName))
                {
                    fullNameList.Add(application.fullName);
                }
            }
            var fullNameArray = fullNameList.Distinct().ToArray();
            var applicationsViewModel = new ApplicationsViewModel()
            {
                fullNameArray = fullNameArray,
                buildingOutput = new GetBuildingsOutput(),
                PropertyOwnerShips= propertyOwnerships,
                YesOrNo= new SelectList(yesOrNo),
                InterventionTypes= interventionTypes,
                RestorationTypes = restorationTypes ,
                Applications = applications,
                Buildings = buildings,
                Building = new CreateBuildingsInput(),
                Neighborhoods = neighborhoods,
                BuildingTypes = buildingTypes,
                BuildingUses = buildingUses 



            };

            return View("ApplicationForm", applicationsViewModel);
        }

        public ActionResult PopulateApplicationForm(int buildingId)
        { 
            
            
            //instantiate object GetBuidlingsInput to get the building entity with given id 
            var getBuildingInput = new GetBuidlingsInput()
            {
              Id= buildingId
            };
            // retrieve the building with givin id 
            var building = _buildingsAppService.getBuildingsById(getBuildingInput);
            var buildingUnits = _buildingUnitsAppService.getAllBuildingUnits().ToList();
            var BuildingUnits = from BU in buildingUnits where BU.BuildingId == buildingId select BU;
            // declare viewmodel object to pass data to view 
            var applicationViewModel = new ApplicationsViewModel()
            {
                 buildingOutput = building
                
            };

             return Json(applicationViewModel, JsonRequestBehavior.AllowGet);
          //  return View("ApplicationForm", applicationViewModel);
        }

        public ActionResult PopulateDropDownListBuildingUnits(int buildingId)
        {
            var buildingUnitsApp = _buildingUnitsAppService.getAllBuildingUnits();
            var buildingUnits = (from BU in buildingUnitsApp where BU.BuildingId == buildingId select BU).ToList();
            List<SelectListItem> buildingUnitsList = new List<SelectListItem>();
            
              foreach (var buildingUnit in buildingUnits)
                 {
                     buildingUnitsList.Add(new SelectListItem { Text =buildingUnit.ResidentName, Value = buildingUnit.Id.ToString() });
                 }
        
            //var ListOfBuildingUnits = new List<string>();
            //foreach (var BuildingUnit in BuildingUnits)
            //{
            //    ListOfBuildingUnits.Add(BuildingUnit.ResidentName);
            //}
            //var applicationViewModelPL = new ApplicationsViewModel()
            //{
            //  // BuildingUnitList = ListOfBuildingUnits
            //   //  BuildingUnits = buildingUnits
            //      BuildingUnitList= buildingUnitsList,
            //    BuildingUnit = new GetBuildingUnitsOutput()
            //};
                return Json(buildingUnitsList, JsonRequestBehavior.AllowGet);
           // return View("ApplicationForm", applicationViewModelPL);
            
        }

        public ActionResult CreateApplication(CreateApplicationsInput model )
        {
            var application = new CreateApplicationsInput();
             application.phoneNumber1 = model.phoneNumber1;
             application.fullName = model.fullName;
             application.phoneNumber2 = model.phoneNumber2;
             application.isThereFundingOrPreviousRestoration = model.isThereFundingOrPreviousRestoration;
             application.isThereInterestedRepairingEntity = model.isThereInterestedRepairingEntity;
             application.housingSince = model.housingSince;
             application.previousRestorationSource = model.previousRestorationSource;
             application.interestedRepairingEntityName = model.interestedRepairingEntityName;
             application.PropertyOwnerShipId =Convert.ToInt32(Request["PropertyOwnerShip"]) ;
             application.otherOwnershipType = model.otherOwnershipType;
             application.interventionTypeId= Convert.ToInt32(Request["interventionTypeName"]);
             application.otherRestorationType = model.otherRestorationType;
             application.propertyStatusDescription = model.propertyStatusDescription;
             application.requiredRestoration = model.requiredRestoration;
             application.buildingId = Convert.ToInt32(Request["BuildingId2"]);
             application.buildingUnitId = Convert.ToInt32(Request["buildingUnitId"]);
            // ==== get of restoration types which it is multi select drop down list ======
             var restorationTypes = Request["example-getting-started"];
             string[] restorationTypesSplited = restorationTypes.Split(',');
             byte[] restorationTypesArray = new byte[restorationTypesSplited.Length];
             for (var i = 0; i < restorationTypesArray.Length; i++)
             {
                restorationTypesArray[i] =Convert.ToByte(restorationTypesSplited[i]) ;
             }

              application.restorationTypeIds = restorationTypesArray;
            // ====== end of RestorationTypes

            _applicationsAppService.Create(application);
            // ==== get list of applications ==============
            var applications = _applicationsAppService.getAllApplications();
            var applicationsViewModel = new ApplicationsViewModel()
            {
                Applications = applications
            };

            return View("Applications", applicationsViewModel);

        }

        public ActionResult EditApplication(int appId)

        {
            var yesOrNo = new List<string>
            {
                "True",
                "False"
            };

            var getApplicationInput = new GetApplicationsInput()
            {
              Id=appId
            };
            
            
            // get application according to givin application Id  
            var application = _applicationsAppService.GetApplicationById(getApplicationInput);
            // get the list of buildings 
            var buildings = _buildingsAppService.getAllBuildings();
            // get the list of building units
            var buildingUnits = _buildingUnitsAppService.getAllBuildingUnits();
            var buildingUnitsByBuildingId = from BU in buildingUnits where BU.BuildingId == application.buildingId select BU;
            // get building information by buildingId in application
            var getBuildingInput = new GetBuidlingsInput()
            {
                Id = application.buildingId
            };
            // get the building information by BuildingId
            var building = _buildingsAppService.getBuildingsById(getBuildingInput);
            // get the information of spicific building unit 
            var getBuildingUnitInput = new GetBuildingUnitsInput()
            {
                Id = application.buildingUnitId
            };
            var buildingUnit = _buildingUnitsAppService.GetBuildingUnitsById(getBuildingUnitInput);
            // get list of propertyOwnerships 
            var propertyOwnerships = _propertyOwnershipAppService.getAllPropertyOwnerships();
            // get list of interventionTypes
            var interventionTypes = _interventionTypeAppService.getAllInterventionTypes();
            // get list of restorationTypes
            var restorationType = _restorationTypeAppService.getAllResorationTypes();


            var ApplicationViewModel = new ApplicationsViewModel()
            {
                applicationsOutput = application,
                Buildings = buildings,
                BuildingUnits = buildingUnitsByBuildingId,
                buildingOutput = building,
                YesOrNo = new SelectList(yesOrNo),
                PropertyOwnerShips=propertyOwnerships,
                BuildingUnit= buildingUnit,
                InterventionTypes= interventionTypes,
                RestorationTypes= restorationType

            };



            return View("_EditApplicationsModal", ApplicationViewModel);
        }

        public ActionResult UpdateApplication (UpdateApplicationsInput model)
        {
            var updateApplication = new UpdateApplicationsInput();
            updateApplication.buildingId =Convert.ToInt32(Request["buildingnumber"]);
            updateApplication.buildingUnitId= Convert.ToInt32(Request["dropDownBuildingUnitApp"]);
            //==== get building and unit related to application for update ======
            var buildingInput = new GetBuidlingsInput()
            {
                Id = updateApplication.buildingId
            };
            var buildingUnitInput = new GetBuildingUnitsInput()
            {
                Id = updateApplication.buildingUnitId
            };
             var buildingApp = _buildingsAppService.getBuildingsById(buildingInput);
             var buildingUnitApp = _buildingUnitsAppService.GetBuildingUnitsById(buildingUnitInput);
             buildingApp.streetName = Request["buildingaddress"]; 
             buildingApp.isInHoush =Convert.ToBoolean(Request["IsInHoush"]);
             buildingApp.houshName = Request["HoushName"];
            // buildingUnitApp.BuildingId = updateApplication.buildingId;
             buildingUnitApp.ResidenceStatus= Request["HoushName"];
            //============================================
            // copy object from getBuildingInput to createBuildingInput
            var createBuildingInput = new CreateBuildingsInput()
            {
                numOfBuildingUnits = buildingApp.numOfBuildingUnits,

            };
            //======================================================

            updateApplication.fullName = model.fullName;
            updateApplication.phoneNumber1 = model.phoneNumber1;
            updateApplication.phoneNumber2 = model.phoneNumber2;
            updateApplication.isThereFundingOrPreviousRestoration = model.isThereFundingOrPreviousRestoration;
            updateApplication.isThereInterestedRepairingEntity = model.isThereInterestedRepairingEntity;
            updateApplication.housingSince = model.housingSince;
            updateApplication.previousRestorationSource = model.previousRestorationSource;
            updateApplication.interestedRepairingEntityName = model.interestedRepairingEntityName;
            updateApplication.PropertyOwnerShipId = Convert.ToInt32(Request["PropertyOwnerShip"]);
            updateApplication.otherOwnershipType = model.otherOwnershipType;
            updateApplication.interventionTypeId = Convert.ToInt32(Request["interventionTypeName"]);
            updateApplication.otherRestorationType = model.otherRestorationType;
            updateApplication.propertyStatusDescription = model.propertyStatusDescription;
            updateApplication.requiredRestoration = model.requiredRestoration;
            updateApplication.buildingId = Convert.ToInt32(Request["BuildingId2"]);
            updateApplication.buildingUnitId = Convert.ToInt32(Request["buildingUnitId"]);
            // ==== get of restoration types which it is multi select drop down list ======
            var restorationTypes = Request["example-getting-started"];
            string[] restorationTypesSplited = restorationTypes.Split(',');
            byte[] restorationTypesArray = new byte[restorationTypesSplited.Length];
            for (var i = 0; i < restorationTypesArray.Length; i++)
            {
                restorationTypesArray[i] = Convert.ToByte(restorationTypesSplited[i]);
            }

            updateApplication.restorationTypeIds = restorationTypesArray;
            // ====== end of RestorationTypes

            _applicationsAppService.Update(updateApplication);
           // _buildingsAppService.update((CreateBuildingsInput)buildingApp)

            return View("Applications");
        }
    }
}