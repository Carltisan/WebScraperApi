using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AngleSharp;
using AngleSharp.Dom;
using Microsoft.Extensions.DependencyInjection;
using WebScraperApiApp.Common.Models;
using static WebScraperApiApp.Common.Constants;

namespace WebScraperApiApp.Scraper
{
    
    public class HzzoHtmlScraper
    {

        static readonly DateTime filterDtParsable2013 = new DateTime(2013, 6, 13);

        readonly IBrowsingContext _browsingContext;


        public HzzoHtmlScraper(IBrowsingContext browsingContext)
        {
            this._browsingContext = browsingContext;
        }

        public async Task<ISet<HzzoMedsDownloadDto>> Run()
        {
            var htmlDocs = await DownloadHtmlDocuments();
            var parsedDocs = ParseHtmlDocuments(htmlDocs);

            return parsedDocs;
        }

        ISet<HzzoMedsDownloadDto> ParseHtmlDocuments(IDocument[] docs) =>
            docs.Aggregate(
                new HashSet<HzzoMedsDownloadDto>(),
                (docList, doc) => new HashSet<HzzoMedsDownloadDto>(docList.Concat(ParseHtmlDocument(doc)))
                );

        static ISet<HzzoMedsDownloadDto> ParseMedsLiElements(IEnumerable<IElement> elems) =>
            elems.Aggregate(new HashSet<HzzoMedsDownloadDto>(), (medsList, li) =>
            {
                var href = li.QuerySelector("a").GetAttribute("href");

                // NOTE: this domain is not available, links don't work
                if (!href.Contains("cdn.hzzo.hr"))
                {
                    var downloadDto = new HzzoMedsDownloadDto(
                        href, li.TextContent.TrimEnd().Split(' ').LastOrDefault(),
                        DOWNLOAD_DIR);

                    if (downloadDto.ValidFrom > filterDtParsable2013)
                        medsList.Add(downloadDto);
                }

                return medsList;
            });

        static ISet<HzzoMedsDownloadDto> ParseHtmlDocument(IDocument doc) =>
            ParseMedsLiElements(SelectLiElements(doc));

        static IEnumerable<IElement> SelectLiElements(IDocument doc) =>
            doc.QuerySelectorAll("section#main > ul li").Where(_predicateForListLiMeds);

        static Func<IElement, bool> _predicateForListLiMeds =
            x =>
            // primary list:
            x.TextContent.Contains("Osnovna lista lijekova") ||
            // supplementary list:
            x.TextContent.Contains("Dopunska lista lijekova");

        Task<IDocument[]> DownloadHtmlDocuments() =>
            Task.WhenAll(
                _browsingContext.OpenAsync(CURRENT_LISTS_URL),
                _browsingContext.OpenAsync(ARCHIVE_LISTS_URL)
                );
    }
    
}
