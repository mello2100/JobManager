using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Net.Http;

namespace JobManagerCore.SignalR
{
    public static class StatusHubClient
    {
        private const string ServerURI = "http://localhost:53008/statusHub";

        private static HubConnection Connection { get; set; }

        public static void SendMessage(string txt)
        {
            try
            {
                Connection.InvokeAsync("sendMessage", txt);
            }
            catch (Exception er)
            {
            }
        }

        private static void ConnectAsync()
        {
            Connection = new HubConnectionBuilder().WithUrl(ServerURI).Build();

            try
            {
                Connection.StartAsync();
            }
            catch (HttpRequestException)
            {
                return;
            }
        }

        private static void Connection_Closed()
        {
        }

        public static void Conectar()
        {
            ConnectAsync();
        }

        public static void Fechar()
        {
            if (Connection != null)
            {
                Connection.StopAsync();
                Connection.DisposeAsync();
            }
        }
    }
}