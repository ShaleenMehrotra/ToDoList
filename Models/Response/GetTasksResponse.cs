using Models.Request;
using System.Collections.Generic;

namespace Models.Response
{
    public class GetTasksResponse : ApiResponseBase
    {
        public List<Task> Tasks { get; set; }
        public int? TotalCount { get; set; }

    }
}
