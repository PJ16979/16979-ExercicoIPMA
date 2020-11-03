using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Xml;

namespace ConsoleApp1
{
    class Program
    {
        static Dictionary<int, string> LerLocais(string ficheiro)
        {

            Dictionary<int, string> dicLocais = new Dictionary<int, string>();

            // Expressão Regular para instanciar objeto Regex.
            String erString = @"^[0-9]{7},[123],([1-9]?\d,){2}[A-Z]{3},([^,\n]*)$";
            Regex g = new Regex(erString);

            using (StreamReader r = new StreamReader(ficheiro))
            {
                string line;

                while ((line = r.ReadLine()) != null)
                {
                    Match m = g.Match(line);
                    if (m.Success)
                    {
                        string[] campos = m.Value.Split(',');
                        int codLocal = int.Parse(campos[0]);
                        string cidade = campos[5];
                        dicLocais.Add(codLocal, cidade);
                        continue;
                    }
                }
            }

            return dicLocais;
        }

        static PrevisaoIPMA LerFicheiroPrevisao(int globalID)
        {
            String jsonString = null;

            using (StreamReader reader = new StreamReader(@"../data_forecast/" + globalID + ".json"))
                jsonString = reader.ReadToEnd();

            PrevisaoIPMA obj = System.Text.Json.JsonSerializer.Deserialize<PrevisaoIPMA>(jsonString);
            return obj;
        }

        static void EscreveOutput(PrevisaoIPMA leitura)
        {
            string jsonData = JsonConvert.SerializeObject(leitura);
            File.WriteAllText(@"../outputs/" + leitura.globalIdLocal + ".json", jsonData);

            XmlDocument xmlDocument = JsonConvert.DeserializeXmlNode(jsonData, "root");
            xmlDocument.Save(@"../outputs/" + leitura.globalIdLocal + ".xml");
        }

        static void Main(string[] args)
        {
            PrevisaoIPMA leitura;
            Dictionary<int, string> locais = LerLocais("../locais.csv");
            int idGlobal;

            foreach (string ficheiro in Directory.GetFiles("../data_forecast/"))
            {
                idGlobal = 0;
                int.TryParse(Path.GetFileNameWithoutExtension(ficheiro), out idGlobal);

                // Verificar se a conversão foi feita corretamente. 
                // Caso não tenha sido, passa para o próximo ficheiro
                if (idGlobal == 0) continue;

                leitura = LerFicheiroPrevisao(idGlobal);

                try { leitura.Local = locais[leitura.globalIdLocal]; }
                catch { continue; }

                EscreveOutput(leitura);

                foreach (PrevisaoDia registo in leitura.data)
                    Console.WriteLine($"Dia: {registo.forecastDate} | Minímo: {registo.tMin} | Máximo: {registo.tMax} | Prob. precipitação: {registo.precipitaProb}\n");
            }
            



            

            Console.ReadLine();
        }
    }
}
