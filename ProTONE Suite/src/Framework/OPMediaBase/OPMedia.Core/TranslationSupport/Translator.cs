#region Copyright (c) 2007, OPMedia Research
//
// Description  : Implements the Translator class. See
//                the Summary field below for more details.
//
// History      : O. Paraschiv,       2007-09-18, Creation.
//                O. Paraschiv,       2007-11-01, Redesign: One Web App for each type of provided content,
//
#endregion

//#define _NO_TRANSLATIONS

#region Using directives
using System;
using System.Resources;
using System.Globalization;
using System.Threading;
using OPMedia.Core.Logging;
using System.Reflection;
using System.Windows.Forms;
using System.Collections;
using System.Collections.Generic;
using OPMedia.Core;
using OPMedia.Core.Utilities;
using OPMedia.Core.Configuration;
using OPMedia.Core.InstanceManagement;
#endregion

namespace OPMedia.Core.TranslationSupport
{
    /// <summary>
    /// Class that provides internationalization support.
    /// </summary>
    /// <remarks>
    /// <b>How does Translator work. The Translation Assembly Stack (TAS)</b>
    /// 
    /// To use an assembly as a Translation Assembly, it must be added in the translation assembly list
    /// by means of a call to Translator.RegisterTranslationAssembly method.
    /// 
    /// Then, when translating a given tag (e.g. TXT_MYTAG) the Translator will try to look the
    /// tag up in all registered assembles. The final translation is built on the first found 
    /// translation resource that matches the given tag.
    /// 
    /// This approach would result in the fact that the first registered assemblies
    /// have priority on top of the last registered ones. What if the developer tries to 
    /// give another translation for the same tag ? With the approach of "return first tag
    /// to match" there would not be a chance to override a given translation tag, whatsoever.
    /// 
    /// For this reason the Translator does in fact a reverse lookup: it looks first into the 
    /// last registered assemnly, keeping the first registered one to be looked up the last.
    /// This yields that the translation assembly list acts like a Translation Assembly Stack.
    /// 
    /// Typically, translations are to be provided by the assemblies that expose UI elements.
    /// They have to be located in the Translations.resx file that resides in the Translations
    /// folder located in the project folder.
    /// 
    /// 
    /// For the OPMedia Library the register/lookup order is typically this one:
    /// 
    /// Assembly                       Register order        Lookup order
    /// 
    /// OPMedia.UI                           1                     1
    /// OPMedia.ProTONE.UI                   2                     2 
    /// OPMedia.MediaLibrary                 3                     3
    /// OPMedia.Addons.Builtin               4                     4
    /// 
    /// Mind that you have to register the addon library EXPLICITLY in order to use it as a Translation
    /// Assembly ! The best place for doing this is in the override of the ConfigureAddon method of the addon panel.
    /// 
    /// </remarks>
	public static class Translator
    {
        #region Members
        static bool translate = true;
        private static List<ResourceManager> _resManagers = new List<ResourceManager>();
        private static List<Assembly> _assemblies = new List<Assembly>();
        #endregion

        static List<string> __knownUntranslatableStrings = new List<string>();

        #region Methods
        /// <summary>
        /// Static constructor.
        /// </summary>
        static Translator()
        {
#if _NO_TRANSLATIONS
     translate = false;
#endif
            __knownUntranslatableStrings.AddRange(new string[]
            {
                "ColumnHeader",
                "...",
                "URL",
            });

        }

        /// <summary>
        /// Registers the given assembly as a Translation assembly.
        /// </summary>
        /// <param name="asm">Assembly to be registered</param>
        public static void RegisterTranslationAssembly(Assembly asm)
        {
            if (translate && asm != null && !_assemblies.Contains(asm))
            {
                _assemblies.Add(asm);

                ResourceManager resManager = 
                    new ResourceManager(string.Format("{0}.Translations.Translation",
                    asm.GetName().Name), asm);

                _resManagers.Add(resManager);
            }
        }

        /// <summary>
        /// Translates the specified tagged string.
        /// </summary>
        /// <param name="tag">The tagged string.</param>
        /// <param name="args">The arguments (syntax similar with
        /// the syntax of method String.Format)</param>
        /// <returns>The translated string</returns>
        public static string Translate(string tag, params object[] args)
		{
            string retVal = tag;

            if (translate)
            {
                if (IsTranslatable(tag))
                {
                    string translatedTag = Translate(tag);

                    retVal = string.Format(
                        (translatedTag == null) ? tag : translatedTag,
                        (args == null || args.Length > 0) ? args : new object[] { string.Empty });

                    if (retVal == tag)
                    {
                        // Not translated
                        Logger.LogUntranslated(tag);
                    }
                }
            }
            
            return retVal;
		}

