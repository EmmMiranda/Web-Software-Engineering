using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Collections.Generic;
using RegistrationSystem.Models;
using System.IO;

namespace RegistrationSystemClient
{
    class Program
    {
        static HttpClient client = new HttpClient();

        static void Main(string[] args)
        {
            if (args != null)
                CreateEnhFileAsync(args[0], args[1], args[3]).GetAwaiter().GetResult();

        }

        private static async Task CreateEnhFileAsync(string cst_code, string sys_code, string ver_code)
        {
            try
            {
                client.BaseAddress = new Uri("https://registrationsystem.azurewebsites.net");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var response = await client.GetAsync("api/EnhancementRegistrationApi/"+cst_code+"/"+sys_code+"/"+ver_code);
                response.EnsureSuccessStatusCode();
                var enhancement_registrations = await response.Content.ReadAsAsync<IEnumerable<EnhancementRegistration>>();

                if (enhancement_registrations != null)
                {
                    using (FileStream fs = File.Create("rser.txt"))
                    using (TextWriter writer = new StreamWriter(fs))
                    {
                        foreach (var er in enhancement_registrations)
                        {
                            writer.WriteLine(er.Enr_Enh_Code);
                            Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(er, Newtonsoft.Json.Formatting.Indented));
                        }
                    }

                    using (FileStream fs = File.OpenRead("rser.txt"))
                    using (TextReader reader = new StreamReader(fs))
                    {
                        string enh_code;
                        while((enh_code = reader.ReadLine()) != null)
                        {
                            Console.WriteLine(enh_code);
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Cannot read content from");
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }

            Console.ReadLine();

        }

    }
}
