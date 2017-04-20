using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Portal.Models;
namespace Portal.Helper
{
    public class DbContextHelper // her istekte bir adet portal entity oluşturuyor.
    {
        private const string contextKey = "MyContext";

        public PortalEntities GetDb
        {
           get
            {
                if (HttpContext.Current.Items[contextKey] == null)
                {
                    HttpContext.Current.Items.Add(contextKey, new PortalEntities());
                }

                return (PortalEntities)HttpContext.Current.Items[contextKey];
            }
           
        }

        public void DisposeContext()
        {
            if (HttpContext.Current.Items[contextKey] != null)
            {
                var context = (PortalEntities)HttpContext.Current.Items[contextKey];
                context.Dispose();
            }
        }
    }
}