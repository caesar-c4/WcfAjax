using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.Text;

namespace WcfAjax.Services
{
    [ServiceContract(Namespace = "")]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class Service
    {
        // To use HTTP GET, add [WebGet] attribute. (Default ResponseFormat is WebMessageFormat.Json)
        // To create an operation that returns XML,
        //     add [WebGet(ResponseFormat=WebMessageFormat.Xml)],
        //     and include the following line in the operation body:
        //         WebOperationContext.Current.OutgoingResponse.ContentType = "text/xml";
        //[OperationContract]
        //public void DoWork()
        //{
        //    // Add your operation implementation here
        //    return;
        //}

        // Add more operations here and mark them with [OperationContract]

        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json)]
        public List<SuperHero> GetAllHeroes()
        {
            return Data.SuperHeroes;
        }
        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json, UriTemplate = "GetHero/{id}")]
        public SuperHero GetHero(string id)
        {
            return Data.SuperHeroes.Find(s => s.Id == int.Parse(id));
        }

        //[OperationContract]
        //[WebGet(ResponseFormat = WebMessageFormat.Json)]
        //public SuperHero GetHero(int id)
        //{
        //    return Data.SuperHeroes.Find(s => s.Id == id);
        //}


        [OperationContract]
        [WebInvoke(ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "AddHero",
            Method = "POST")]
        public SuperHero AddHero(SuperHero hero)
        {
            hero.Id = Data.SuperHeroes.Max(s => s.Id) + 1;
            Data.SuperHeroes.Add(hero);
            return hero;
        }


        [OperationContract]
        [WebInvoke(ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "UpdateHero/{id}",
            Method = "PUT")]
        public SuperHero UpdateHero(SuperHero updatedHero, string id)
        {
            SuperHero hero = Data.SuperHeroes.Where(s => s.Id == int.Parse(id)).FirstOrDefault();
            hero.FirstName = updatedHero.FirstName;
            hero.LastName = updatedHero.LastName;
            hero.HeroName = updatedHero.HeroName;
            hero.PlaceOfBirth = updatedHero.PlaceOfBirth;
            hero.Combat = updatedHero.Combat;
            return hero;
        }

        [OperationContract]
        [WebInvoke(ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "DeleteHero/{id}",
            Method = "DELETE")]
        public List<SuperHero> DeleteHero(string id)
        {
            Data.SuperHeroes = Data.SuperHeroes.Where(s => s.Id != int.Parse(id)).ToList();
            return Data.SuperHeroes;
        }


        [OperationContract]
        [WebInvoke(ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "SearchHero/{searchText}",
            Method = "GET")]
        public List<SuperHero> SearchHero(string searchText)
        {
            List<SuperHero> result = Data.SuperHeroes
                .Where<SuperHero>(s => s.FirstName.ToLower().Contains(searchText)
                || s.LastName.ToLower().Contains(searchText)
                     || s.HeroName.ToLower().Contains(searchText)
                          || s.PlaceOfBirth.ToLower().Contains(searchText)
                ).ToList<SuperHero>();

            if (result.Count==0)
            {
                //WebOperationContext.Current.OutgoingResponse.StatusCode = System.Net.HttpStatusCode.NotFound;
                //WebOperationContext.Current.OutgoingResponse.StatusDescription = "Whoops";
                throw new WebFaultException<string>("No Hero Found", System.Net.HttpStatusCode.NotFound);
            }
            return result;
        }

        [OperationContract]
        [WebInvoke(ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "GetSortedHeroList/{type}",
            Method = "GET")]
        public List<SuperHero> GetSortedHeroList(string type)
        {
            switch (type)
            {
                case "firstname":
                    return Data.SuperHeroes.OrderBy(h => h.FirstName)
                        .ThenBy(s => s.LastName).ToList();

                case "lastname":
                    return Data.SuperHeroes.OrderBy(h => h.LastName)
                        .ThenBy(s => s.FirstName).ToList();

                case "hero":
                    return Data.SuperHeroes.OrderBy(h => h.HeroName)
                        .ThenBy(s => s.LastName).ToList();

                case "birthplace":
                    return Data.SuperHeroes.OrderBy(h => h.PlaceOfBirth)
                        .ThenBy(s => s.FirstName).ToList();

                case "combat":
                default:
                    return Data.SuperHeroes.OrderBy(h => h.Combat)
                        .ThenBy(s => s.HeroName).ToList();

            }
        }


          [OperationContract]
        [WebInvoke(ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "Fight/{id1}/{id2}",
            Method = "GET")]
        public string Fight(string id1, string id2)
        {
            string mes = " Wins!";

            SuperHero hero1 = Data.SuperHeroes.Find(s => s.Id == int.Parse(id1));
            SuperHero hero2 = Data.SuperHeroes.Find(s => s.Id == int.Parse(id2));
            
            if (hero1.Combat>hero2.Combat)
            {
                return hero1.HeroName + mes;
            }

            if (hero2.Combat > hero1.Combat)
            {
                return hero2.HeroName + mes;
            }

            return "\n It's a tie";
        }
    }
}
