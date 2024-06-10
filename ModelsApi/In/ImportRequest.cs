using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelsApi.In
{
    public class ImportRequest
    {
        public string ImporterName { get; set; }
        public string FileName { get; set; } //JSON file name
    }
}
