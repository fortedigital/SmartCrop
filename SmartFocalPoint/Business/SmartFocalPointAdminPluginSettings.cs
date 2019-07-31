using System;
using System.Data;
using EPiServer.Logging;
using EPiServer.PlugIn;

namespace Forte.SmartFocalPoint.Business
{
    public class SmartFocalPointAdminPluginSettings
    {
        private static DataSet _customDataSet;
        private const string ConnectionEnabledKey = "IsConnectionEnabled";
        private const string MediaFolderKey = "SearchedMediaFolder";
        private static readonly ILogger Logger = LogManager.GetLogger();
        
        public SmartFocalPointAdminPluginSettings()
        {
            _customDataSet = new DataSet();
            _customDataSet.Tables.Add(new DataTable());
            _customDataSet.Tables[0].Columns.Add(new DataColumn(ConnectionEnabledKey, typeof(bool)));
            _customDataSet.Tables[0].Columns.Add(new DataColumn(MediaFolderKey, typeof(Guid)));
        }

        private object[] LoadSettings()
        {

            var returnBool = false;
            var returnGuid = Guid.Empty;
            try
            {
                PlugInSettings.Populate(typeof(SmartFocalPointAdminPluginSettings), _customDataSet);
                returnBool = (bool)_customDataSet.Tables[0].Rows[0][ConnectionEnabledKey];
                returnGuid = (Guid)_customDataSet.Tables[0].Rows[0][MediaFolderKey];
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message);
            }

            return new object[] {returnBool, returnGuid};
        }

        public void SaveSettingsValue(bool enabledFlag, Guid chosenFolder)
        {
            try
            {
                var newRow = _customDataSet.Tables[0].NewRow();
                newRow[ConnectionEnabledKey] = enabledFlag;
                newRow[MediaFolderKey] = chosenFolder;
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
            return (bool) LoadSettings()[0];
        }

        public virtual Guid GetChosenMediaFolder()
        {
            return (Guid) LoadSettings()[1];
        }

    }
}
