using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectTools.Core.Internal.Implementations.DotSln
{
    public class DotSlnTemplater : AbstractTemplater
    {
        public override bool DirectoryValidForTemplater(string directory)
        {
            foreach (var file in Directory.GetFiles(directory))
            {
                if (file.EndsWith(".sln"))
                {
                    return true;
                }
            }

            foreach (var dir in Directory.GetDirectories(directory))
            {
                var result = DirectoryValidForTemplater(dir);
                if (result)
                {
                    return true;
                }
            }

            return false;
        }

        public override string Prepare(PrepareOptions options, Func<string, bool> log)
        {
            throw new NotImplementedException();
        }
    }
}
