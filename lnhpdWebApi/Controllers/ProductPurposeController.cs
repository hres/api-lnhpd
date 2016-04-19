﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using lnhpdWebApi.Models;

namespace lnhpdWebApi.Controllers
{
    public class ProductPurposeController : ApiController
    {
        static readonly IProductPurposeRepository databasePlaceholder = new ProductPurposeRepository();

        public IEnumerable<ProductPurpose> GetAllProductPurpose()
        {

            return databasePlaceholder.GetAll();
        }


        public ProductPurpose GetProductPurposeByID(int id)
        {
            ProductPurpose purpose = databasePlaceholder.Get(id);
            if (purpose == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            return purpose;
        }
    }
}