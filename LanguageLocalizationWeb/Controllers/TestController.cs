using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System.Globalization;
using System.Reflection;

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


        [HttpPost("{culture2}")]
        public IActionResult GetTranslations2(string culture2 = "zh-CN")
        {
            // Validate culture code
            if (!IsValidCulture(culture2))
            {
                return BadRequest("不支持该语言");
            }


            var moduleUI = new Dictionary<string, string>();
            var moduleLogic = new Dictionary<string, string>();

            try
            {
                // Set the current culture
                CultureInfo.CurrentUICulture = new CultureInfo(culture2);

                // Fetch all resource strings
                var translations = new Dictionary<string, string>();

                var userName = "John Doe";
                var greetingMessage = _localizer["Greeting", userName];

                var dic = _localizer.GetAllStrings(includeParentCultures: true);
                foreach (var entry in dic)
                {
                    translations.Add(entry.Name, entry.Value);
                    //if (entry.Name.StartsWith("UI_"))
                    //{
                    //    moduleUI.Add(entry.Name.Substring("UI_".Length), entry.Value);
                    //}
                    //else if (entry.Name.StartsWith("logic_"))
                    //{
                    //    moduleLogic.Add(entry.Name.Substring("logic_".Length), entry.Value);
                    //}
                }

                //var result = new Dictionary<string, Dictionary<string, string>>
                //                {
                //    { "moduleUI", moduleUI },
                //    { "moduleLogic", moduleLogic }
                //};
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
