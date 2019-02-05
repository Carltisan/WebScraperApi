using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebScraperApiApp.Common.Models
{
    public class HzzoMedsDownloadDto
    {
        private readonly string _rootLocation;

        public string Href { get; internal set; }

        public DateTime ValidFrom { get; private set; }


        public HzzoMedsDownloadDto(string href, string validFrom, string rootLocation)
        {
            this.Href = href;
            this.ValidFrom = DateTime.Parse(validFrom);
            this._rootLocation = rootLocation;
        }

        public string FileName =>
            ValidFrom.ToString("yyy-MM-dd") +
            (Href.Split('/').LastOrDefault() ?? Href.Replace("/", "_").Replace(":", "_")).TrimEnd();
    }
}
