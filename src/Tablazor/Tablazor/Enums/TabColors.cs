using Tablazor.Attributes;

namespace Tablazor.Enums;

/// <summary>
/// Represents the available colors in Tablazor
/// </summary>
public enum TabColors
{
    /// <summary>
    /// Default color
    /// </summary>
    Default,

    /// <summary>
    /// Primary
    /// </summary>
    [CssVariableName("--tblr-primary")]
    [CssClassName("primary")]
    Primary,

    /// <summary>
    /// Secondary
    /// </summary>
    [CssVariableName("--tblr-secondary")]
    [CssClassName("secondary")]
    Secondary,

    /// <summary>
    /// Success
    /// </summary>
    [CssVariableName("--tblr-success")]
    [CssClassName("success")]
    Success,

    /// <summary>
    /// Info
    /// </summary>
    [CssVariableName("--tblr-info")]
    [CssClassName("info")]
    Info,

    /// <summary>
    /// Warning
    /// </summary>
    [CssVariableName("--tblr-warning")]
    [CssClassName("warning")]
    Warning,

    /// <summary>
    /// Danger
    /// </summary>
    [CssVariableName("--tblr-danger")]
    [CssClassName("danger")]
    Danger,

    /// <summary>
    /// Light
    /// </summary>
    [CssVariableName("--tblr-light")]
    [CssClassName("light")]
    Light,

    /// <summary>
    /// Dark
    /// </summary>
    [CssVariableName("--tblr-dark")]
    [CssClassName("dark")]
    Dark,

    /// <summary>
    /// Muted
    /// </summary>
    [CssVariableName("--tblr-muted")]
    [CssClassName("muted")]
    Muted,
    
    /// <summary>
    /// Blue
    /// </summary>
    [CssVariableName("--tblr-blue")]
    [CssClassName("blue")]
    Blue,

    /// <summary>
    /// Azure
    /// </summary>
    [CssVariableName("--tblr-azure")]
    [CssClassName("azure")]
    Azure,

    /// <summary>
    /// Indigo
    /// </summary>
    [CssVariableName("--tblr-indigo")]
    [CssClassName("indigo")]
    Indigo,

    /// <summary>
    /// Purple
    /// </summary>
    [CssVariableName("--tblr-purple")]
    [CssClassName("purple")]
    Purple,

    /// <summary>
    /// Pink
    /// </summary>
    [CssVariableName("--tblr-pink")]
    [CssClassName("pink")]
    Pink,

    /// <summary>
    /// Red
    /// </summary>
    [CssVariableName("--tblr-red")]
    [CssClassName("red")]
    Red,

    /// <summary>
    /// Orange
    /// </summary>
    [CssVariableName("--tblr-orange")]
    [CssClassName("orange")]
    Orange,

    /// <summary>
    /// Yellow
    /// </summary>
    [CssVariableName("--tblr-yellow")]
    [CssClassName("yellow")]
    Yellow,

    /// <summary>
    /// Lime
    /// </summary>
    [CssVariableName("--tblr-lime")]
    [CssClassName("lime")]
    Lime,

    /// <summary>
    /// Green
    /// </summary>
    [CssVariableName("--tblr-green")]
    [CssClassName("green")]
    Green,

    /// <summary>
    /// Teal
    /// </summary>
    [CssVariableName("--tblr-teal")]
    [CssClassName("teal")]
    Teal,

    /// <summary>
    /// Cyan
    /// </summary>
    [CssVariableName("--tblr-cyan")]
    [CssClassName("cyan")]
    Cyan,

    /// <summary>
    /// Gray 50
    /// </summary>
    [CssVariableName("--tblr-gray-50")]
    [CssClassName("gray-50")]
    Gray50,

    /// <summary>
    /// Gray 100
    /// </summary>
    [CssVariableName("--tblr-gray-100")]
    [CssClassName("gray-100")]
    Gray100,

    /// <summary>
    /// Gray 200
    /// </summary>
    [CssVariableName("--tblr-gray-200")]
    [CssClassName("gray-200")]
    Gray200,

    /// <summary>
    /// Gray 300
    /// </summary>
    [CssVariableName("--tblr-gray-300")]
    [CssClassName("gray-300")]
    Gray300,

    /// <summary>
    /// Gray 400
    /// </summary>
    [CssVariableName("--tblr-gray-400")]
    [CssClassName("gray-400")]
    Gray400,

    /// <summary>
    /// Gray 500
    /// </summary>
    [CssVariableName("--tblr-gray-500")]
    [CssClassName("gray-500")]
    Gray500,

    /// <summary>
    /// Gray 600
    /// </summary>
    [CssVariableName("--tblr-gray-600")]
    [CssClassName("gray-600")]
    Gray600,

    /// <summary>
    /// Gray 700
    /// </summary>
    [CssVariableName("--tblr-gray-700")]
    [CssClassName("gray-700")]
    Gray700,

    /// <summary>
    /// Gray 800
    /// </summary>
    [CssVariableName("--tblr-gray-800")]
    [CssClassName("gray-800")]
    Gray800,

    /// <summary>
    /// Gray 900
    /// </summary>
    [CssVariableName("--tblr-gray-900")]
    [CssClassName("gray-900")]
    Gray900,

    /// <summary>
    /// Facebook
    /// </summary>
    [CssVariableName("--tblr-facebook")]
    [CssClassName("facebook")]
    Facebook,

    /// <summary>
    /// Twitter / X
    /// </summary>
    [CssVariableName("--tblr-twitter")]
    [CssClassName("twitter")]
    Twitter,

    /// <summary>
    /// LinkedIn
    /// </summary>
    [CssVariableName("--tblr-linkedin")]
    [CssClassName("linkedin")]
    Linkedin,

    /// <summary>
    /// Google
    /// </summary>
    [CssVariableName("--tblr-google")]
    [CssClassName("google")]
    Google,

    /// <summary>
    /// Youtube
    /// </summary>
    [CssVariableName("--tblr-youtube")]
    [CssClassName("youtube")]
    Youtube,

    /// <summary>
    /// Vimeo
    /// </summary>
    [CssVariableName("--tblr-vimeo")]
    [CssClassName("vimeo")]
    Vimeo,

    /// <summary>
    /// Dribbble
    /// </summary>
    [CssVariableName("--tblr-dribbble")]
    [CssClassName("dribbble")]
    Dribbble,

    /// <summary>
    /// Github
    /// </summary>
    [CssVariableName("--tblr-github")]
    [CssClassName("github")]
    Github,

    /// <summary>
    /// Instagram
    /// </summary>
    [CssVariableName("--tblr-instagram")]
    [CssClassName("instagram")]
    Instagram,

    /// <summary>
    /// Pinterest
    /// </summary>
    [CssVariableName("--tblr-pinterest")]
    [CssClassName("pinterest")]
    Pinterest,

    /// <summary>
    /// VK
    /// </summary>
    [CssVariableName("--tblr-vk")]
    [CssClassName("vk")]
    Vk,

    /// <summary>
    /// Rss
    /// </summary>
    [CssVariableName("--tblr-rss")]
    [CssClassName("rss")]
    Rss,

    /// <summary>
    /// Flickr
    /// </summary>
    [CssVariableName("--tblr-flickr")]
    [CssClassName("flickr")]
    Flickr,

    /// <summary>
    /// Bitbucket
    /// </summary>
    [CssVariableName("--tblr-bitbucket")]
    [CssClassName("bitbucket")]
    Bitbucket,

    /// <summary>
    /// Tabler
    /// </summary>
    [CssVariableName("--tblr-tabler")]
    [CssClassName("tabler")]
    Tabler,
}