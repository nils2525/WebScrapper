using System;
using System.Net.Http;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace WebScrapper
{
    public class ScrappyClient : HttpClient
    {
        public int DelayFactor { get; set; } = 3;
        public HttpClientHandler HttpClientHandler { get; set; }

        private int _lastResponseTime;
        private long _lastResponseTicks;

        public ScrappyClient(HttpClientHandler handler) : base(handler)
        {
            _lastResponseTime = 0;
            _lastResponseTicks = DateTime.Now.Ticks;
        }

        public override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            //Wait until delay is expired
            while(_lastResponseTicks + (_lastResponseTime * DelayFactor) >= DateTime.Now.Ticks)
            {
                Task.Delay(1).Wait();
            }

            //ToDo if no user agent in property or request found, throw exception
            if(request.Headers.UserAgent == null)
            {
                throw new Exception();
            }

            return base.SendAsync(request, cancellationToken);            
        }
    }
}
