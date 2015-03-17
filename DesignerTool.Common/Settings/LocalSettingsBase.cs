using DesignerTool.Common.Base;
using DesignerTool.Common.Global;
using DesignerTool.Common.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml;

namespace DesignerTool.Common.Settings
{
    public class LocalSettingsBase : NotifyPropertyChangedBase
    {
        private const string ROOTNAME = "Settings";
        private const string VALUE_NODE = "Value";
        private const string DATATYPE_NODE = "DataType";
        private const string DESCRIPTION_NODE = "Description";

        private string _settingsFileName = Path.Combine(ApplicationPaths.ProgramData, "Settings.xml");
        public LocalSettingsBase()
        {
            // Create the settings file if it doesn't exist yet.
            if (!File.Exists(this._settingsFileName))
            {
                File.Create(this._settingsFileName);
            }

            this.loadFromFile();
        }

        #region Load

        private void loadFromFile()
        {
            // Load Settings XML
            try
            {
                // Load the xml document
                XmlDocument xDoc = new XmlDocument();
                xDoc.Load(this._settingsFileName);

                if ((xDoc == null) || (!xDoc.HasChildNodes))
                {
                    return;
                }

                // Get Root Element. Get the Element that contains the settings
                var rootElement = (from m in xDoc.ChildNodes.Cast<XmlNode>()
                                   where m.Name.ToUpper() == ROOTNAME.ToUpper()
                                       && m is XmlElement
                                   select m).FirstOrDefault() as XmlElement;

                if ((rootElement == null) || (!rootElement.HasChildNodes))
                {
                    return;
                }

                foreach (PropertyInfo prop in this.GetType().GetProperties())
                {
                    var xElement = (from m in rootElement.ChildNodes.Cast<XmlNode>()
                                    where m.Name.ToUpper() == prop.Name.ToUpper()
                                        && m is XmlElement
                                    select m).FirstOrDefault() as XmlElement;

                    if ((xElement == null) || (!xElement.HasChildNodes))
                    {
                        continue;
                    }

                    var xElementValue = (from m in xElement.ChildNodes.Cast<XmlNode>()
                                         where m.Name.ToUpper() == VALUE_NODE.ToUpper()
                                            && m is XmlElement
                                         select m).FirstOrDefault() as XmlElement;

                    if (xElementValue == null)
                    {
                        continue;
                    }

                    try
                    {
                        this.setPropertyValue(xElementValue.InnerXml, prop);
                    }
                    catch
                    {
                        // Skip this property, continue with the next
                    }
                }
            }
            catch
            {
                // Do not do anything here. Simply exit the method.
            }
        }

        /// <summary>
        /// Set the property's value according to the xml result.
        /// </summary>
        /// <param name="xmlValue"></param>
        /// <param name="prop"></param>
        private void setPropertyValue(string xmlValue, PropertyInfo prop)
        {
            TypeConverter conv = TypeDescriptor.GetConverter(prop.PropertyType);
            prop.SetValue(this, conv.ConvertFromString(xmlValue), null);
        }

        private DateTime getDateFromTicks(string value)
        {
            // Return the nullReturnValue if the value that is passed in is empty
            if (string.IsNullOrWhiteSpace(value))
            {
                return DateTime.MinValue;
            }

            try
            {
                long tmpTicks;
                if (!long.TryParse(value, out tmpTicks))
                {
                    return DateTime.MinValue;
                }

                return new DateTime(tmpTicks);
            }
            catch
            {
                return DateTime.MinValue;
            }
        }

        #endregion

        #region Save

