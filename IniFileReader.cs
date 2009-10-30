using System;
using System.Collections;
using System.Collections.Specialized;
using System.IO;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Xsl;
using System.Xml.XPath;
using System.Text;

enum IniItemTypeEnum {
   GetKeys=0,
   GetValues=1,
   GetKeysAndValues=2
}
namespace IniFile {
   public class IniFileReaderNotInitializedException : System.ApplicationException {
      public override String Message {
         get {
            return "The IniFileReader instance has not been properly initialized.";
         }
      }
   }
   public class IniFileReader {
      private String m_IniFilename;
      //private int commentCount=0;
      private XmlDocument m_XmlDoc;
      private ArrayList unattachedComments = new ArrayList();
      private StringCollection sections = new StringCollection();
      private bool m_CaseSensitive = false;
      private String m_SaveFilename;
      private bool m_initialized = false;
      public IniFileReader(String IniFilename) {
         InitIniFileReader(IniFilename,false);
      }
      public IniFileReader(String IniFilename, bool IsCaseSensitive) {
         InitIniFileReader(IniFilename, IsCaseSensitive);
      }
      private void InitIniFileReader(String IniFilename, bool IsCaseSensitive) {
         FileInfo fi=null;
         String s;
         TextReader tr=null;
         m_CaseSensitive=IsCaseSensitive;
         m_XmlDoc = new XmlDocument();
         if ((IniFilename == null) || (IniFilename.Trim() == "")) {
            return;
         }
         //try to load the file as an XML file
         try {
            m_XmlDoc.Load(IniFilename);
            UpdateSections();
            m_IniFilename=IniFilename;
            m_initialized=true;
         }
         catch  {
            // load the default XML
            m_XmlDoc.LoadXml("<?xml version=\"1.0\" encoding=\"UTF-8\"?><sections></sections>");
            try  {
               fi = new FileInfo(IniFilename);
               if (fi.Exists) {
                  tr = fi.OpenText();
                  while ((s = tr.ReadLine()) != null) {
                     ParseLineXml(s, m_XmlDoc);
                  }
                  m_IniFilename=IniFilename;
                  m_initialized=true;
               }
               else{
                  m_XmlDoc.Save(IniFilename);
                  m_IniFilename=IniFilename;
                  m_initialized=true;
               }
            }
            catch (Exception e) {
               MessageBox.Show(e.Message);
            }
            finally {
               if (tr != null) {
                  tr.Close();
               }
            }
         }
      }
      public String IniFilename {
         get {
            if (!Initialized) throw new IniFileReaderNotInitializedException();
            return (m_IniFilename);
         }
      }
      
      public bool Initialized {
         get {
            return m_initialized;
         }
      }

      public bool CaseSensitive {
         get {
            return m_CaseSensitive;
         }
      }

      private String SetNameCase(String aName) {
         if (CaseSensitive) {
            return aName;
         }
         else {
            return aName.ToLower();
         }
      }

      private XmlElement GetRoot() {
         return m_XmlDoc.DocumentElement;
      }
		
      private XmlElement GetLastSection() {
         if (sections.Count == 0) {
            return GetRoot();
         }
         else {
            return GetSection(sections[sections.Count-1]);
         }
      }

      private XmlElement GetSection(String sectionName) {
         if ((sectionName != null) && (sectionName != "")) {
            sectionName = SetNameCase(sectionName);
            return ((XmlElement) m_XmlDoc.SelectSingleNode("//section[@name='" + sectionName + "']"));
         }
         return null;
      }

      private XmlElement GetItem(String sectionName, String keyName) {
         if ((keyName != null) && (keyName != "")) {
            keyName = SetNameCase(keyName);
            XmlElement section = GetSection(sectionName);
            if (section != null) {
               return (XmlElement) section.SelectSingleNode("item[@key='" + keyName + "']");
            }
         }
         return null;
      }


