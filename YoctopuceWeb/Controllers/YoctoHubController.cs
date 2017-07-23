using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace YoctopuceWeb.Controllers
{
    public class YoctoHubController : ApiController
    {
        // GET: api/YoctoHub
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/YoctoHub/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/YoctoHub
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/YoctoHub/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/YoctoHub/5
        public void Delete(int id)
        {
        }
    }
}
