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
    public class CreditNoteCreationController : ApiController
    {
        BusinessLogic BL = new BusinessLogic();

        [HttpPost]
        public ResponseModel CreditNoteCreation([FromBody] CreditNotePosting model)
        {
            return this.BL.CreditNote(model);
        }
    }
}
