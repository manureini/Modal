using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blazored.Modal.Configuration
{
    public class Settings
    {
        public Settings()
        {
            
        }

        /// <summary>
        /// Gets or sets the default JavaScript path where to locate BlazoredModal.razor.js, without including the name of the file.
        /// </summary>
        public string? JsPath { get; set; }
    }
}
