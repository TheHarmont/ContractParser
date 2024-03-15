using ContractParser.Models;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace ContractParser.Handlers
{
    public static partial class NodeHandler
    {
        /// <summary>
        /// true - список не пуст,
        /// false - в противном случае
        /// </summary>
        public static bool IsCorrect(this List<XmlNode> nodeList)
        {
            return nodeList != null && nodeList.Any();
        }

        /// <summary>
        /// Возвращает список, состоящий из содержимого секции,
        /// отобранного по указанному пути
        /// </summary>
        public static List<XmlNode> GetNodeContents(XmlNode node, string xPath, XmlNamespaceManager nsManager)
        {
            return node.SelectNodes(xPath, nsManager).Cast<XmlNode>().ToList();
        }

        /// <summary>
        /// Возвращает первое значение из списка
        /// </summary>
        public static string? GetNodeValue(this List<XmlNode> nodeList)
        {
            return nodeList.IsCorrect() ? nodeList.FirstOrDefault()?.InnerText : null;
        }
    }
}