      public bool SetIniSection(String oldSection, String newSection) {
         if (!Initialized) throw new IniFileReaderNotInitializedException();
         if ((newSection != null) && (newSection != "")) {
            XmlElement section = GetSection(oldSection);
            if (section != null) {
               section.SetAttribute("name",SetNameCase(newSection));
               UpdateSections();
               return true;
            }
         }
         return false;
      }
      public bool SetIniValue(String sectionName, String keyName, String newValue) {
         XmlElement item = null;
         XmlElement section= null;
         if (!Initialized) throw new IniFileReaderNotInitializedException();
         section = GetSection(sectionName);
         if (section == null) {
            if (CreateSection(sectionName)) {
               section = GetSection(sectionName);
               // exit if keyName is null or blank
               if ((keyName==null) || (keyName == "")){
                  return true;
               }
            }
            else {
               // can't create section
               return false;
            }
         }
         if (keyName == null) {
            // delete the section
            return DeleteSection(sectionName);
         }         
         item = GetItem(sectionName, keyName);
         if (item != null) {
            if (newValue==null) {
               // delete this item
               return DeleteItem(sectionName, keyName);
            }
            else {
               // add or update the value attribute
               item.SetAttribute("value", newValue);
               return true;
            }
         }
         else {
            // try to create the item
            if ((keyName != "") && (newValue != null)) {
               // construct a new item (blank values are OK)
               item = m_XmlDoc.CreateElement("item");
               item.SetAttribute("key",SetNameCase(keyName));
               item.SetAttribute("value",newValue);
               section.AppendChild(item);
               return true;
            }
         }
         return false;         
      }

      private bool DeleteSection(String sectionName){
         XmlElement section=null;
         if ((section = GetSection(sectionName)) != null) {
            section.ParentNode.RemoveChild(section);
            this.UpdateSections();
            return true;
         }
         return false;  
      }
      private bool DeleteItem(String sectionName, String keyName){
         XmlElement item = null;
         if ((item = GetItem(sectionName, keyName)) != null) {
            item.ParentNode.RemoveChild(item);
            return true;
         }
         return false;  
      }

      public bool SetIniKey(String sectionName, String keyName, String newValue) {
         if (!Initialized) throw new IniFileReaderNotInitializedException();
         XmlElement item = GetItem(sectionName, keyName);
         if (item != null) {
            item.SetAttribute("key", SetNameCase(newValue));
            return true;
         }
         return false;         
      }

      public String GetIniValue(String sectionName, String keyName) {
         if (!Initialized) throw new IniFileReaderNotInitializedException();
         XmlNode N = GetItem(sectionName, keyName);
         if (N != null) {
            return(N.Attributes.GetNamedItem("value").Value);
         }
         return null;
      }
      public StringCollection GetIniComments(String sectionName) {
         if (!Initialized) throw new IniFileReaderNotInitializedException();
         StringCollection sc = new StringCollection();
         XmlNode target = null;
         if (sectionName == null) {
            target = m_XmlDoc.DocumentElement;
         }
         else {
            target = GetSection(sectionName);
         }
         if (target != null) {
            XmlNodeList nodes = target.SelectNodes("comment");
            if (nodes.Count > 0) {
               foreach (XmlNode N in nodes) {
                  sc.Add(N.InnerText);
               }
            }
         }
         return sc;
      }

      public bool SetIniComments(string sectionName, StringCollection comments) {
         if (!Initialized) throw new IniFileReaderNotInitializedException();
         XmlNode target = null;
         if (sectionName==null) {
            target = m_XmlDoc.DocumentElement;
         }
         else {
            target = GetSection(sectionName);
         }
         if (target != null) {
            XmlNodeList nodes = target.SelectNodes("comment");
            foreach (XmlNode N in nodes) {
               target.RemoveChild(N);
            }
            foreach (String s in comments)  {
               XmlElement N = m_XmlDoc.CreateElement("comment");
               N.InnerText=s;
               XmlElement NLastComment = (XmlElement) target.SelectSingleNode("comment[last()]");
               if (NLastComment==null) { 
                  target.PrependChild(N);
               }
               else {
                  target.InsertAfter(N, NLastComment);
               }
            }
            return true;
         }
         return false;
      }        
      private void UpdateSections() {
         sections = new StringCollection();
         foreach(XmlElement N in m_XmlDoc.SelectNodes("sections/section")) {
            sections.Add(N.GetAttribute("name"));
         }
      }

