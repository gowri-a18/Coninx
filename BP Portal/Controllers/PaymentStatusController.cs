using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DemoProject.Models.BusinessLayer;
using DemoProject.Models.DomainClasses;

namespace POS.Controllers
{
    public class PaymentStatusController : ApiController
    {
        BusinessLogic BL = new BusinessLogic();

        [HttpGet]
        public ResponseModel getPaymentStatus([FromBody] PaymentStatus model)
        {
            return this.BL.getPaymentStatus(model);
        }
    }
}
