﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using lnhpdWebApi.Models;

namespace lnhpdWebApi.Controllers
{
    public class ProductRouteController : ApiController
    {
        static readonly IProductDoseRepository databasePlaceholder = new ProductDoseRepository();

        public IEnumerable<ProductDose> GetAllProductDose()
        {

            return databasePlaceholder.GetAll();
        }


        public ProductDose GetProductDoseByID(int id)
        {
            ProductDose dose = databasePlaceholder.Get(id);
            if (dose == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            return dose;
        }
    }
}