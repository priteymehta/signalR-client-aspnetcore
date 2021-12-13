﻿using Microsoft.AspNetCore.SignalR.Client;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace SignalR.Client
{
    class Program
    {

        static readonly HttpClient httpClient = new HttpClient();
        static readonly string baseUrl = "http://localhost:5000";
        static async Task Main(string[] args)
        {
            //Console.Write("Username: ");
            var username = "";

            //Console.Write("Password: ");
            var password = "";
            //while (true)
            //{
            //    var key = Console.ReadKey(intercept: true);
            //    if (key.Key == ConsoleKey.Enter) break;
            //    password += key.KeyChar;
            //}

            var hubConnection = new HubConnectionBuilder().WithUrl($"{baseUrl}/signalr-server", options =>
            {
                //options.AccessTokenProvider = async () =>
                //{
                //    var stringData = JsonConvert.SerializeObject(new { username, password });
                //    var content = new StringContent(stringData);
                //    content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                //    var response = await httpClient.PostAsync($"{baseUrl}/api/token", content);
                //    response.EnsureSuccessStatusCode();
                //    return await response.Content.ReadAsStringAsync();
                //};
            }).Build();

            hubConnection.On<string, string>("newMessage", (sender, message) => Console.WriteLine($"{sender}: {message}"));
            await hubConnection.StartAsync();
            string currentConnectionId = await hubConnection.InvokeAsync<string>("GetClientConnectionId");
            System.Console.WriteLine("\nConnected with server.\nConnection ID: " + currentConnectionId);
            while (true)
            {
                var message = Console.ReadLine();
                await hubConnection.SendAsync("SendMessageToAllClients", message);
            }
        }

    }
}
