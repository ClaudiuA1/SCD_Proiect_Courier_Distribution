﻿using System.Collections.Generic;
using System.Net.Http;
using System;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Linq;

namespace WinFormsApp1
{
    internal class PackageService
    {
        static HttpClient client = new HttpClient();

        public void createConnection()
        {
            // Update port # in the following line.
            client.BaseAddress = new Uri("http://localhost:8080/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public List<Package> GetPackages()
        {
            List<Package> packages = null;
            HttpResponseMessage response = client.GetAsync("package").Result;
            if (response.IsSuccessStatusCode)
            {
                string resultString = response.Content.ReadAsStringAsync().Result;
                Console.WriteLine("received : " + resultString);
                packages = JsonSerializer.Deserialize<List<Package>>(resultString);
                return packages;

            }
            return null;
        }

        public List<Courier> GetBusyCouriers()
        {
            List<Package> packages = this.GetPackages();
            return packages
                .Select(p => p.courier)
                .GroupBy(c => c.name)  // Grupăm după nume
                .Select(g => g.First()) // Alegem primul curier din fiecare grup
                .ToList(); // Transformăm rezultatul în List<Courier>
        }

    }
}