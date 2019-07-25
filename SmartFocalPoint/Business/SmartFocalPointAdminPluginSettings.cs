using System;
using System.Data;
using EPiServer.Logging;
using EPiServer.PlugIn;

namespace Forte.SmartFocalPoint.Business
{
    public class SmartFocalPointAdminPluginSettings
    {
        private readonly DataSet _customDataSet;
        private const string Key = "SmartCrop";
        private static readonly ILogger Logger = LogManager.GetLogger();

        public SmartFocalPointAdminPluginSettings()
        {
            _customDataSet = new DataSet();
            _customDataSet.Tables.Add(new DataTable());
            _customDataSet.Tables[0].Columns.Add(new DataColumn(Key, typeof(string)));
        }

        private bool LoadSettings()
        {

            var returnVal = string.Empty;
            try
            {
                PlugInSettings.Populate(typeof(SmartFocalPointAdminPluginSettings), _customDataSet);
                returnVal = _customDataSet.Tables[0].Rows[0][Key].ToString();
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message);
            }

            return string.Equals("True", returnVal);
        }

        public virtual bool IsConnectionEnabled()
        {
            return LoadSettings();
        }

        public void SaveSettings(bool value)
        {
            try
            {
                var newRow = _customDataSet.Tables[0].NewRow();
                newRow[Key] = value.ToString();
                _customDataSet.Tables[0].Rows.Add(newRow);
                PlugInSettings.Save(typeof(SmartFocalPointAdminPluginSettings), _customDataSet);
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message);
            }
        }

    }
}
