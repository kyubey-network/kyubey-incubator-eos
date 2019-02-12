using Andoromeda.Framework.EosNode;
using Andoromeda.Framework.GitHub;
using Andoromeda.Framework.Logger;
using Andoromeda.Kyubey.Incubator.Lib;
using Andoromeda.Kyubey.Incubator.Models;
using Andoromeda.Kyubey.Incubator.Repository;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.NodeServices;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Andoromeda.Kyubey.Incubator.Repository
{
    public class TokenInfoRespository
    {
        private string tokenFolderAbsolutePath;
        private const string manifestFileName = "manifest.json";
        private const string iconFileName = "icon.png";
        private IServiceProvider _provider;
        private string _lang;

        public TokenInfoRespository(string path, string lang, IServiceProvider provider)
        {
            _lang = lang;
            tokenFolderAbsolutePath = path;
            _provider = provider;
        }

        public IEnumerable<TokenManifest> EnumerateAll()
        {
            var tokenFolderDirectories = Directory.GetDirectories(tokenFolderAbsolutePath);
            var result = new List<TokenManifest>();
            foreach (var t in tokenFolderDirectories)
            {
                var item = GetSingle(t);
                if (item != null)
                    result.Add(item);
            }
            return result;
        }

        public TokenManifest GetSingle(object id)
        {
            var filePath = Path.Combine(tokenFolderAbsolutePath, id.ToString(), manifestFileName);
            if (File.Exists(filePath))
            {
                var fileContent = System.IO.File.ReadAllText(filePath);
                return JsonConvert.DeserializeObject<TokenManifest>(fileContent);
            }
            return null;
        }

        public string GetTokenIconPath(string tokenId)
        {
            var absolutePath = Path.Combine(tokenFolderAbsolutePath, tokenId, iconFileName);
            if (File.Exists(absolutePath))
            {
                return absolutePath;
            }
            return null;
        }

        public string GetPriceJsText(string tokenId)
        {
            var filePath = Path.Combine(tokenFolderAbsolutePath, tokenId, "contract_exchange", "price.js");
            if (File.Exists(filePath))
            {
                return File.ReadAllText(filePath);
            }
            return null;
        }

        public string GetTokenDescription(string tokenId, string cultureStr)
        {
            var folderPath = Path.Combine(tokenFolderAbsolutePath, tokenId, "incubator");
            if (!Directory.Exists(folderPath))
            {
                return null;
            }

            var availableFilePaths = GlobalizationFileFinder.GetCultureFiles(folderPath, cultureStr, ".txt").Where(x => Path.GetFileNameWithoutExtension(x).StartsWith("description."));

            var availablePath = availableFilePaths.FirstOrDefault();
            if (availablePath != null)
                return File.ReadAllText(availablePath);
            return null;
        }

        public string GetTokenIncubationDetail(string tokenId, string cultureStr)
        {
            var folderPath = Path.Combine(tokenFolderAbsolutePath, tokenId, "incubator");
            if (!Directory.Exists(folderPath))
            {
                return null;
            }

            var availablePath = GlobalizationFileFinder.GetCultureFiles(folderPath, cultureStr, ".md").Where(x => Path.GetFileNameWithoutExtension(x).StartsWith("detail.")).FirstOrDefault();
            if (availablePath != null)
                return File.ReadAllText(availablePath);
            return null;
        }

        public IEnumerable<string> GetTokenIncubationBannereRelativePaths(string tokenId, string cultureStr)
        {
            var sliderFolderPath = Path.Combine(tokenFolderAbsolutePath, tokenId, "slides");
            if (!Directory.Exists(sliderFolderPath))
            {
                yield return null;
            }

            var availablePaths = GlobalizationFileFinder.GetCultureFiles(sliderFolderPath, cultureStr, ".png").ToList();

            foreach (var path in availablePaths)
            {
                Uri absolutePath = new Uri(path);
                Uri folderPath = new Uri(tokenFolderAbsolutePath + "/");
                string relativePath = folderPath.MakeRelativeUri(absolutePath).ToString();
                yield return relativePath;
            };
        }

        public string GetWhitePaper(string tokenId, string cultureStr)
        {
            var sliderFolderPath = Path.Combine(tokenFolderAbsolutePath, tokenId, "whitepaper");
            if (!Directory.Exists(sliderFolderPath))
            {
                return null;
            }

            var availablePaths = GlobalizationFileFinder.GetCultureFiles(sliderFolderPath, cultureStr, ".pdf").ToList();

            if (availablePaths.Count >= 1)
            {
                var path = availablePaths.FirstOrDefault();
                Uri absolutePath = new Uri(path);
                Uri folderPath = new Uri(tokenFolderAbsolutePath + "/");
                string relativePath = folderPath.MakeRelativeUri(absolutePath).ToString();
                return relativePath;
            };
            return null;
        }

        public List<TokenIncubatorUpdateModel> GetTokenIncubatorUpdates(string tokenId, string cultureStr)
        {
            var folderPath = Path.Combine(tokenFolderAbsolutePath, tokenId, "updates");
            if (!Directory.Exists(folderPath))
            {
                return null;
            }

            var availableFiles = GlobalizationFileFinder.GetCultureFiles(folderPath, cultureStr, ".json").ToList();
            var mainFile = availableFiles.FirstOrDefault(x => x.EndsWith(".json"));

            if (!string.IsNullOrWhiteSpace(mainFile))
            {
                var updateList = JsonConvert.DeserializeObject<List<TokenIncubatorUpdateModel>>(System.IO.File.ReadAllText(mainFile));
                if (updateList != null)
                {
                    foreach (var u in updateList)
                    {
                        var contentPath = Path.Combine(folderPath, u.Content);
                        if (!string.IsNullOrWhiteSpace(u.Content) && File.Exists(contentPath))
                        {
                            u.Content = System.IO.File.ReadAllText(contentPath);
                        }
                    }
                }
                return updateList;
            }
            return null;
        }

        public async Task<TokenContractPriceModel> GetContractPriceAsync(string id)
        {
            var logger = (ILogger)_provider.GetService(typeof(ILogger));

            try
            {
                var tokenInfo = GetSingle(id);
                var currentPriceJavascript = GetPriceJsText(id);

                var nodeService = (INodeServices)_provider.GetService(typeof(INodeServices));
                var nodeApiService = (NodeApiInvoker)_provider.GetService(typeof(NodeApiInvoker));

                var paramRows = new object[] { };

                if (!string.IsNullOrWhiteSpace(tokenInfo.Basic.Contract.Pricing)
                    && !string.IsNullOrWhiteSpace(tokenInfo.Basic.Price_Scope)
                    && !string.IsNullOrWhiteSpace(tokenInfo.Basic.Price_Table))
                {
                    try
                    {
                        paramRows = (await nodeApiService.GetTableRowsAsync<IDictionary<string, object>>
                            (tokenInfo.Basic.Contract.Pricing, tokenInfo.Basic.Price_Table, tokenInfo.Basic.Price_Scope)).rows.ToArray();
                    }
                    catch (Exception ex)
                    {
                        logger.LogError(ex.ToString());
                    }
                }

                var buyPrice = await nodeService.InvokeExportAsync<decimal>("./price", "buyPrice", paramRows, currentPriceJavascript);
                buyPrice = decimal.Round(buyPrice, 8, MidpointRounding.AwayFromZero);

                var sellPrice = await nodeService.InvokeExportAsync<decimal>("./price", "sellPrice", paramRows, currentPriceJavascript);
                sellPrice = decimal.Round(sellPrice, 8, MidpointRounding.AwayFromZero);

                return new TokenContractPriceModel
                {
                    BuyPrice = buyPrice,
                    SellPrice = sellPrice
                };
            }
            catch (Exception ex)
            {
                logger.LogError(ex.ToString());
                return new TokenContractPriceModel
                {
                    BuyPrice = 0,
                    SellPrice = 0
                };
            }
        }
    }
}

public class TokenRepositoryFactory
{
    private IConfiguration _config;
    private IHostingEnvironment _hostingEnv;
    private IServiceProvider _provider;

    public TokenRepositoryFactory(IConfiguration config, IHostingEnvironment hostingEnv, IServiceProvider provider)
    {
        _config = config;
        _hostingEnv = hostingEnv;
        _provider = provider;
    }

    public async Task<TokenInfoRespository> CreateAsync(string lang)
    {
        var path = Path.Combine(_hostingEnv.ContentRootPath, _config["RepositoryStore"], "token-list");
        if (!Directory.Exists(path))
        {
            await GitHubSynchronizer.CreateOrUpdateRepositoryAsync(
"kyubey-network", "token-list", "master",
Path.Combine(_config["RepositoryStore"], "token-list"));
        }
        return new TokenInfoRespository(path, lang, _provider);
    }

    public TokenInfoRespository Create(string lang)
    {
        return CreateAsync(lang).Result;
    }
}

namespace Microsoft.Extensions.DependencyInjection
{
    public static class TokenRepositoryExtensions
    {
        public static IServiceCollection AddTokenRepositoryactory(this IServiceCollection self)
        {
            return self.AddSingleton<TokenRepositoryFactory>();
        }
    }
}
