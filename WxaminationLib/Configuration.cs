using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using ExaminationLib.Helpers;

namespace ExaminationLib
{
    public class Configuration
    {
        public static Message GetConfiguration(out ConfigurationModel configuration)
        {
            configuration = new ConfigurationModel();
            try
            {
                var settings = ConfigurationManager.AppSettings;
                if (settings.Count == 0)
                {
                    return new Message(false, "No AppSettings found in config file");
                }


                var checkList = new bool[]
                {
            settings.AllKeys.Any(x => x.ToString() == Constants.Constants.GROUP_NAME),
            settings.AllKeys.Any(x => x.ToString() == Constants.Constants.SUBJECTS),
            settings.AllKeys.Any(x => x.ToString() == Constants.Constants.PARTING_CHAR),
            settings.AllKeys.Any(x => x.ToString() == Constants.Constants.SUBJECT_PARTING_CHAR),
                };

                if (checkList.Contains(false))
                {
                    return new Message(false, "App.config does not contain required elemts(group, partingChar, subjects)");
                }


                configuration.Group = settings[Constants.Constants.GROUP_NAME].Trim();
                configuration.PartingChar = settings[Constants.Constants.PARTING_CHAR].Trim().FirstOrDefault();
                configuration.Subjects = settings[Constants.Constants.SUBJECTS].Trim().Split(',');
                configuration.SubjectPartingChar = settings[Constants.Constants.SUBJECT_PARTING_CHAR].Trim().FirstOrDefault();

            }
            catch (Exception ex)
            {
                return new Message(false, "Reading of configuration failed!", ex);
            }

            return new Message(true);
        }
    }
}
