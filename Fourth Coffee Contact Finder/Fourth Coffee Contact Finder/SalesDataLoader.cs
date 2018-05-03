using System;
using System.IO;
using System.Text;
using FourthCoffee.DataService.Infrastructure;
// 01: Bring the System.Net namespace into scope.
using System.Net;

namespace Fourth_Coffee_Contact_Finder
{
    public class SalesDataLoader
    {
        Uri _serviceUri;
        // Declare a global object to encapsulate an HTTP request.
        HttpWebRequest _request;
  

        public SalesDataLoader(Uri serviceUri)
        {
            if(serviceUri ==null)
                 throw new NullReferenceException("serviceUri");

            this._serviceUri = serviceUri;
        }


        public SalesPerson GetPersonByEmail(string email)
        {
            if (string.IsNullOrEmpty(email))
                return null;

            this.InitializeRequest();

            var rawData = Encoding.Default.GetBytes(
                "{\"emailAddress\":\"" + email.Trim() + "\"}");

            // 04: Configure the request to send JSON data.
            this._request.Method = "Post";
            this._request.ContentType = "Aplication/JSON";
            this._request.ContentLength = rawData.Length;
          

            this.WriteDataToRequestStream(rawData);

            return this.ReadDataFromResponseStream();
        }

        private void InitializeRequest()
        {
            // 03: Instantiate the _request object.
            this._request = WebRequest.Create(this._serviceUri.AbsoluteUri) as HttpWebRequest;
         

            if (this._request == null)
                throw new NullReferenceException("_request");
        }

        private void WriteDataToRequestStream(byte[] data)
        {
            // 05: Write data to the request stream.
            var datastream = this._request.GetRequestStream();
            datastream.Write(data, 0, data.Length);
            datastream.Close();
           
        }

        private SalesPerson ReadDataFromResponseStream()
        {
            // 06: Create an HttpWebResponse object.
            var response = this._request.GetResponse()
                as HttpWebResponse;


            // 07: Check to see if the response contains any data.
            if (response.ContentLength == 0) return null;



            // 08: Read and process the response data.
            var stream = new StreamReader(response.GetResponseStream());
            var result = SalesPerson.FromJson(stream.BaseStream);
            stream.Close();
          

            return result;
        }
    }
}
