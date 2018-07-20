using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskListModels
{
    public class TaskList
    {
        [Required]
        [System.ComponentModel.DisplayName("Task Id")]
        public int TaskId { get; set; }

        [Required]
        [System.ComponentModel.DisplayName("Task Name")]
        public string TaskName { get; set; }

        [Required]
        [System.ComponentModel.DisplayName("Task Requester")]
        public string TaskRequester { get; set; }

        [Required]
        [System.ComponentModel.DisplayName("Task Assignee")]
        public string TaskAssignee { get; set; }

        public DateTime TaskCreatedDate { get; set; }
        public DateTime TaskUpdatedDate { get; set; }
    }
}
