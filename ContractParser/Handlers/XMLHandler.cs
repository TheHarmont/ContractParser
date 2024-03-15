using System.Xml;
using Contract = ContractParser.Domain.Entity.Contract;

namespace ContractParser.Handlers
{
    public static class XMLHandler
    {
        /// <summary>
        /// Обрабатывает xml файл, находит в нем объект contract и парсит его содержимое.
        /// </summary>
        /// <param name="fileInfo">Параметры файла</param>
        /// <returns>Список, содержащий всю информацию как о файле, так и о его содержимом.</returns>
        public static List<Contract> GetFileData(Models.FileInfo fileInfo)
        {
            List<Contract> fileDataList = new List<Contract>();

            XmlDocument xDoc = new XmlDocument();
            xDoc.Load(fileInfo.filePath);

            #region Иницализация NameSpaceManager
            XmlNamespaceManager nsManager = new XmlNamespaceManager(xDoc.NameTable);
            nsManager.AddNamespace("ns3", "http://zakupki.gov.ru/oos/export/1");
            nsManager.AddNamespace("ns4", "http://zakupki.gov.ru/oos/common/1");
            nsManager.AddNamespace("def", "http://zakupki.gov.ru/oos/types/1");
            #endregion

            var contractNodes = NodeHandler.GetNodeContents(xDoc, "//ns3:export/ns3:contract", nsManager);
            if (contractNodes.IsCorrect())
            {
                foreach (XmlNode contract in contractNodes)
                {
                    Contract cD = new Contract();
                    cD.purchaseCode = NodeHandler.GetNodeContents(contract, ".//def:foundation//def:fcsOrder//def:order/def:purchaseCode", nsManager).GetNodeValue();

                    cD.regNum = NodeHandler.GetNodeContents(contract, "./def:regNum", nsManager).GetNodeValue();
                    cD.signDate = DateTime.Parse(NodeHandler.GetNodeContents(contract, "./def:signDate", nsManager).GetNodeValue());
                    cD.number = NodeHandler.GetNodeContents(contract, "./def:number", nsManager).GetNodeValue();

                    string contractSubject = NodeHandler.GetNodeContents(contract, "./def:contractSubject", nsManager).GetNodeValue();
                    //Приводим строку к требуемому виду.
                    cD.contractSubject = contractSubject.Replace("Поставка", "поставку").Replace("Оказание", "оказание").Replace("Поставку", "поставку");

                    //Из-за большого количества вложений, выделил в пути общую часть и поместил в legalEntityRF. Далее отталкиваюсь от неё
                    var legalEntityRF = NodeHandler.GetNodeContents(contract, ".//def:suppliersInfo//def:supplierInfo//def:legalEntityRF", nsManager);
                    if (legalEntityRF.IsCorrect())
                    {
                        var lERF = legalEntityRF.FirstOrDefault();
                        cD.singularName = NodeHandler.GetNodeContents(lERF, ".//def:EGRULInfo//def:legalForm/def:singularName", nsManager).GetNodeValue();
                        cD.fullName = NodeHandler.GetNodeContents(lERF, ".//def:EGRULInfo/def:fullName", nsManager).GetNodeValue();
                        cD.shortName = NodeHandler.GetNodeContents(lERF, ".//def:EGRULInfo/def:shortName", nsManager).GetNodeValue();
                        cD.INN = NodeHandler.GetNodeContents(lERF, ".//def:EGRULInfo/def:INN", nsManager).GetNodeValue();
                        cD.KPP = NodeHandler.GetNodeContents(lERF, ".//def:EGRULInfo/def:KPP", nsManager).GetNodeValue();
                        cD.address = NodeHandler.GetNodeContents(lERF, ".//def:EGRULInfo/def:address", nsManager).GetNodeValue();

                        cD.mailingAddress = NodeHandler.GetNodeContents(contract, ".//def:otherInfo//def:postAdressInfo/def:mailingAdress", nsManager).GetNodeValue();
                        cD.contactEMail = NodeHandler.GetNodeContents(contract, ".//def:otherInfo/def:contactEMail", nsManager).GetNodeValue();
                    }

                    cD.href = NodeHandler.GetNodeContents(contract, "./def:href", nsManager).GetNodeValue();
                    cD.counterpartyName = NodeHandler.GetNodeContents(contract, ".//def:suppliersInfo//def:supplierInfo//def:supplierAccountsDetails/def:supplierAccountDetails/ns4:counterpartyName", nsManager).GetNodeValue();

                    var attachments = NodeHandler.GetNodeContents(contract, ".//def:scanDocuments/def:attachment", nsManager);

                    //Один contract может иметь несколько секций attach (вложений)
                    //Парсим все секции, получаем url и fileName
                    if (attachments.IsCorrect())
                    {
                        cD.Attachments = attachments.Select(x => new Domain.Entity.Attachment
                        {
                            url = NodeHandler.GetNodeContents(x, "./def:url", nsManager).GetNodeValue(),
                            fileName = NodeHandler.GetNodeContents(x, "./def:fileName", nsManager).GetNodeValue()
                        }).ToList();
                    }

                    //Добавляем в объект информацию о файле
                    cD.sourceHash = fileInfo.fileHash;
                    cD.uploadDate = DateTime.Now;
                    cD.sourceSize = fileInfo.fileSize;


                    fileDataList.Add(cD);
                }
            }

            return fileDataList;
        }
    }
}
