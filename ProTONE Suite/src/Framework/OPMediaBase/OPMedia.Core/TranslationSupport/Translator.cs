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
using System.Linq;
using System.Resources;
using System.Threading;
using OPMedia.Core.Logging;
using System.Reflection;
using System.Windows.Forms;
using System.Collections;
using System.Collections.Generic;
using OPMedia.Core.Utilities;
using OPMedia.Core.Configuration;
using OPMedia.Core.InstanceManagement;
#endregion

namespace OPMedia.Core.TranslationSupport
{
    /// <summary>
    /// Class that provides internationalization support.
    /// </summary>
	public static class Translator
    {
        #region Members
        static bool translate = true;

        private static Dictionary<string, TranslationFile> _translations = new Dictionary<string, TranslationFile>();
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

            AppConfig.SupportedCultures.ToList().ForEach(c =>
            {
                var tf = new TranslationFile(c.Name);
                tf.TranslationUpdated += Tf_TranslationUpdated;
                if (tf?.Translations?.Count > 0)
                    _translations.Add(c.Name, tf);
            });
        }

        private static void Tf_TranslationUpdated(string lang)
        {
            string crtLang = AppConfig.GetCulture(_languageId).Name;
            if (string.Compare(crtLang, lang, true) == 0)
                SetInterfaceLanguage(lang);
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

            try
            {
                if (translate)
                {
                    if (IsTranslatable(tag))
                    {
                        bool hasTrailingColon = tag.TrimEnd().EndsWith(":");
                        tag = tag.TrimEnd(':');

                        string translatedTag = null;

                        string lang = AppConfig.GetCulture(_languageId).Name;
                        if (_translations.ContainsKey(lang))
                        {
                            translatedTag = _translations[lang][tag];
                            if (translatedTag != null)
                            {
                                retVal = translatedTag;
                                if (hasTrailingColon && !retVal.EndsWith(":"))
                                    retVal += ": ";
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                retVal = tag;
            }

            if (retVal == tag)
            {
                Logger.LogUntranslated(tag);
            }
            else
            {
                Logger.LogTranslated($"{log}");
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

                string lang = AppConfig.GetCulture(_languageId).Name;
                if (_translations.ContainsKey(lang))
                {
                    foreach (var kvp in _translations[lang].Translations)
                    {
                        retVal = retVal.Replace(kvp.Key, kvp.Value);
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