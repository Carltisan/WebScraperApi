using System;
using System.Collections.Generic;
using System.Text;
using AngleSharp;
using Microsoft.Extensions.DependencyInjection;

namespace WebScraperApiApp.Scraper
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddAngleSharp(this IServiceCollection services) =>
            services.AddSingleton(BrowsingContext.New(AngleSharp.Configuration.Default.WithDefaultLoader()));
    }
}
