using System;
using System.Collections.Generic;
using System.Linq; // For ToList()

namespace LibXYZ.Database
{
    public class LogService
    {
        // In a larger application, DbContext might be injected.
        // For this console app, LogService will manage its own DbContext instance.
        // private readonly AppDbContext _context; // If injected

        public LogService()
        {
            // _context = context; // If injected
            // EnsureCreated is called by AppDbContext constructor
        }

        public List<LogEntry> GetAllLogEntries()
        {
            using (var context = new AppDbContext()) // Create and dispose context for each operation
            {
                return context.LogEntries.OrderByDescending(e => e.Timestamp).ToList();
            }
        }

        public void AddRandomLogEntry()
        {
            using (var context = new AppDbContext())
            {
                var random = new Random();
                var newEntry = new LogEntry
                {
                    Timestamp = DateTime.UtcNow,
                    Message = "Random Log Event #" + random.Next(1, 10001),
                    RandomValue = random.Next(-10000, 10001)
                };

                context.LogEntries.Add(newEntry);
                context.SaveChanges();
            }
        }

        // Optional: Method to ensure DB is created, though AppDbContext constructor does this.
        // Could be useful if constructor logic changes or for explicit calls.
        public void EnsureDatabaseReady()
        {
            using (var context = new AppDbContext())
            {
                // Database.EnsureCreated() is already called in AppDbContext constructor.
                // This method can be a no-op or provide additional checks if needed.
                // For instance, logging that the service is ready.
                Console.WriteLine("LogService: Database is ready (ensured by AppDbContext).");
            }
        }
    }
}
