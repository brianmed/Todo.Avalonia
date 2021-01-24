using System.IO;
using System.Collections.Generic;

namespace Todo.AppCtx
{
    public class DirectoriesCtx
    {
        public static void Provision()
        {
            if (!IsProvisioned()) {
                List<string> directories = new List<string>();

                directories.Add(Program.ConfigCtx.BaseDirectory);
                directories.Add(Program.ConfigCtx.AppDbDirectory);
                directories.Add(Program.ConfigCtx.LogDirectory);

                foreach (string directory in directories)
                {
                    if (!Directory.Exists(directory)) {
                        Directory.CreateDirectory(directory);
                    }
                }
            }
        }

        private static bool IsProvisioned()
        {
            return Directory.Exists(Program.ConfigCtx.BaseDirectory)
                && Directory.Exists(Program.ConfigCtx.AppDbDirectory)
                && Directory.Exists(Program.ConfigCtx.LogDirectory);
        }
    }
}