using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Hosting;
using System.Web.Http;
using TaskListModels;

namespace TaskListApi.Controllers
{
    public class TaskApiController : ApiController
    {

        private string GetFilePath()
        {
            return (HostingEnvironment.MapPath("~/Data/" + ConfigurationManager.AppSettings["FilePath"]));
        }

        private string GetJsonFileData()
        { 
            return File.ReadAllText(GetFilePath()); 
        }

        private List<TaskList> GetAllTasks()
        {
            return JsonConvert.DeserializeObject<List<TaskList>>(GetJsonFileData());
        }

        private void UpdateFile(string content)
        {
            File.WriteAllText(GetFilePath(), content);
        }

        [HttpPost]
        public void AddTask(TaskList task)
        {
            try
            { 
                var jsonData = GetAllTasks();
                task.TaskId = jsonData.Count() > 0 ? (jsonData.Max(x => x.TaskId) + 1) : 1;
                task.TaskCreatedDate = DateTime.Now;
                task.TaskUpdatedDate = DateTime.Now;
                jsonData.Add(task); 

                UpdateFile(Newtonsoft.Json.JsonConvert.SerializeObject(jsonData, Newtonsoft.Json.Formatting.Indented));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
         

        // GET: api/TaskApi  
        [HttpGet]
        public List<TaskList> GetTasks()
        {
            try 
            { 
                return GetAllTasks();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public void UpdateTask(TaskList updatedTask)
        { 
            try
            { 
                var jsonData = GetAllTasks();

                jsonData.RemoveAll(x => x.TaskId == updatedTask.TaskId);

                updatedTask.TaskUpdatedDate = DateTime.Now;

                jsonData.Add(updatedTask);

                UpdateFile(Newtonsoft.Json.JsonConvert.SerializeObject(jsonData, Newtonsoft.Json.Formatting.Indented));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        public void DeleteTask(int id)
        {
            try
            { 
                var tasksList = GetAllTasks().Where(l => l.TaskId != id).ToList();

                UpdateFile(Newtonsoft.Json.JsonConvert.SerializeObject(tasksList, Newtonsoft.Json.Formatting.Indented));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        public TaskList EditTask(int id)
        {
            try
            { 
                return GetAllTasks().Where(l => l.TaskId == id).FirstOrDefault(); 
            }
            catch (Exception ex)
            {
                throw ex;
            }
        } 
    }
} 