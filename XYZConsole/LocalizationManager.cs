using System;
using System.Globalization;
using System.Reflection;
using System.Resources;
using System.Threading;

namespace XYZConsole
{
    public static class LocalizationManager
    {
        private static ResourceManager? _resourceManager;

        static LocalizationManager()
        {
            // Default to English if no specific culture is set yet.
            // The actual ResourceManager will be initialized once a culture is set.
            SetLanguage("en");
        }

        public static void SetLanguage(string cultureCode)
        {
            try
            {
                CultureInfo cultureInfo = new CultureInfo(cultureCode);
                Thread.CurrentThread.CurrentCulture = cultureInfo;
                Thread.CurrentThread.CurrentUICulture = cultureInfo;

                // The resource manager should be specific to the XYZConsole assembly
                // and the base name of the resource files.
                _resourceManager = new ResourceManager("XYZConsole.Resources.Messages", Assembly.GetExecutingAssembly());

                Console.WriteLine($"DEBUG: Language set to: {cultureInfo.DisplayName}");
                // Test string retrieval
                string? testWelcome = _resourceManager.GetString("WelcomeMessage", cultureInfo);
                if (string.IsNullOrEmpty(testWelcome))
                {
                     Console.WriteLine($"DEBUG: WelcomeMessage for {cultureCode} is null or empty. Check resx file and namespace/assembly.");
                }
                else
                {
                    Console.WriteLine($"DEBUG: Welcome message in {cultureCode}: {testWelcome}");
                }

            }
            catch (CultureNotFoundException)
            {
                Console.WriteLine($"Error: Culture '{cultureCode}' not found. Defaulting to English.");
                SetLanguage("en"); // Fallback to English
            }
            catch (Exception ex)
            {
                 Console.WriteLine($"Error initializing ResourceManager for {cultureCode}: {ex.Message}. Ensure XYZConsole.Resources.Messages.resx exists and is an embedded resource.");
                 // Fallback or throw
                 if (_resourceManager == null && cultureCode != "en") SetLanguage("en"); // Prevent infinite loop
            }
        }

        public static string GetString(string key)
        {
            if (_resourceManager == null)
            {
                // This might happen if SetLanguage failed catastrophically or was never called properly.
                Console.WriteLine("DEBUG: ResourceManager not initialized. Attempting to re-initialize with CurrentUICulture.");
                // Attempt to initialize with current UI culture as a fallback.
                // This is a defensive measure.
                _resourceManager = new ResourceManager("XYZConsole.Resources.Messages", Assembly.GetExecutingAssembly());
            }

            string? value = _resourceManager.GetString(key, Thread.CurrentThread.CurrentUICulture);
            if (value == null)
            {
                Console.WriteLine($"DEBUG: String for key '{key}' not found in culture '{Thread.CurrentThread.CurrentUICulture.Name}'. Returning key itself or empty.");
                return key; // Fallback to key name if string is not found
            }
            return value;
        }
    }
}
