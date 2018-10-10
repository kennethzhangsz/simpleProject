using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.IO;


namespace Demo_OSMO
{
    class xmlClass
    {
        #region "Define"
        public List<string> get_xelmentName = new List<string>();
        public List<string> get_xelmentValue = new List<string>();
        public List<string> get_rootXelmentName = new List<string>();
        #endregion


        string fileName = "test.xml";
        string fileSaveDirection = "c:\\";
        string xmlPath = "";
        
        void clearXelmentNameValue()
        {
            get_xelmentName.Clear();
        }

        void clearxelmentValue()
        {
            get_xelmentValue.Clear();
        }
        void clearrootXelmentName()
        {
            get_rootXelmentName.Clear();
        }


        /// <summary>
        /// creat simple xmlFile
        /// </summary>
        /// <param name="fileSaveDirection"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public string  CreatXmlFile(string fileSaveDirection, string fileName) //fileName根据//进行区分往下进行
        {
            string  rtn = string.Empty;
            try
            {               
                XDocument xdoc = new XDocument();
                XElement root = new XElement("parimater");
                XElement sample = new XElement("sample");
                sample.Add(new XElement("Date", "20180920"));
                root.Add(sample);
                xdoc.Add(root);
                xdoc.Save(fileSaveDirection  + fileName);
            }
            catch (Exception ex)
            {
                rtn = ex.ToString();
            }                
            return rtn;
        }
        /// <summary>
        /// send filePath 
        /// </summary>
        /// <param name="filePath"></param>
        public void setxmlFilePath(string filePath)
        {

            xmlPath = filePath;

        }

        /// <summary>
        ///  creat INI parameter
        /// </summary>
        void CreatXml()
        {
            XElement srcTree = new XElement("Root",
                new XElement("premater1",
                new XElement("Element1", 1),
                new XElement("Element2", 2),
                new XElement("Element3", 3),
                new XElement("Element4", 4),
                new XElement("Element5", 5)
                ),

                new XElement("premater2",
                new XElement("Element1", 1),
                new XElement("Element2", 2),
                new XElement("Element3", 3),
                new XElement("Element4", 4),
                new XElement("Element5", 5)
                ),

                new XElement("premater3",
                new XElement("Element1", 1),
                new XElement("Element2", 2),
                new XElement("Element3", 3),
                new XElement("Element4", 4),
                new XElement("Element5", 5)
                ),

                new XElement("Axis1",
                new XElement("P0", 0),
                new XElement("P1", 0),
                new XElement("P2", 0),
                new XElement("P3", 0),
                new XElement("P4", 0),
                new XElement("P5", 0),
                new XElement("P6", 0),
                new XElement("P7", 0),
                new XElement("P8", 0)
                ),

                new XElement("Axis2",
                new XElement("P0", 0),
                new XElement("P1", 0),
                new XElement("P2", 0),
                new XElement("P3", 0),
                new XElement("P4", 0),
                new XElement("P5", 0),
                new XElement("P6", 0),
                new XElement("P7", 0),
                new XElement("P8", 0)
                ),

                new XElement("Axis3",
                new XElement("P0", 0),
                new XElement("P1", 0),
                new XElement("P2", 0),
                new XElement("P3", 0),
                new XElement("P4", 0),
                new XElement("P5", 0),
                new XElement("P6", 0),
                new XElement("P7", 0),
                new XElement("P8", 0)
                ),

                new XElement("Axis4",
                new XElement("P0", 0),
                new XElement("P1", 0),
                new XElement("P2", 0),
                new XElement("P3", 0),
                new XElement("P4", 0),
                new XElement("P5", 0),
                new XElement("P6", 0),
                new XElement("P7", 0),
                new XElement("P8", 0)
                ),

                new XElement("Axis5",
                new XElement("P0", 0),
                new XElement("P1", 0),
                new XElement("P2", 0),
                new XElement("P3", 0),
                new XElement("P4", 0),
                new XElement("P5", 0),
                new XElement("P6", 0),
                new XElement("P7", 0),
                new XElement("P8", 0)
                ),

                new XElement("Axis6",
                new XElement("P0", 0),
                new XElement("P1", 0),
                new XElement("P2", 0),
                new XElement("P3", 0),
                new XElement("P4", 0),
                new XElement("P5", 0),
                new XElement("P6", 0),
                new XElement("P7", 0),
                new XElement("P8", 0)
                ),

                new XElement("Axis7",
                new XElement("P0", 0),
                new XElement("P1", 0),
                new XElement("P2", 0),
                new XElement("P3", 0),
                new XElement("P4", 0),
                new XElement("P5", 0),
                new XElement("P6", 0),
                new XElement("P7", 0),
                new XElement("P8", 0)
                ),

                new XElement("Axis8",
                new XElement("P0", 0),
                new XElement("P1", 0),
                new XElement("P2", 0),
                new XElement("P3", 0),
                new XElement("P4", 0),
                new XElement("P5", 0),
                new XElement("P6", 0),
                new XElement("P7", 0),
                new XElement("P8", 0)
                )

             );
            srcTree.Save(xmlPath);
        }

