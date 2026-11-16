using DemoProject.Models.BusinessLayer;
using DemoProject.Models.DomainClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace DemoApi.Controllers
{
    [Authorize]
    public class OutGoingPaymentController : ApiController
    {
        BusinessLogic BL = new BusinessLogic();
        [HttpPost]
        public ResponseModel OutGoingPaymentPosting([FromBody] OutGoingPayment model)
        {
            return this.BL.OGPPosting(model);
        }
    }
}
