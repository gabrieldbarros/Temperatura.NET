using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Temperatura.Models;

namespace Temperatura.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            Dados dados = InfoApi();
            return View(dados);
        }

        public String Pesquisar(string dataIni, string dataFim)
        {
            string retorno = "";

            //Datas vazias
            if (string.IsNullOrEmpty(dataIni) && string.IsNullOrEmpty(dataFim))
            {
                retorno = "<tr><<td colspan='4' class='text-center'>Nenhuma informação encontrada</td></tr>";
            }
            else
            {
                Dados dados = InfoApi();
                DateTime dataHoje = DateTime.Today;

                //Data final vazia
                if (!string.IsNullOrEmpty(dataIni) && string.IsNullOrEmpty(dataFim))
                {
                    for (var i = 0; i < dados.Resultados.Count; i++)
                    {
                        DateTime dataApiDt = DateTime.Parse(dados.Resultados[i].Date);
                        DateTime dataIniDt = DateTime.Parse(dataIni);

                        if (dataApiDt >= dataIniDt)
                        {
                            retorno += "<tr>";
                            retorno += "<td>" + dados.Resultados[i].Date + "</td>";
                            retorno += "<td>" + dados.Resultados[i].Max + "</td>";
                            retorno += "<td>" + dados.Resultados[i].Min + "</td>";
                            retorno += "<td>" + dados.Resultados[i].Description + "</td>";
                            retorno += "</tr>";
                        }

                    }
                }
                else
                {
                    //Data inicial vazia
                    if (string.IsNullOrEmpty(dataIni) && !string.IsNullOrEmpty(dataFim))
                    {

                        for (var i = 0; i < dados.Resultados.Count; i++)
                        {
                            DateTime dataApiDt = DateTime.Parse(dados.Resultados[i].Date);
                            DateTime dataFimDt = DateTime.Parse(dataFim);

                            if (dataApiDt <= dataFimDt)
                            {
                                retorno += "<tr>";
                                retorno += "<td>" + dados.Resultados[i].Date + "</td>";
                                retorno += "<td>" + dados.Resultados[i].Max + "</td>";
                                retorno += "<td>" + dados.Resultados[i].Min + "</td>";
                                retorno += "<td>" + dados.Resultados[i].Description + "</td>";
                                retorno += "</tr>";
                            }

                        }
                    }
                    else
                    {
                        //As duas datas preenchidas
                        for (var i = 0; i < dados.Resultados.Count; i++)
                        {
                            DateTime dataApiDt = DateTime.Parse(dados.Resultados[i].Date);
                            DateTime dataIniDt = DateTime.Parse(dataIni);
                            DateTime dataFimDt = DateTime.Parse(dataFim);

                            if (dataApiDt >= dataIniDt && dataApiDt <= dataFimDt)
                            {
                                retorno += "<tr>";
                                retorno += "<td>" + dados.Resultados[i].Date + "</td>";
                                retorno += "<td>" + dados.Resultados[i].Max + "</td>";
                                retorno += "<td>" + dados.Resultados[i].Min + "</td>";
                                retorno += "<td>" + dados.Resultados[i].Description + "</td>";
                                retorno += "</tr>";
                            }

                        }
                    }
                }
            }

            if (string.IsNullOrEmpty(retorno))
            {
                retorno = "<tr><<td colspan='4' class='text-center'>Nenhuma informação encontrada</td></tr>";
            }

            return retorno;
        }

        public Dados InfoApi()
        {
            string urlApi = "https://api.hgbrasil.com/weather?woeid=455827";

            var model = new Dados();

            try
            {
                var httpWebRequest = (HttpWebRequest)WebRequest.Create(urlApi);
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "GET";

                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    dynamic obj = Newtonsoft.Json.JsonConvert.DeserializeObject(streamReader.ReadToEnd());

                    if (obj != null)
                    {
                        model.Resultados = obj.results.forecast.ToObject<List<Resultados>>();
                        model.CityName = obj.results.city_name;
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }

            return model;
        }
    }
}