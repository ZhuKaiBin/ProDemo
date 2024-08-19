using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System.Globalization;

namespace LanguageLocalizationWeb.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class TestController : ControllerBase
    {

        private readonly IStringLocalizer _localizer;

        public TestController(IStringLocalizerFactory factory)
        {
            var type = typeof(Resources); // Replace Resources with your resource class name
            _localizer = factory.Create("Resources", type.Assembly.GetName().Name);
        }

        [HttpGet("{culture}")]
        public IActionResult GetTranslations(string culture)
        {
            // Validate culture code
            if (!IsValidCulture(culture))
            {
                return BadRequest("不支持该语言");
            }

            try
            {
                // Set the current culture
                CultureInfo.CurrentUICulture = new CultureInfo(culture);

                // Fetch all resource strings
                var translations = new Dictionary<string, string>();
                foreach (var entry in _localizer.GetAllStrings(includeParentCultures: true))
                {
                    translations.Add(entry.Name, entry.Value);
                }

                // Check if translations were found
                if (translations.Count == 0)
                {
                    return NotFound("No translations found for the specified culture.");
                }

                return Ok(translations);
            }
            catch (CultureNotFoundException)
            {
                return BadRequest("不支持该语言");
            }
        }

        // Method to check if the culture is valid
        private bool IsValidCulture(string culture)
        {
            try
            {
                // Try to create a CultureInfo object to validate the culture
                CultureInfo.GetCultureInfo(culture);
                return true;
            }
            catch (CultureNotFoundException)
            {
                return false;
            }
        }


        public class Resources
        { }
    }
}
