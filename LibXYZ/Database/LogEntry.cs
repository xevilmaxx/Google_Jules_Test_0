using System;
using System.ComponentModel.DataAnnotations; // For [Key]

namespace LibXYZ.Database
{
    public class LogEntry
    {
        [Key] // Explicitly define Id as Primary Key
        public int Id { get; set; }
        public DateTime Timestamp { get; set; }

        public string Message { get; set; } = string.Empty;

        public int RandomValue { get; set; }
    }
}
