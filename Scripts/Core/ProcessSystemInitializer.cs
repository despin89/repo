namespace GD.Core
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Xml.Linq;
    using Game.ActionsSystem;
    using Game.Combat;
    using MessengerSystem;
    using UnityEngine;

    public static class ProcessSystemInitializer
    {
        #region Constants

        private static readonly string _systemNamespace = "GD.Game.ActionsSystem.ProcessSystems.";

        private static List<ProcessSystemBase> _processSystems;

        #endregion

        public static void Init()
        {
            if (_processSystems == null)
            {
                _processSystems = new List<ProcessSystemBase>();
                string data =
                    File.ReadAllText(Application.streamingAssetsPath + "/Data/ProcessSystemData/ProcessSystems.xml");
                XDocument database = XDocument.Parse(data);
                List<XElement> events = database.Root.Elements("Event").ToList();
                foreach (XElement e in events)
                foreach (XElement listner in e.Elements("Listner"))
                {
                    ProcessSystemBase system =
                        Activator.CreateInstance(Type.GetType(_systemNamespace + listner.Value)) as
                            ProcessSystemBase;
                    if (system != null)
                    {
                        XAttribute messengerAttribute = e.Attribute("Messenger");
                        if ((messengerAttribute != null) && (messengerAttribute.Value == "CombatantMessenger"))
                        {
                            Messenger<ICombatant>.AddListener(e.Attribute("Type").Value, system.Process);
                        }
                        else
                        {
                            CombatMessenger.AddListener(e.Attribute("Type").Value, system.Process);
                        }
                        _processSystems.Add(system);
                    }
                }
            }
        }
    }
}