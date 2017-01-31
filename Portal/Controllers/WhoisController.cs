using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Xml;
using Portal.Models;

namespace Portal.Controllers
{
    public class WhoisController : BaseController
    {
       

        public JsonResult IPAdresi(int domainID)
        {
            Domain domainim = Db.Domains.FirstOrDefault(a => a.DomainID == domainID);
            string stackoverflow = domainim.DomainAdi;
            string ipler = "";

            try
            {
                IPAddress[] addresslist = Dns.GetHostAddresses(stackoverflow);

                foreach (IPAddress theaddress in addresslist)
                {
                    ipler = ipler + theaddress.ToString();
                }

                if (domainim.IpAdres != ipler)
                {
                    domainim.IpAdres = ipler;
                }

                Db.SaveChanges();

            }
            catch
            {
                ipler = "false";
            }

            return Json(ipler.ToString(), JsonRequestBehavior.AllowGet);
        }

        public static string Duzelt(string kelime)
        {
            kelime = kelime.Replace("-Jan-", " Ocak ");
            kelime = kelime.Replace("-Feb-", " Şubat ");
            kelime = kelime.Replace("-Mar-", " Mart ");
            kelime = kelime.Replace("-Apr-", " Nisan ");
            kelime = kelime.Replace("-May-", " Mayıs ");
            kelime = kelime.Replace("-Jun-", " Haziran ");
            kelime = kelime.Replace("-Jul-", " Temmuz ");
            kelime = kelime.Replace("-Aug-", " Ağustos ");
            kelime = kelime.Replace("-Sep-", " EYlül ");
            kelime = kelime.Replace("-Oct-", " Ekim ");
            kelime = kelime.Replace("-Nov-", " Kasım ");
            kelime = kelime.Replace("-Dec-", " Aralık ");
            kelime = kelime.Replace("-jan-", " Ocak ");
            kelime = kelime.Replace("-feb-", " Şubat ");
            kelime = kelime.Replace("-mar-", " Mart ");
            kelime = kelime.Replace("-apr-", " Nisan ");
            kelime = kelime.Replace("-may-", " Mayıs ");
            kelime = kelime.Replace("-jun-", " Haziran ");
            kelime = kelime.Replace("-jul-", " Temmuz ");
            kelime = kelime.Replace("-aug-", " Ağustos ");
            kelime = kelime.Replace("-sep-", " EYlül ");
            kelime = kelime.Replace("-oct-", " Ekim ");
            kelime = kelime.Replace("-nov-", " Kasım ");
            kelime = kelime.Replace("-dec-", " Aralık ");
            return kelime;
        }
        public class Whois
        {
            public enum RecordType { domain, nameserver, registrar };

            /// <summary>
            /// retrieves whois information
            /// </summary>
            /// <param name="domainname">The registrar or domain or name server whose whois information to be retrieved</param>
            /// <param name="recordType">The type of record i.e a domain, nameserver or a registrar</param>
            /// <returns>The string containg the whois information</returns>
            public static string lookup(string domainname, RecordType recordType)
            {
                List<string> res = lookup(domainname, recordType, "whois.internic.net");
                string result = "";
                foreach (string st in res)
                {
                    result += st + "\n";
                }
                return result;
            }        /// <summary>
                     /// retrieves whois information
                     /// </summary>
                     /// <param name="domainname">The registrar or domain or name server whose whois information to be retrieved</param>
                     /// <param name="recordType">The type of record i.e a domain, nameserver or a registrar</param>
                     /// <param name="returnlist">use "whois.internic.net" if you dont know whoisservers</param>
                     /// <returns>The string list containg the whois information</returns>
            public static List<string> lookup(string domainname, RecordType recordType, string whois_server_address)
            {
                string whoisSonucu = string.Empty;

                string[] tarihim = new string[3];
                string[] ns = new string[2];
                string[] date = new string[3];
                StreamReader oku;
                Stream str;
                byte[] diziDomain;
                string domain;
                string satir;

                if (domainname.ToLower().EndsWith(".tr"))
                {
                    TcpClient client = new TcpClient();
                    client.Connect("whois.nic.tr", 43);
                    domain = domainname + System.Environment.NewLine;
                    diziDomain = Encoding.ASCII.GetBytes(domain.ToCharArray());
                    str = client.GetStream();
                    str.Write(diziDomain, 0, domain.Length);
                    oku = new StreamReader(client.GetStream(), Encoding.ASCII);

                    List<string> result = new List<string>();
                    while (null != (satir = oku.ReadLine()))
                    {
                        result.Add(satir);
                    }
                    client.Close();
                    return result;
                }
                else if (domainname.ToLower().EndsWith(".ru"))
                {

                    TcpClient client = new TcpClient();
                    client.Connect("whois.ripn.net", 43);
                    domain = domainname + System.Environment.NewLine;
                    diziDomain = Encoding.ASCII.GetBytes(domain.ToCharArray());
                    str = client.GetStream();
                    str.Write(diziDomain, 0, domain.Length);
                    oku = new StreamReader(client.GetStream(), Encoding.ASCII);

                    List<string> result = new List<string>();
                    while (null != (satir = oku.ReadLine()))
                    {
                        result.Add(satir);
                    }
                    client.Close();
                    return result;
                }
                else if (domainname.ToLower().EndsWith(".eu"))
                {

                    TcpClient client = new TcpClient();
                    client.Connect("whois.eu", 43);
                    domain = domainname + System.Environment.NewLine;
                    diziDomain = Encoding.ASCII.GetBytes(domain.ToCharArray());
                    str = client.GetStream();
                    str.Write(diziDomain, 0, domain.Length);
                    oku = new StreamReader(client.GetStream(), Encoding.ASCII);

                    List<string> result = new List<string>();
                    while (null != (satir = oku.ReadLine()))
                    {
                        result.Add(satir);
                    }
                    client.Close();
                    return result;
                }
                else
                {
                    if (whois_server_address == "")
                        whois_server_address = "whois.internic.net";
                    TcpClient tcp = new TcpClient();
                    tcp.Connect(whois_server_address, 43);
                    string strDomain = recordType.ToString() + " " + domainname + "\r\n";
                    byte[] bytDomain = Encoding.ASCII.GetBytes(strDomain.ToCharArray());
                    Stream s = tcp.GetStream();
                    s.Write(bytDomain, 0, strDomain.Length);
                    StreamReader sr = new StreamReader(tcp.GetStream(), Encoding.ASCII);
                    string strLine = "";
                    List<string> result = new List<string>();
                    while (null != (strLine = sr.ReadLine()))
                    {
                        result.Add(strLine);
                    }
                    tcp.Close();
                    return result;
                }
            }
        }
    }
}