namespace OPMedia.Runtime.ProTONE.FfdShowApi
{
    public static class FFDShowConstants
    {
        /// <summary>
        /// Parameter Id of FFDShow to set/get
        /// </summary>
        public enum FFDShowDataId : int
        {
            IDFF_isSubtitles = 801,
            IDFF_subTempFilename = 3402,
            IDFF_subShowEmbedded = 857, //id of displayed embedded subtitle, 0 if none,
            IDFF_fontName = 820,
            IDFF_fontCharset = 802,
            IDFF_fontSizeA = 824,
            IDFF_fontWeight = 804,
            IDFF_fontColor = 809,
            IDFF_isOSD = 1501,
            IDFF_OSDfontName = 1509,
            IDFF_OSDfontCharset = 1502,
            IDFF_OSDfontSize = 1503,
            IDFF_OSDfontWeight = 1504,
            IDFF_OSDfontColor = 1508,
            IDFF_fontItalic = 3555,
            IDFF_OSDfontItalic = 3556,
            IDFF_fontUnderline = 3557,
            IDFF_OSDfontUnderline = 3558,
        }
    }


    #region Constants
    /// <summary>
    /// List of commands understood by FFDShow remote API
    /// These are commands that concern integers transmission (get or set) or 
    /// single commands such as "Pause video"
    /// </summary>
    public enum FFD_WPRM : int
    {
        SET_PARAM_NAME = 0,
        SET_PARAM_VALUE_INT = 1,
        GET_PARAM_VALUE_INT = 3,
        SET_PARAM_VALUE_STR = 9,
        GET_FRAMERATE = 24,
        SET_WPRM_SET_OSDDURATION = 28,
        SET_WPRM_SET_OSD_CLEAN = 29,
    }

    /// <summary>
    /// List of commands understood by FFDShow remote API.
    /// These are commands that require strings transmissions
    /// </summary>
    public enum FFD_MSG
    {
        GET_PARAMSTR = 19,
        GET_CURRENT_SUBTITLES = 20,
        GET_SOURCEFILE = 22,
    }


    #endregion Constants
}
