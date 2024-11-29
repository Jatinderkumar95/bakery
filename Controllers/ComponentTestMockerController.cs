using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Dynamic;
using System.Reflection;
using System.Text;

namespace bakery.Controllers
{
    [Route("api/[controller]/{componentName}")]
    [ApiController]
    public class ComponentTestMockerController : Controller
    {
        public ComponentTestMockerController()
        {
            
        }
        public async Task<IActionResult> ApplyAsync([FromRoute] string componentName)
        {
            var body = await GetRequestBody();
            if(string.IsNullOrEmpty(body)) return ViewComponent(componentName);

            var bodyData = JsonConvert.DeserializeObject<Dictionary<string, object>>(body);
            var componentType = GetComponentType(componentName);

            if (componentType != null) return BadRequest();
            
            var invokeMethodInfo = GetInvokeMethodInfo(componentType);

            if (invokeMethodInfo != null) return BadRequest();

            var arguments =BuildInvokeArguments(invokeMethodInfo, bodyData);

            return ViewComponent(componentName,arguments);
        }

        private static IDictionary<string, object> BuildInvokeArguments(MethodInfo? invokeMethodInfo, Dictionary<string, object> inputObject)
        {
            var parameters = invokeMethodInfo.GetParameters();

            var componentData = new ExpandoObject() as IDictionary<string, object>;
            foreach (var parameter in parameters)
            {
                if (!inputObject.ContainsKey(parameter.Name)) continue;

                if (parameter.ParameterType.IsEnum && inputObject[parameter.Name] is string inputValue)
                    componentData.Add(parameter.Name, Enum.Parse(parameter.ParameterType, inputValue));

                else if (parameter.ParameterType == typeof(decimal) || parameter.ParameterType == typeof(double) || parameter.ParameterType == typeof(int) || parameter.ParameterType == typeof(long))
                    componentData.Add(parameter.Name, JsonConvert.DeserializeObject(inputObject[parameter.Name].ToString(), parameter.ParameterType));

                else if (parameter.ParameterType == typeof(string) || parameter.ParameterType.IsValueType)
                    componentData.Add(parameter.Name, inputObject[parameter.Name]);

                else
                    componentData.Add(parameter.Name,
                        JsonConvert.DeserializeObject(inputObject[parameter.Name].ToString(), parameter.ParameterType));
            }

            return componentData;
        }

        private static Type GetComponentType(string componentName)
        {
          return  Assembly.GetEntryAssembly().GetTypes().FirstOrDefault(t => t.Name == componentName && t.GetTypeInfo().BaseType == typeof(ViewComponent));
        }

        private MethodInfo GetInvokeMethodInfo(Type componentType)
        {
           return componentType.GetMethods().FirstOrDefault(x => x.Name == "Invoke" || x.Name == "InvokeAsync");
        }

        private async Task<string> GetRequestBody()
        {
            using (var memoryStream = new MemoryStream())
            {
                await Request.Body.CopyToAsync(memoryStream);
               return Encoding.UTF8.GetString(memoryStream.ToArray());
            }
        }
    }
}
