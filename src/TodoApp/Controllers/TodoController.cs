using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TodoApp.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.Net.Http.Headers;
using System.Net.Http;
using Newtonsoft.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace TodoApp.Controllers
{
    public class TodoController : Controller
    {
        private readonly IOptions<AppSettings> _appSettingsAccessor;

        public TodoController(IOptions<AppSettings> appSettingsAccessor)
        {
            _appSettingsAccessor = appSettingsAccessor;
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            var id_token = User.Claims.FirstOrDefault(c => c.Type == "id_token")?.Value;
            List<TodoItem> todo = new List<TodoItem>();
            
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(_appSettingsAccessor.Value.TodoApiEndpoint);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Add("authorization", $"Bearer {id_token}");

            try
            {
                HttpResponseMessage response = await client.GetAsync("api/todo");
                if (response.IsSuccessStatusCode)
                {
                    var t = await response.Content.ReadAsStringAsync();
                    todo = JsonConvert.DeserializeObject<List<TodoItem>>(t);
                }
            }
            catch (Exception ex)
            {
                var log = new TodoItem();
                log.Name = ex.Message.ToString();
                log.IsComplete = false;

                HttpResponseMessage response = await client.PostAsJsonAsync("api/log", log);
            }            
            
            return View(todo);
        }
    }
}