        /// <summary>
        /// find xmlelement value
        /// </summary>
        /// <param name="path"></param>
        /// <param name="fatherName"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        string checkXmlValue(string path, string fatherName, string name)
        {
            string rtn = "";
            if (File.Exists(path))
            {

                XElement xdoc = XElement.Load(path);
                var userValue = (from projec in xdoc.Element(fatherName).Elements(name) select projec).FirstOrDefault().Value;
                rtn = Convert.ToString(userValue);
            }

            return rtn;

        }

        /// <summary>
        /// addElement include name and value 
        /// </summary>
        /// <param name="path"></param>
        /// <param name="fathename"></param>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public void AddToElementAfterAndBefore(string path, string fathename, string name, string value)
        {
            if (File.Exists(path))
            {

                XElement xdoc = XElement.Load(path);
                var item = (from item2 in xdoc.Elements(fathename)

                            select item2).FirstOrDefault();
                if (item != null)
                {
                    XElement newitem = new XElement(name, value);
                    item.Add(newitem);
                    xdoc.Save(path);
                }

            }
        }

        /// <summary>
        /// modifiyXmlValue
        /// </summary>
        /// <param name="path"></param>
        /// <param name="fathername"></param>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public void modifyXmlValue(string path, string fathername, string name, string value)
        {
            if (File.Exists(path))
            {
                XElement xdoc = XElement.Load(path);
                var item = (from items in xdoc.Element(fathername).Elements(name) select items).FirstOrDefault();
                item.SetValue(value);
                xdoc.Save(path);


            }
        }

        /// <summary>
        /// showAllChildElement and save
        /// </summary>
        /// <param name="path"></param>
        /// <param name="name"></param>
        void showAllChildElement(string path, string name)
        {
            if (File.Exists(path))
            {
                XElement xele = XElement.Load(path);
                var item = xele.Descendants(name);
                foreach (var sub in item)
                {
                    var nameStr = sub.Name;
                    var nameValue = sub.Value;

                    get_xelmentName.Add(Convert.ToString(nameStr));
                    get_xelmentValue.Add(Convert.ToString(nameStr));
                }


            }
        }






        public string CheckValueToXmlFile(string path, string rootName,string elementName)
        {
            string rtn = string.Empty;
            try
            {
                if (string .IsNullOrEmpty(path)== false & path.Contains(".xml"))
                {
                    if (File.Exists(path)==true )
                    {
                        XDocument xdoc = XDocument.Load(path);
                        //foreach (XElement  item in xdoc.Root.Descendants(rootName))
                        //{
                        //    rtn = item.Element(elementName).Value;

                        //}

                        var checkItem = xdoc.Descendants(rootName)
                                            // .Where(p => p.Element("").Value.Contains("A"))   在进行筛选的时候进行
                                             .Select(p => new { name = p.Element(elementName).Value}).First();
                        rtn = checkItem.name;

                    }
                }
               
            }
            catch(Exception ex )
            {
                rtn = ex.ToString();
            }
            return rtn;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="rootName"></param>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public void AppendElement(string filePath,string rootName,string name,string value)
        {
            if (File.Exists(filePath))
            {
                XDocument xdoc = new XDocument();
                xdoc = XDocument.Load(filePath);

                XElement parentItem = xdoc.Descendants("parimater").First();
                                    //  .Where(p => p.Element(rootName).Value.Contains("ATE");                
               // XElement items = new XElement(name, new XAttribute("id", "03"), "09");
              
                XElement items2 = new XElement(name);
                items2.SetElementValue(name, value);
                parentItem.Add(items2);                             
                xdoc.Save(filePath);                                
            }
        }

        //public string ModifyNode(string filePath, string rootName, string name, string value)
        //{
        //    string rtn = string.Empty;
        //    try
        //    {
        //        if (File.Exists(filePath))
        //        {
        //            XDocument xdoc = new XDocument();
        //            xdoc = XDocument.Load(filePath);

        //            XElement item = xdoc.Descendants("name")
        //                .Where(p => (string)p.Element(name).Attribute("id").Value == "01").First();
        //            XElement item2 = xdoc.Element(name);
        //            XElement item3 = xdoc.Descendants("name")
                  
        //            //XElement root = xdoc.Descendants("paramter")
        //            //    .Where(p=> p.Element(rootName).Value=)
        //            XElement xelRoot = xdoc.Root;
        //            XElement rootName2 = xelRoot.Elements("").Where(x => x.Attribute("ID").Value == "1").Single();
        //            // XElement rootName3 = xelRoot.Descendants("");
        //            XElement rootname3 = xelRoot.Element("Axis2");

        //            var item = (from ele in xdoc.Descendants("child")
        //                        select ele).FirstOrDefault();
        //            var item2 = (from ele2 in xdoc.Descendants("child")
        //                         select ele2).FirstOrDefault();
        //            var item3 = (from ele3 in xdoc.Descendants("chile")
        //                         select ele3).FirstOrDefault();
        //            var item4 = (from ele3 in xdoc.Descendants("c") select ele3).Single();
        //            if (item3 != null)
        //            {
        //                foreach (var sub in item4.Ancestors())
        //                {
                           
        //                }

        //                foreach (var sub in item4.ElementsBeforeSelf())
        //                {

        //                }
        //                foreach (var sub in item4.ElementsAfterSelf())
        //                {
                            
        //                }
        //            }



                       
        //        }
        //    }
        //    catch
        //    {

        //    }            
        //    return rtn;
        //}



    }
}
