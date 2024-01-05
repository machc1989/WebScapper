
using System.Xml;
using System;
using WebScrapperAPI.Models;
using System.Net;
using HtmlAgilityPack;
using WebScrapperAPI.Repository;

namespace WebScrapperAPI
{
    public class BackGroundScrapper : BackgroundService
    {
        private readonly IWebScrapperRepository _repository;
        private readonly HttpClient _httpClient;

        public BackGroundScrapper(IWebScrapperRepository repository, HttpClient httpClient)
        {
            _repository = repository;
            _httpClient = httpClient;
        }

        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await ScrapeAndProcess();

                await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);
            }
        }

        private async Task ScrapeAndProcess()
        {
            try
            {
                var pages = await _repository.GetPagesForProcess();

                foreach (var page in pages)
                {
                    string htmlContent = await _httpClient.GetStringAsync(page.Url);

                    HtmlDocument htmlDocument = new HtmlDocument();
                    htmlDocument.LoadHtml(htmlContent);

                    string xpathExpression = "//a[@href]";
                    HtmlNodeCollection anchorNodes = htmlDocument.DocumentNode.SelectNodes(xpathExpression);

                    if (anchorNodes != null)
                    {
                        foreach (HtmlNode anchorNode in anchorNodes)
                        {
                            string linkText = anchorNode.InnerText.Trim();
                            string linkUrl = anchorNode.GetAttributeValue("href", "");

                            var link = new Link()
                            {
                                LinkName = linkText,
                                Url = linkUrl
                            };

                            page.Links.Add(link);
                        }
                        page.LinkCount = page.Links.Count;
                    }
                    else
                    {
                        page.LinkCount = 0;
                    }

                    page.Processed = true;

                    await _repository.UpdatePage(page);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}
