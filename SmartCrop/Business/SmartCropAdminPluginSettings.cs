using System;
using System.Data;
using EPiServer.Logging;
using EPiServer.PlugIn;

namespace Forte.SmartCrop.Business
{
    public class SmartCropAdminPluginSettings
    {
        private DataSet customDataSet;
        private const string Key = "SmartCrop";
        private static readonly ILogger Logger = LogManager.GetLogger();

        public SmartCropAdminPluginSettings()
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
                PlugInSettings.Populate(typeof(SmartCropAdminPluginSettings), customDataSet);
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
                PlugInSettings.Save(typeof(SmartCropAdminPluginSettings), customDataSet);
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message);
            }
        }

    }
}
