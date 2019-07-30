using System;
using System.Data;
using EPiServer.Logging;
using EPiServer.PlugIn;

namespace Forte.SmartFocalPoint.Business
{
    public class SmartFocalPointAdminPluginSettings
    {
        private static DataSet _customDataSet;
        private const string Key = "SmartFocalPoint";
        private static readonly ILogger Logger = LogManager.GetLogger();
        
        public SmartFocalPointAdminPluginSettings()
        {
            _customDataSet = new DataSet();
            _customDataSet.Tables.Add(new DataTable());
            _customDataSet.Tables[0].Columns.Add(new DataColumn(Key, typeof(bool)));
        }

        private bool LoadSettings()
        {

            var returnVal = false;
            try
            {
                PlugInSettings.Populate(typeof(SmartFocalPointAdminPluginSettings), _customDataSet);
                returnVal = (bool)_customDataSet.Tables[0].Rows[0][Key];
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message);
            }

            return returnVal;
        }

        public void SaveSettingsValue(bool value)
        {
            try
            {
                var newRow = _customDataSet.Tables[0].NewRow();
                newRow[Key] = value;
                _customDataSet.Tables[0].Rows.Add(newRow);
                PlugInSettings.Save(typeof(SmartFocalPointAdminPluginSettings), _customDataSet);
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message);
            }
        }

        public virtual bool IsConnectionEnabled()
        {
            return LoadSettings();
        }

    }
}
