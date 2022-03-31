using System;
using System.Diagnostics.CodeAnalysis;

namespace Models.Request
{
    [ExcludeFromCodeCoverage]
    public class Task
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public DateTime LastUpdatedDate { get; set; }
    }
}