      public StringCollection AllSections {
         get {
            if (!Initialized) {
               throw new IniFileReaderNotInitializedException();
            }
            return sections;
         }
      }

      private StringCollection GetItemsInSection(String sectionName, IniItemTypeEnum itemType) {
         XmlNodeList nodes=null;
         StringCollection items = new StringCollection();
         XmlNode section = GetSection(sectionName);			
         if (section == null) {
            return null;
         }
         else {
            nodes = section.SelectNodes("item");
            if (nodes.Count > 0) {
               foreach(XmlNode N in nodes) {
                  switch (itemType) {
                     case IniItemTypeEnum.GetKeys :
                        items.Add(N.Attributes.GetNamedItem("key").Value);
                        break;
                     case IniItemTypeEnum.GetValues :
                        items.Add(N.Attributes.GetNamedItem("value").Value);
                        break;
                     case IniItemTypeEnum.GetKeysAndValues :
                        items.Add(N.Attributes.GetNamedItem("key").Value + "=" + 
                           N.Attributes.GetNamedItem("value").Value);
                        break;
                  }
               }
            }
            return items;
         }
      }
      public StringCollection AllKeysInSection(String sectionName) {
         if (!Initialized) throw new IniFileReaderNotInitializedException();
         return(GetItemsInSection(sectionName, IniItemTypeEnum.GetKeys));
      }

      public StringCollection AllValuesInSection(String sectionName) {
         if (!Initialized) {
            throw new IniFileReaderNotInitializedException();
         }
         return(GetItemsInSection(sectionName, IniItemTypeEnum.GetValues));
      }
      
      public StringCollection AllItemsInSection(String sectionName) {
         if (!Initialized) throw new IniFileReaderNotInitializedException();
         return(GetItemsInSection(sectionName, IniItemTypeEnum.GetKeysAndValues));
      }

      public string GetCustomIniAttribute(string sectionName, string keyName, string attributeName) {
         if (!Initialized) throw new IniFileReaderNotInitializedException();
         if ((attributeName != null) && (attributeName  != "")) {
            XmlElement N = GetItem(sectionName, keyName);
            if (N != null) {
               attributeName = SetNameCase(attributeName);
               return N.GetAttribute(attributeName);
            }
         }
         return null;
      }

      public bool SetCustomIniAttribute(string sectionName, string keyName, string attributeName, string attributeValue) {
         if (!Initialized) throw new IniFileReaderNotInitializedException();
         if (attributeName  != "") {
            XmlElement N = GetItem(sectionName, keyName);
            if (N != null) {
               try {
                  if (attributeValue == null) {
                     // delete the attribute
                     N.RemoveAttribute(attributeName);
                     return true;
                  }
                  else {
                     attributeName = SetNameCase(attributeName);
                     N.SetAttribute(attributeName, attributeValue);
                     return true;
                  }
               }
               catch (Exception e) {
                  MessageBox.Show(e.Message);
               }
            }
         }
         return false;
      }
      
      private bool CreateSection(String sectionName) {
         if ((sectionName != null) && (sectionName != "")) {
            sectionName=SetNameCase(sectionName);
            try {
               XmlElement N = m_XmlDoc.CreateElement("section");
               XmlAttribute Natt = m_XmlDoc.CreateAttribute("name");
               Natt.Value = SetNameCase(sectionName);
               N.Attributes.SetNamedItem(Natt);
               m_XmlDoc.DocumentElement.AppendChild(N);
               sections.Add(Natt.Value);
               return true;
            }
            catch (Exception e) {
               MessageBox.Show(e.Message);
               return false;
            }
         }
         return false;
      }
      private bool CreateItem(String sectionName, String keyName, String newValue) {
         XmlElement item = null;
         XmlElement section = null;
         try {
            if ((section = GetSection(sectionName)) != null) {
               item = m_XmlDoc.CreateElement("item");
               item.SetAttribute("key", keyName);
               item.SetAttribute("newValue",newValue);
               section.AppendChild(item);
               return true;
            }
            return false;
         }
         catch (Exception e) {
            MessageBox.Show(e.Message);
            return false;
         }
      }

