using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
namespace Portal.Hubs
{
    public class DomainIsHub:Hub
    {
        public void GonderYorumEklendi(int indexDomainIs,string jsnYorum, string fromId, string toId)
        {            
            Clients.All.yorumEklendi(indexDomainIs, jsnYorum, fromId, toId);
        }
        public void GonderDurumDegisti(string jsnDomainIs,string fromId)
        {
            Clients.All.domaisDurumDegisti(jsnDomainIs,fromId);
        }
        public void GonderSayfayiYenidenYukle()
        {
            Clients.All.domainDurumDegisti();
        }
    }
}