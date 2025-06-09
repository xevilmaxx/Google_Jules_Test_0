using Microsoft.EntityFrameworkCore;
using System;
using System.IO; // For Path.Join
// using NLog; // Removed direct NLog dependency from DbContext for this version

namespace LibXYZ.Database
{
    public class AppDbContext : DbContext
    {
        // It's good practice to make DbSet properties non-nullable if they are always initialized.
        // EF Core initializes them.
        public DbSet<LogEntry> LogEntries { get; set; } = null!; // Initialize with null! to satisfy compiler

        public string DbPath { get; }

        public AppDbContext()
        {
            var folder = AppContext.BaseDirectory;
            DbPath = Path.Join(folder, "appdata.db");

            // Console.WriteLine($"DEBUG: Database path: {DbPath}"); // For quick debug during dev

            try
            {
                // Database.EnsureCreated() is called here.
                // It's simple but less flexible than migrations for schema evolution.
                // For production apps, migrations are recommended.
                Database.EnsureCreated();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error ensuring database is created: {ex.Message}");
                // Consider more robust error handling or logging here
            }
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            if (!options.IsConfigured)
            {
                options.UseSqlite($"Data Source={DbPath}");
                // To enable NLog for EF Core (example, requires NLog package and configuration):
                // options.LogTo(
                //    (eventId, logLevel) => logLevel >= LogLevel.Information, // Filter what to log
                //    eventData => NLog.LogManager.GetLogger("Microsoft.EntityFrameworkCore.Database.Command")
                //                            .Log(NLog.LogLevel.From ನಮ್ಮsystemLogLevel(eventData.LogLevel), eventData.ToString())
                // );
                // options.EnableSensitiveDataLogging(); // If you need to see parameter values
            }
        }
        // Helper for NLog mapping (if used above)
        // private static NLog.LogLevel From ನಮ್ಮsystemLogLevel(Microsoft.Extensions.Logging.LogLevel logLevel)
        // {
        //     switch (logLevel)
        //     {
        //         case Microsoft.Extensions.Logging.LogLevel.Trace: return NLog.LogLevel.Trace;
        //         case Microsoft.Extensions.Logging.LogLevel.Debug: return NLog.LogLevel.Debug;
        //         case Microsoft.Extensions.Logging.LogLevel.Information: return NLog.LogLevel.Info;
        //         case Microsoft.Extensions.Logging.LogLevel.Warning: return NLog.LogLevel.Warn;
        //         case Microsoft.Extensions.Logging.LogLevel.Error: return NLog.LogLevel.Error;
        //         case Microsoft.Extensions.Logging.LogLevel.Critical: return NLog.LogLevel.Fatal;
        //         default: return NLog.LogLevel.Off;
        //     }
        // }
    }
}