      private void ParseLineXml(String s, XmlDocument doc) {
         s.TrimStart();
         String key, value;
         XmlElement N;
         XmlAttribute Natt;
         if (s.Length == 0) {
            return;
         }
         switch (s.Substring(0,1)) {
            case "[" :
               // this is a section
               // trim the first and last characters
               s = s.TrimStart('[');
               s = s.TrimEnd(']');
               // create a new section element
               CreateSection(s);
               break;
            case ";" :
               // new comment
               N = doc.CreateElement("comment"); // + commentCount.ToString());					
               //commentCount++;
               N.InnerText = s.Substring(1);
               GetLastSection().AppendChild(N);
               break;                  
            default :
               // split the string on the "=" sign, if present
               if (s.IndexOf('=') > 0) {
                  String[] parts = s.Split('=');
                  key = parts[0].Trim();
                  value = parts[1].Trim();
               }
               else {
                  key = s;
                  value = "";
               }
               N = doc.CreateElement("item");	
               Natt = doc.CreateAttribute("key");
               Natt.Value = SetNameCase(key);
               N.Attributes.SetNamedItem(Natt);
               Natt = doc.CreateAttribute("value");
               Natt.Value = value;
               N.Attributes.SetNamedItem(Natt);
               GetLastSection().AppendChild(N);
               break;
         }
			

      }
      public String OutputFilename {
         get {
            if (!Initialized) throw new IniFileReaderNotInitializedException();
            return m_SaveFilename;
         }
         set {
            if (!Initialized) throw new IniFileReaderNotInitializedException();
            FileInfo fi = new FileInfo(value);
            if (!fi.Directory.Exists) {
               MessageBox.Show("Invalid path.");
            }
            else {
               m_SaveFilename = value;
            }
         }
      }

      public void Save() {
         if (!Initialized) throw new IniFileReaderNotInitializedException();
         if ((OutputFilename != null) && (m_XmlDoc != null)) {
            FileInfo fi = new FileInfo(OutputFilename);
            if (!fi.Directory.Exists) {
               MessageBox.Show("Invalid path.");
               return;
            }
            if (fi.Exists) {
               fi.Delete();
               m_XmlDoc.Save(OutputFilename);
            }
         }
      }

      public String AsIniFile() {
         if (!Initialized) throw new IniFileReaderNotInitializedException();
         try {
            XslCompiledTransform xsl = new XslCompiledTransform();
            xsl.Load("c:\\XMLToIni.xslt");
            StringBuilder sb = new StringBuilder();
            StringWriter sw = new StringWriter(sb);
            xsl.Transform(m_XmlDoc,null,sw);
            sw.Close();
            return sb.ToString();
         }
         catch (Exception e) {
            MessageBox.Show(e.Message);
            return null;
         }
      }


      
      public XmlDocument XmlDoc {
         get {
            if (!Initialized) throw new IniFileReaderNotInitializedException();
            return m_XmlDoc;
         }
      }

      public String XML {
         get {
            if (!Initialized) throw new IniFileReaderNotInitializedException();
            StringBuilder sb = new StringBuilder();
            StringWriter sw = new StringWriter(sb);
            XmlTextWriter xw = new XmlTextWriter(sw);
            xw.Indentation=3;
            xw.Formatting=Formatting.Indented;
            m_XmlDoc.WriteContentTo(xw);
            xw.Close();
            sw.Close();
            return sb.ToString();
         }
      }

   }
}
