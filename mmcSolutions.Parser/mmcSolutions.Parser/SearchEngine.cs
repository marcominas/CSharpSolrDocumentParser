using System;
using System.Collections.Generic;
using System.Net;
using System.Xml;
using mmcSolutions.SolrParser.Sample.DTO;

namespace mmcSolutions.SolrParser
{
    public class SearchEngine
    {

        /// <summary>
        /// Executa uma busca genérica.
        /// Essa busca deve ser usada para coleções que não tem um modelo fixo de domínio.
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static List<Document> Search(Parameters parameters)
        {
            return Search<Document>(parameters);
        }

        /// <summary>
        /// Executa uma busca no Solr.
        /// </summary>
        /// <typeparam name="T">Tipo de retorno desejado</typeparam>
        /// <param name="parameters">Parâmetros a serem usados na busca.</param>
        /// <returns>Uma lista de objetos do tipo T de acordo com os critérios especificados pelos parâmetros.</returns>
        public static List<T> Search<T>(Parameters parameters)
        {
            var instance = (T)Activator.CreateInstance<T>();

            if (instance is President)
                return QueryEmployeesByRole<T>(parameters, "president");

            if (instance is Director)
                return QueryEmployeesByRole<T>(parameters, "director");

            if (instance is Manager)
                return QueryEmployeesByRole<T>(parameters, "manager");

            if (instance is Supervisor)
                return QueryEmployeesByRole<T>(parameters, "supervisor");

            if (instance is Employee)
                return QueryEmployees<T>(parameters);

            // se não é nenhum dos acima retorna uma coleção de DocumentoBusca na lista de documentos.
            return GetSearchResult<T>(parameters, true);
        }

        #region Métodos Privados

        /// <summary>
        /// Executa uma query genérica.
        /// Essa busca deve ser usada para coleções que não tem um modelo fixo de domínio.
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        private static List<T> GetSearchResult<T>(Parameters parameters, bool addDocs)
        {
            var xml = GetSearchResult(parameters);
            var result = new List<T>();

            // Check if has some result to cast docs.
            if (!string.IsNullOrEmpty(xml.InnerXml))
            {
                // Adiciona um documento de busca para cada nó "<doc>" encontrado no XML retornado.
                if (addDocs)
                {
                    var type = typeof(T);
                    foreach (XmlNode item in GetNodes(xml, ResultParser.XpathSolrDocuments))
                    {
                        var format = "{0}";
                        if (!item.InnerXml.Contains("<doc>"))
                            format = "<doc>{0}</doc>";
                        var doc = (T)ResultParser.Parse<T>(string.Format(format, item.InnerXml));
                        result.Add(doc);
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// Specific manipulation according Type passed by execution method at runtime.
        /// </summary>
        /// <typeparam name="T">A Employee class or a derivated of it.</typeparam>
        /// <param name="parameters">Query parameters.</param>
        /// <param name="role">Role name to be validated.</param>
        /// <returns>A list with specifics criteria specified type.</returns>
        private static List<T> QueryEmployeesByRole<T>(Parameters parameters, string role)
        {
            // Add a "role" criteria if not already exists.
            if (!ContainsCriterionInCriteriaParameters("role", parameters))
                parameters.Criteria.Add("role", role);
            // Pass parameters to Solr Employee query.
            return QueryEmployees<T>(parameters);
        }

        /// <summary>
        /// Check if parameters already contains a "role" criteria.
        /// </summary>
        /// <param name="parameters">Query parameters.</param>
        /// <returns></returns>
        private static bool ContainsCriterionInCriteriaParameters(string criteriaName, Parameters parameters)
        {
            return parameters.Criteria.ContainsKey(criteriaName);
        }

        private static List<T> QueryEmployees<T>(Parameters parameters)
        {

            var xml = GetSearchResult(parameters);
            var docs = new List<T>();
            var type = typeof(T);
            object c;
            foreach (XmlNode doc in GetNodes(xml, ResultParser.XpathSolrDocuments))
            {
                var role = DocumentParser.GetEmployeeRole(doc.OuterXml);

                if (role.Equals("employee", StringComparison.OrdinalIgnoreCase))
                {
                    c = DocumentParser.ParseEmployee(doc.OuterXml);
                    if (c != null) docs.Add((T)c);
                }
                else if (role.Equals("supervisor", StringComparison.OrdinalIgnoreCase))
                {
                    c = DocumentParser.ParseSupervisor(doc.OuterXml);
                    if (c != null) docs.Add((T)c);
                }
                else if (role.Equals("manager", StringComparison.OrdinalIgnoreCase))
                {
                    c = DocumentParser.ParseManager(doc.OuterXml);
                    if (c != null) docs.Add((T)c);
                }
                else if (role.Equals("director", StringComparison.OrdinalIgnoreCase))
                {
                    c = DocumentParser.ParseDirector(doc.OuterXml);
                    if (c != null) docs.Add((T)c);
                }
                else if (role.Equals("president", StringComparison.OrdinalIgnoreCase))
                {
                    c = DocumentParser.ParsePresident(doc.OuterXml);
                    if (c != null) docs.Add((T)c);
                }
            }
            return docs;
        }

        /// <summary>
        /// Retorna um xml com base na query informada
        /// </summary>
        /// <param name="colecao"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        private static XmlDocument GetSearchResult(Parameters parameters)
        {
            if (!string.IsNullOrEmpty(parameters.SolrURL))
                // This method need be improved to a real use.
                return GetSearchResultFromWeb(parameters);
            else if (!string.IsNullOrEmpty(parameters.SolrResultFile))
                // This method is used for test purpose only.
                return GetSearchResultFromFile(parameters);
            else
                throw new NotImplementedException();
        }

        private static XmlDocument GetSearchResultFromWeb(Parameters parameters)
        {
            var xml = new XmlDocument();
            try
            {
                // TODO: cast Parameters into Solr query parameters to be passed with Solr URL.
                var req = WebRequest.Create(parameters.SolrURL);
                req.ContentType = "text/xml; encoding='utf-8'";
                req.Method = "GET";
                using (var resposta = req.GetResponse())
                {
                    xml.Load(new XmlTextReader(resposta.GetResponseStream()));
                }
            }
            catch (WebException we)
            {
                throw we;
            }

            return xml;
        }
        /// <summary>
        /// This method is used for test purpose only.
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        private static XmlDocument GetSearchResultFromFile(Parameters parameters)
        {
            var docs = new XmlDocument();
            var xml = System.IO.File.ReadAllText(parameters.SolrResultFile, System.Text.Encoding.Unicode);
            var result = "";
            var role = string.Empty;
            int count = 0;

            if (parameters.Criteria != null && parameters.Criteria.Count > 0)
                parameters.Criteria.TryGetValue("role", out role);

            foreach (XmlNode doc in ResultParser.GetSolrDocumentsFromXML(xml))
            {
                if (string.IsNullOrEmpty(role) || DocumentParser.GetEmployeeRole(doc.OuterXml).Equals(role, StringComparison.InvariantCultureIgnoreCase))
                {
                    result += doc.OuterXml;
                    count++;
                }
            }
            result = string.Format("<response>\r\n  <result>\r\n    {0}\n  </result>\r\n</response>", result);
            docs.LoadXml(result);
            return docs;
        }

        internal static XmlNodeList GetNodes(XmlNode xmlNode, string xpathQuery)
        {
            return xmlNode.SelectNodes(xpathQuery);
        }

        #endregion
    }
}