        public void SaveToFile()
        {
            try
            {
                XmlDocument xDoc = new XmlDocument();

                // Try to load the local settings file if it exists
                FileInfo fInfo = new FileInfo(_settingsFileName);
                if (fInfo.Exists)
                {
                    try
                    {
                        xDoc.Load(_settingsFileName);
                    }
                    catch
                    {
                        // Do not throw the exception if the current file cannot be read. The system will automatically try to overwrite the file
                        // This might be because the file structure has been corrupted somehow
                    }
                }
                else
                {
                    // Make sure that the directory exists where the file will be saved
                    if (!Directory.Exists(fInfo.DirectoryName))
                    {
                        Directory.CreateDirectory(fInfo.DirectoryName);
                    }
                }

                // Try to find the settings root node in the document
                XmlElement xElementRootNode = null;
                if (xDoc != null)
                {
                    if (xDoc.HasChildNodes)
                    {
                        // Get the Element that contains the settings
                        xElementRootNode = (from m in xDoc.ChildNodes.Cast<XmlNode>()
                                            where m.Name.ToUpper() == ROOTNAME.ToUpper()
                                                && m is XmlElement
                                            select m)
                                            .FirstOrDefault() as XmlElement;
                    }

                    if (xElementRootNode == null)
                    {
                        // Create a new document. The structure of the old document might not be valid anymore.
                        xDoc = new XmlDocument();

                        // Create a new root element if the root element was not found.
                        xElementRootNode = xDoc.CreateElement(ROOTNAME);
                        xDoc.AppendChild(xElementRootNode);
                    }

                    if (xElementRootNode != null)
                    {
                        // Clear all the child elements
                        if (xElementRootNode.HasChildNodes)
                        {
                            xElementRootNode.RemoveAll();
                        }
                        // Loop through the cache and create an xmlNode for each item
                        foreach (PropertyInfo prop in this.GetType().GetProperties())
                        {
                            XmlElement xElementMain = xDoc.CreateElement(prop.Name.ToString());
                            if (xElementMain != null)
                            {
                                XmlElement xElementSettingValue = xDoc.CreateElement(VALUE_NODE);

                                // VALUE: Write the value for the property to the xml element
                                if (xElementSettingValue != null)
                                {
                                    if (prop.PropertyType == typeof(System.DateTime))
                                    {
                                        var tmpValue = (System.DateTime)prop.GetValue(this, null);

                                        if (tmpValue == null)
                                        {
                                            xElementSettingValue.InnerXml = string.Empty;
                                        }
                                        else
                                        {
                                            xElementSettingValue.InnerXml = tmpValue.Ticks.ToString();
                                        }
                                    }
                                    else
                                    {
                                        var tmpValue = prop.GetValue(this, null);

                                        if (tmpValue == null)
                                        {
                                            xElementSettingValue.InnerXml = string.Empty;
                                        }
                                        else
                                        {
                                            xElementSettingValue.InnerXml = tmpValue.ToString();
                                        }
                                    }

                                    xElementMain.AppendChild(xElementSettingValue);
                                }

                                // DESCRIPTION: Write the description of the property to the xml element
                                XmlElement xElementSettingDescription = xDoc.CreateElement(DESCRIPTION_NODE);
                                if (xElementSettingDescription != null)
                                {
                                    var settingDescription = (DescriptionAttribute)prop.GetCustomAttributes(typeof(DescriptionAttribute), false).FirstOrDefault();

                                    if (settingDescription != null)
                                    {
                                        try
                                        {
                                            xElementSettingDescription.InnerXml = settingDescription.Description.Trim();
                                        }
                                        catch
                                        {
                                            // Catch this here as the string that the developer entered might be invalid.
                                            xElementSettingDescription.InnerXml = string.Empty;
                                        }
                                    }
                                    else
                                    {
                                        xElementSettingDescription.InnerXml = string.Empty;
                                    }
                                    xElementMain.AppendChild(xElementSettingDescription);
                                }

                                // DATATYPE: Write the Data Type of the property to the xml element. This is not currently used for anything.
                                XmlElement xElementSettingDataType = xDoc.CreateElement(DATATYPE_NODE);
                                if (xElementSettingDataType != null)
                                {
                                    xElementSettingDataType.InnerXml = prop.PropertyType.ToString();

                                    xElementMain.AppendChild(xElementSettingDataType);
                                }

                                // Add the node to the document
                                xElementRootNode.AppendChild(xElementMain);
                            }
                        }

                        xDoc.Save(_settingsFileName);
                    }
                }
            }
            catch (Exception ex)
            {
                // Log and Suppress exception.
                Logger.Log("Exception when saving xml local settings", ex);
            }
        }

        #endregion
    }
}
