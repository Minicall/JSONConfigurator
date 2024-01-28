using JSONConfigurator.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace JSONConfigurator.Controllers
{
    public class ConfigurationController : Controller
    {
        private readonly ConfigurationDbContext _dbContext;

        public ConfigurationController(ConfigurationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var configurations = _dbContext.Configurations.ToList();
            var hierarchyData = BuildHierarchy(configurations, null);

            return Json(hierarchyData);
        }

        private List<ConfigurationDTO> BuildHierarchy(List<Configuration> configurations, int? parentId)
        {
            return configurations
                .Where(c => c.ParentId == parentId)
                .Select(config => new ConfigurationDTO
                {
                    Id = config.Id,
                    ParentId = config.ParentId,
                    Name = config.Name,
                    Children = BuildHierarchy(configurations, config.Id)
                })
                .ToList();
        }




        [HttpPost]
        public IActionResult UploadConfiguration(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                var result = _dbContext.Configurations.ToList();
                var configurationResponses = new List<ConfigurationResponse>();

                foreach (var config in result)
                {
                    ConfigurationResponse newResponse = new ConfigurationResponse()
                    {
                        Id = config.Id,
                        Name = config.Name,
                        ParentId = config.ParentId
                    };
                    configurationResponses.Add(newResponse);
                }
                return Ok(configurationResponses);
            }

            try
            {
                using (var reader = new StreamReader(file.OpenReadStream()))
                {
                    var jsonContent = reader.ReadToEnd();

                    var configurations = JsonConvert.DeserializeObject<Configuration>(jsonContent);

                    _dbContext.Configurations.Add(configurations);
                    _dbContext.SaveChanges();

                    var result = _dbContext.Configurations.ToList();

                    return Ok(result);
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"Error uploading configuration: {ex.Message}";
                return RedirectToAction("Index");
            }
        }
    }
}
