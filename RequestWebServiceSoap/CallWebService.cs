using System.Xml;
using System.Net;
using System.IO;
using System;
using System.Data;

namespace RequestWebServiceSoap
{
    public class CallWebService
    {
        public static void CallWebServiceSoap()
        {
            var _url = "http://urlsoap/wsDocumentos.asmx";
            var _action = "http://urlactionsoap?op=FluxoCaixa";

            XmlDocument soapEnvelopeXml = CreateSoapEnvelope();
            HttpWebRequest webRequest = CreateWebRequest(_url, _action);
            InsertSoapEnvelopeIntoWebRequest(soapEnvelopeXml, webRequest);

            // begin async call to web request.
            IAsyncResult asyncResult = webRequest.BeginGetResponse(null, null);

            // suspend this thread until call is complete. You might want to
            // do something usefull here like update your UI.
            asyncResult.AsyncWaitHandle.WaitOne();

            // get the response from the completed web request.
            //string soapResult;
            DataSet ds = new DataSet();
            XmlDocument soapEnvelopeDocument = new XmlDocument();
            using (WebResponse webResponse = webRequest.EndGetResponse(asyncResult))
            {
                ds.ReadXml(webResponse.GetResponseStream(), XmlReadMode.Fragment);
                //using (StreamReader rd = new StreamReader(webResponse.GetResponseStream()))
                //{
                    
                //    soapResult = rd.ReadToEnd();
                //}
                
                //Console.Write(soapResult);
            }
        }

        private static HttpWebRequest CreateWebRequest(string url, string action)
        {
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);
            //webRequest.Headers.Add("SOAPAction", action);
            webRequest.ContentType = "text/xml;charset=\"utf-8\"";
            webRequest.Accept = "text/xml";
            webRequest.Method = "POST";
            return webRequest;
        }

        private static XmlDocument CreateSoapEnvelope()
        {
            XmlDocument soapEnvelopeDocument = new XmlDocument();
            string soapRequest = @"stringxmlsoap";
            soapEnvelopeDocument.LoadXml(soapRequest);
            return soapEnvelopeDocument;
        }

        private static void InsertSoapEnvelopeIntoWebRequest(XmlDocument soapEnvelopeXml, HttpWebRequest webRequest)
        {
            using (Stream stream = webRequest.GetRequestStream())
            {
                soapEnvelopeXml.Save(stream);
            }
        }
    }
}
