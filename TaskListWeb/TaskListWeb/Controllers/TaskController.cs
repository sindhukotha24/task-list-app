using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TaskListModels;

namespace TaskListWeb.Controllers
{
    public class TaskController : Controller
    {
        // GET: Task
        public async Task<ActionResult> Index() 
        {
            List<TaskList> taskList = new List<TaskList>();

            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri(ConfigurationManager.AppSettings["WebApiUrl"]);

                HttpResponseMessage response = await httpClient.GetAsync("/api/TaskApi/GetTasks");

                var contents = await response.Content.ReadAsStringAsync();

                taskList = JsonConvert.DeserializeObject<List<TaskList>>(contents).ToList();
            }

            return View(taskList);
        }


        // GET: Task/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Task/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Task/Create
        [HttpPost]
        public async Task<ActionResult> Create(TaskList task)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    using (HttpClient httpClient = new HttpClient())
                    {
                        httpClient.BaseAddress = new Uri(ConfigurationManager.AppSettings["WebApiUrl"]);
                        var result = await httpClient.PostAsJsonAsync<TaskList>("/api/TaskApi/AddTask", task);
                        string resultContent = await result.Content.ReadAsStringAsync();
                        return RedirectToAction("Index");
                    }
                }
                else
                {
                    return Create();
                }
            }
            catch (Exception ex)
            {
                return Json(new { errors = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        // GET: Task/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            TaskList task = new TaskList();

            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri(ConfigurationManager.AppSettings["WebApiUrl"]);

                HttpResponseMessage response = await httpClient.GetAsync("/api/TaskApi/EditTask?id=" + id);

                var contents = await response.Content.ReadAsStringAsync();

                task = JsonConvert.DeserializeObject<TaskList>(contents);
            }

            return View(task);
        }

        [HttpPost]
        public async Task<ActionResult> Update(TaskList task)
        {
            if (ModelState.IsValid)
            {
                using (HttpClient httpClient = new HttpClient())
                {
                    httpClient.BaseAddress = new Uri(ConfigurationManager.AppSettings["WebApiUrl"]);
                    var result = await httpClient.PostAsJsonAsync<TaskList>("/api/TaskApi/UpdateTask", task);
                    string resultContent = await result.Content.ReadAsStringAsync();
                    return RedirectToAction("Index");
                }
            }
            else
            {
                return Create();
            }
        }

        // POST: Task/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here 
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Task/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            List<TaskList> taskList = new List<TaskList>();

            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri(ConfigurationManager.AppSettings["WebApiUrl"]);

                HttpResponseMessage response = await httpClient.GetAsync("/api/TaskApi/DeleteTask?id=" + id);

                var contents = await response.Content.ReadAsStringAsync();

                return Json(contents, JsonRequestBehavior.AllowGet); 
            }  
        }

        // POST: Task/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // Public: Serializes the Key/Value pairs in ModelState errors and returns a Json object
        public JsonResult ModelStateErrorSerializer(ModelStateDictionary modelState)
        {
            // Initialize Dictionary for holding ModelState errors then loop through and add
            Dictionary<string, string> errors = new Dictionary<string, string>();

            foreach (var item in modelState)
            {
                foreach (var error in item.Value.Errors)
                {
                    errors.Add(item.Key, error.ErrorMessage);
                }
            }
            // Return the serialized object
            return Json(errors, JsonRequestBehavior.AllowGet);
        }
    }
}
