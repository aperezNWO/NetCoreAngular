using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using NetCoreAngular.Models;
using Newtonsoft.Json;

namespace NetCoreAngular.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AccessLogController : ControllerBase
    {
        #region "Campos"
        //
        private readonly IConfiguration               _config;
        #endregion

        #region "Constructor"
        //
        public AccessLogController(IConfiguration configuration)
        {
            _config = configuration;
        }
        #endregion

        #region "Metodos"
        //
        [HttpGet]
        public string Get()
        {
            //
            string connectionString = _config.GetConnectionString("mcsdexnacato");

            //
            List<AccessLogEntity> accessLogCollection =  new LogModel(connectionString).GetAccessLog();
            string jsonString                          = JsonConvert.SerializeObject(accessLogCollection, Formatting.Indented);

            //
            return jsonString;
        }
        #endregion
    }
}