        public static string Translate(string tag)
        {
            string retVal = tag;
            string log = string.Empty;

            if (translate)
            {
                if (IsTranslatable(tag))
                {
                    bool hasTrailingColon = tag.TrimEnd().EndsWith(":");
                    tag = tag.TrimEnd(':');

                    string translatedTag = null;

                    foreach (ResourceManager rm in _resManagers)
                    {
                        try
                        {
                            translatedTag = rm.GetString(tag, AppConfig.GetCulture(_languageId));
                            if (translatedTag != null)
                            {
                                log = $"{rm.BaseName.Replace("Translations.Translation", "").Trim('.')}=>{tag}";

                                retVal = translatedTag;

                                if (hasTrailingColon && !retVal.EndsWith(":"))
                                {
                                    retVal += ": ";
                                }

                                break;
                            }

                        }
                        catch (Exception ex)
                        {
                            Logger.LogException(ex);
                            retVal = tag;
                        }
                    }

                    if (retVal == tag)
                    {
                        Logger.LogUntranslated(tag);
                    }
                    else
                    {
                        Logger.LogTranslated($"{log}");
                    }
                }
            }

            return retVal;
        }

        static string _languageId = "en";

        /// <summary>
        /// Sets the interface language.
        /// </summary>
        /// <param name="languageId">The language id.</param>
        public static void SetInterfaceLanguage(string languageId)
        {
            try
            {
                if (OpMediaApplication.AllowRealTimeGUIUpdate == false ||
                    string.IsNullOrEmpty(languageId))
                {
                    languageId = "en";
                }

                Logger.LogTrace("SetInterfaceLanguage: Requested UI language is: {0}", languageId);
                Logger.LogTrace("SetInterfaceLanguage: Previous UI language was: {0}", _languageId);

                _languageId = languageId;

                StringUtils.RebuildDateTimeFormatString();
                EventDispatch.DispatchEvent(EventNames.PerformTranslation);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
        }

        public static string GetInterfaceLanguage()
        {
            return _languageId;
        }

        public static void TranslateControl(Control ctl, bool designMode)
        {
            try
            {
                if (translate && !designMode && ctl != null)
                {
                    foreach (Control child in ctl.Controls)
                    {
                        TranslateControl(child, designMode);
                    }

                    if (ctl is ListView)
                    {
                        foreach (ColumnHeader ch in (ctl as ListView).Columns)
                        {
                            if (IsTranslatable(ch.Text))
                                ch.Text = Translator.Translate(ch.Text);
                        }
                    }
                    else if (ctl is DataGridView)
                    {
                        foreach (DataGridViewColumn col in (ctl as DataGridView).Columns)
                        {
                            if (IsTranslatable(col.HeaderText))
                                col.HeaderText = Translator.Translate(col.HeaderText);
                        }
                    }
                    else if (ctl is ComboBox)
                    {
                    }
                    else
                    {
                        if (IsTranslatable(ctl.Text))
                            ctl.Text = Translator.Translate(ctl.Text);
                    }

                }
            }
            catch(Exception ex)
            {
                string s = ex.Message;
            }
        }

        public static void TranslateToolStrip(ToolStrip ts, bool designMode)
        {
            try
            {
                if (translate && !designMode && ts != null)
                {
                    foreach (ToolStripItem child in ts.Items)
                    {
                        TranslateToolStripItem(child, designMode);
                    }
                }
            }
            catch
            {
            }
        }

        public static void TranslateToolStripItem(ToolStripItem tsi, bool designMode)
        {
            try
            {
                if (translate && !designMode && tsi != null)
                {
                    if (tsi is ToolStripDropDownItem)
                    {
                        foreach (ToolStripItem child in (tsi as ToolStripDropDownItem).DropDownItems)
                        {
                            TranslateToolStripItem(child, designMode);
                        }
                    }

                   if (IsTranslatable(tsi.Text))
                        tsi.Text = Translator.Translate(tsi.Text);

                   tsi.ToolTipText = tsi.Text;
                   
                }
            }
            catch
            {
            }
        }

        public static string TranslateTaggedString(string taggedString)
        {
            string retVal = taggedString;

            if (translate && IsTranslatable(taggedString))
            {
                retVal = taggedString;

                foreach (ResourceManager rm in _resManagers)
                {
                    ResourceSet rs = rm.GetResourceSet(Thread.CurrentThread.CurrentUICulture, true, true);
                    if (rs != null)
                    {
                        IDictionaryEnumerator enu = rs.GetEnumerator();

                        while (enu.MoveNext())
                        {
                            retVal = retVal.Replace(enu.Key as string, enu.Value as string);
                        }
                    }
                }
            }

            return retVal;
        }
        #endregion

        private static bool IsTranslatable(string s)
        {
            if (string.IsNullOrEmpty(s) || 
                __knownUntranslatableStrings.Contains(s))
                return false;

            if (s.StartsWith("TXT_"))
                return true;

            Logger.LogUntranslatable(s);
            return false;
        }
    }
}