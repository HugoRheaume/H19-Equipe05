using NetflixPrjeq05.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NetflixPrjeq05.Service
{
    public class BaseService
    {
        protected Entities db;

        public BaseService(Entities db)
        {
            this.db = db;
        }

        public void Dispose()
        {
            db.Dispose();
        }




    }
}