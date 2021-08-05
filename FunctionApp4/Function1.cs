using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace FunctionApp4
{
    public class Function1
    {
        private readonly AppDbContext dbContext;

        public Function1(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [FunctionName("Function1")]
        public async Task<ActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequestData req,
            FunctionContext executionContext)
        {
            var pcs = dbContext.PCs.ToList();

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            PC pc = JsonConvert.DeserializeObject<PC>(requestBody);

            var result = pcs.FirstOrDefault(i => i.PCName == pc.PCName);

            if (result != null)
            {
                if(pc.PCName == result.PCName 
                    && pc.PCNetVersion == result.PCNetVersion 
                    && pc.PCOSName == result.PCOSName
                    && pc.PCTimeZone == result.PCTimeZone)
                {
                    return new OkResult();
                }
                else
                {
                    dbContext.PCs.Attach(result);
                    result.PCName = pc.PCName;
                    result.PCNetVersion = pc.PCNetVersion;
                    result.PCOSName = pc.PCOSName;
                    result.PCTimeZone = pc.PCTimeZone;

                    dbContext.SaveChanges();
                    return new OkResult();
                }
            }
            else
            {
                dbContext.PCs.Add(pc);
                dbContext.SaveChanges();
                return new OkResult();
            }
        }
    }
}
