using System;
using System.Data;
using EPiServer.Logging;
using EPiServer.PlugIn;

namespace Forte.SmartFocalPoint.Business
{
    public class SmartFocalPointAdminPluginSettings
    {
        private DataSet customDataSet;
        private const string Key = "SmartCrop";
        private static readonly ILogger Logger = LogManager.GetLogger();

        public SmartFocalPointAdminPluginSettings()
        {
            customDataSet = new DataSet();
            customDataSet.Tables.Add(new DataTable());
            customDataSet.Tables[0].Columns.Add(new DataColumn(Key, typeof(string)));
        }

        public bool LoadSettings()
        {

            var returnVal = string.Empty;
            try
            {
                PlugInSettings.Populate(typeof(SmartFocalPointAdminPluginSettings), customDataSet);
                returnVal = customDataSet.Tables[0].Rows[0][Key].ToString();
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message);
            }

            return string.Equals("True", returnVal);
        }

        public void SaveSettings(bool value)
        {
            try
            {
                var newRow = customDataSet.Tables[0].NewRow();
                newRow[Key] = value.ToString();
                customDataSet.Tables[0].Rows.Add(newRow);
                PlugInSettings.Save(typeof(SmartFocalPointAdminPluginSettings), customDataSet);
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message);
            }
        }

    }
}
