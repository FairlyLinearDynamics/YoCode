﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace YoCode {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "15.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public class messages {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal messages() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("YoCode.messages", typeof(messages).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to appsettings.json file missing. Please make sure to create the file as specified in the README.
        /// </summary>
        public static string AppsettingsHelp {
            get {
                return ResourceManager.GetString("AppsettingsHelp", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to If you would like to see list of commands, type: --help.
        /// </summary>
        public static string AskForHelp {
            get {
                return ResourceManager.GetString("AskForHelp", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Inputs certain values directly to the web app through the backend and checks if the issues deliberately left in the test project have been fixed. YoScore for this check is calculated by taking the percentage of issues fixed..
        /// </summary>
        public static string BadInputCheck {
            get {
                return ResourceManager.GetString("BadInputCheck", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Could not retrieve the port number. Another program might be using it..
        /// </summary>
        public static string BadPort {
            get {
                return ResourceManager.GetString("BadPort", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Runs an external tool that calculates the code coverage. YoScore for this check is calculated by taking the code coverage and adding a certain weight to it..
        /// </summary>
        public static string CodeCoverageCheck {
            get {
                return ResourceManager.GetString("CodeCoverageCheck", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Install dotCover, Download link: https://www.jetbrains.com/dotcover/download/#section=commandline
        ///After you downloaded it please specify its location in appsettings.json file, which lives in the root directory of this project.
        /// </summary>
        public static string CodeCoverageHelp {
            get {
                return ResourceManager.GetString("CodeCoverageHelp", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to 
        ///______________________________
        ///___|__ _________________ __|__
        ///_|___ ||           |   ||_|__
        ///___|_ ||       )  &apos;|   ||___|_
        ///_|____||    ( ()\( |   ||_|___
        ///___|_ ||  ( ,|,(X)&apos;|   ||___#_
        ///_|___ || /,)/|`\``\\\  || __/\
        ///      &apos;&apos;---------------&apos;&apos;  /  `--#
        ///      . - ------------ . #/      |
        ///    (( (((  (( ))) ))))  )\      |
        ///      `  -   ---- __ -/\  `.__.-#
        ///                C(__)`\ \____
        ///                    /_`\/___/.
        /// </summary>
        public static string ConsoleFireplaceBannerFrame1 {
            get {
                return ResourceManager.GetString("ConsoleFireplaceBannerFrame1", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to 
        ///______________________________
        ///___|__ _________________ __|__
        ///_|___ ||    ,      |   ||_|__
        ///___|_ ||     )    (|   ||___|_
        ///_|____||  ,) ,() ( |   ||_|___
        ///___|_ || )  )|,(X)&apos;|   ||___#_
        ///_|___ || /,)/|`\``\\\  || __/\
        ///      &apos;&apos;-------  ------&apos;&apos;  /  `--#
        ///      . - ---------  - . #/      |
        ///    (( (((  (( ))) ))))  )\      |
        ///      `  -   ---- __ -/\  `.__.-#
        ///                C(__)`\ \____
        ///                    /_`\/___/.
        /// </summary>
        public static string ConsoleFireplaceBannerFrame2 {
            get {
                return ResourceManager.GetString("ConsoleFireplaceBannerFrame2", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to 
        ///______________________________
        ///___|__ _________________ __|__
        ///_|___ ||           |   ||_|__
        ///___|_ ||  )     (  |   ||___|_
        ///_|____||   ) ,())  | , ||_|___
        ///___|_ ||   ,)|,(X)&apos;|   ||___#_
        ///_|___ || /,)/|`\``\\\  || __/\
        ///      &apos;&apos;-----  --- ----&apos;&apos;  /  `--#
        ///      . - ------- ---- . #/      |
        ///    (( (((  (( ))) ))))  )\      |
        ///      `  -   ---- __ -/\  `.__.-#
        ///                C(__)`\ \____
        ///                    /_`\/___/.
        /// </summary>
        public static string ConsoleFireplaceBannerFrame3 {
            get {
                return ResourceManager.GetString("ConsoleFireplaceBannerFrame3", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to 
        ///______________________________
        ///___|__ _________________ __|__
        ///_|___ ||   ,   .   |   ||_|__
        ///___|_ || (  ,   (  |.  ||___|_
        ///_|____||  (   ())  |  )||_|___
        ///___|_ || ) ,)|,(X) | . ||___#_
        ///_|___ || /,)/|`\``\\\  || __/\
        ///      &apos;&apos;-  ------------&apos;&apos;  /  `--#
        ///      . - ------   --- . #/      |
        ///    (( (((  (( ))) ))))  )\      |
        ///      `  -   ---- __ -/\  `.__.-#
        ///                C(__)`\ \____
        ///                    /_`\/___/.
        /// </summary>
        public static string ConsoleFireplaceBannerFrame4 {
            get {
                return ResourceManager.GetString("ConsoleFireplaceBannerFrame4", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to 
        ///______________________________
        ///___|__ _________________ __|__
        ///_|___ ||           |   ||_|__
        ///___|_ ||  ,   )   .|   ||___|_
        ///_|____||   ),( )   |,  ||_|___
        ///___|_ ||  ,()|,(X) | ) ||___#_
        ///_|___ || /,)/|`\``\\\  || __/\
        ///      &apos;&apos;---- -   ------&apos;&apos;  /  `--#
        ///      . - ---------  - . #/      |
        ///    (( (((  (( ))) ))))  )\      |
        ///      `  -   ---- __ -/\  `.__.-#
        ///                C(__)`\ \____
        ///                    /_`\/___/.
        /// </summary>
        public static string ConsoleFireplaceBannerFrame5 {
            get {
                return ResourceManager.GetString("ConsoleFireplaceBannerFrame5", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to ===================================================================================
        ///.
        /// </summary>
        public static string Divider {
            get {
                return ResourceManager.GetString("Divider", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to To run this application you will have to install Command Line Tools by Jetbrains
        ///Direct download link here: https://www.jetbrains.com/resharper/download/download-thanks.html?platform=windows&amp;code=RSCLT
        ///After you downloaded it please specify its location in appsettings.json file, which lives in the root directory of this project.
        /// </summary>
        public static string DupFinderHelp {
            get {
                return ResourceManager.GetString("DupFinderHelp", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Runs an external tool that calculates how much code and duplication is in the project under test and compares it to the unmodified version of the project (note: Original Codebase Cost and Duplicate Cost is stored in appsettings.json). YoScore for this check is calculated by comparing unmodified and modified Codebase Costs and Duplicate Costs..
        /// </summary>
        public static string DuplicationCheck {
            get {
                return ResourceManager.GetString("DuplicationCheck", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Uses Git to get the files modified/added/deleted, number of insertions and deletions in each file and Untracked/Uncommited files between the last commit made by a Waters employee and head. This check does not contribute to the overall YoScore..
        /// </summary>
        public static string FilesChangedCheck {
            get {
                return ResourceManager.GetString("FilesChangedCheck", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Uses Git to check if the project under tests contains a valid Git Repository. YoScore for this check will be 100% if it finds a valid Repository and 0% if it doesn’t (also multiplied by its weighting)..
        /// </summary>
        public static string GitCheck {
            get {
                return ResourceManager.GetString("GitCheck", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Application takes only one parameter: path to the modified test directory
        ///Possible commands: --{0}; --{1}; --{2}; --{3}; --{4}; --{5};
        ///Example use: --{0}=&lt;path-to-modified-test&gt;
        ///To disable loading animation, append --{2} at the end of command line
        ///Example: --{0}=&lt;...&gt; --{2}
        ///To disable automatic HTML report opening, append --{3} at the end of command line
        ///Example: --{0}=&lt;...&gt; --{3}
        ///To specify directory to witch generate HTML report, append --{4} at the end of command line
        ///Example --{0}=&lt;...&gt; --{4}=&lt;p [rest of string was truncated]&quot;;.
        /// </summary>
        public static string HelpMessage {
            get {
                return ResourceManager.GetString("HelpMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &lt;div class=&quot;paragraph-banner&quot;&gt;
        ///          &lt;iframe src=&quot;https://giphy.com/embed/Hj7mksbFWIOdO&quot; width=&quot;480&quot; height=&quot;317&quot; frameBorder=&quot;0&quot;&gt;&lt;/iframe&gt;
        ///      &lt;/div&gt;.
        /// </summary>
        public static string HtmlFireplaceBanner {
            get {
                return ResourceManager.GetString("HtmlFireplaceBanner", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &lt;div class=&quot;paragraph-block&quot;&gt;
        ///  {CONTENT}
        ///&lt;/div&gt;.
        /// </summary>
        public static string HtmlParagraphBlock {
            get {
                return ResourceManager.GetString("HtmlParagraphBlock", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to 
        ///&lt;!DOCTYPE html&gt;
        ///&lt;html&gt;
        ///
        ///  &lt;head&gt;
        ///    &lt;meta charset=&quot;utf-8&quot;&gt;
        ///    &lt;meta http-equiv=&quot;x-ua-compatible&quot; content=&quot;ie=edge&quot;&gt;
        ///    &lt;meta name=&quot;viewport&quot; content=&quot;width=device-width, initial-scale=1.0&quot;&gt;
        ///
        ///    &lt;title&gt;YoCode Report&lt;/title&gt;
        ///
        ///    &lt;link rel=&quot;stylesheet&quot; href=&quot;https://cdnjs.cloudflare.com/ajax/libs/font-awesome/4.7.0/css/font-awesome.min.css&quot;&gt;
        ///    &lt;style&gt;
        ///       :root{
        ///        --console-bg: #222121;
        ///        --console-text: #c0c0c0;
        ///        --green-bg: #3bc511;
        ///        --red-bg: #eb4b4b;
        ///  [rest of string was truncated]&quot;;.
        /// </summary>
        public static string HtmlTemplate {
            get {
                return ResourceManager.GetString("HtmlTemplate", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &lt;!DOCTYPE html&gt;
        ///&lt;html&gt;
        ///
        ///  &lt;head&gt;
        ///    &lt;meta charset=&quot;utf-8&quot;&gt;
        ///    &lt;meta http-equiv=&quot;x-ua-compatible&quot; content=&quot;ie=edge&quot;&gt;
        ///    &lt;meta name=&quot;viewport&quot; content=&quot;width=device-width, initial-scale=1.0&quot;&gt;
        ///
        ///    &lt;title&gt;YoCode Report&lt;/title&gt;
        ///    &lt;style&gt;
        ///      :root{
        ///        --main-bg: #d6cbda;
        ///        --panel-bg: #f4eef8;
        ///        --panel-text: #3b3838;
        ///        --toolbar-bg: #8B2C87;
        ///        --toolbar-text: #e2d7d7;
        ///        --element-underlining: #c1b8c9;
        ///      }
        ///
        ///      body {
        ///        background-color:  [rest of string was truncated]&quot;;.
        /// </summary>
        public static string HtmlTemplate_HelpPage {
            get {
                return ResourceManager.GetString("HtmlTemplate_HelpPage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &lt;span class=&quot;accordion-icon {0}&quot;&gt;
        ///    &lt;span class=&quot;fa {1}&quot;&gt;&lt;/span&gt;
        ///  &lt;/span&gt;
        ///  &lt;span class=&quot;accordion-score-holder&quot;&gt;
        ///    &lt;span class=&quot;accordion-score&quot;&gt;{2}&lt;/span&gt;
        ///  &lt;/span&gt;&lt;span class=&quot;accordion-title&quot;&gt;{3}&lt;span class=&quot;weight&quot;&gt;raw score: {4}, weighting: {5}&lt;/span&gt;&lt;/span&gt;
        ///  .
        /// </summary>
        public static string HtmlTitleTemplate {
            get {
                return ResourceManager.GetString("HtmlTitleTemplate", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &lt;div class=&quot;accordion&quot;&gt;
        ///                    {TITLE}
        ///                &lt;/div&gt;
        ///                &lt;div class=&quot;panel&quot;&gt;
        ///                    &lt;div class=&quot;panel-content&quot;&gt;
        ///                      &lt;div class=&quot;panel-info&quot;&gt;
        ///                        &lt;span class=&quot;fa fa-info-circle&quot;&gt;&lt;/span&gt;
        ///                      &lt;/div&gt;
        ///
        ///                      &lt;div class=&quot;panel-info-text&quot;&gt;
        ///                        {INFO-CONTENT}
        ///                      &lt;/div&gt;
        ///
        ///                      &lt;div class=&quot;panel-check-text&quot;&gt;
        ///                         [rest of string was truncated]&quot;;.
        /// </summary>
        public static string ListElementTemplate {
            get {
                return ResourceManager.GetString("ListElementTemplate", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Get comfortable, YoCode is gathering your results.
        /// </summary>
        public static string LoadingMessage {
            get {
                return ResourceManager.GetString("LoadingMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to -----------------------------------------------------------------------------------.
        /// </summary>
        public static string ParagraphDivider {
            get {
                return ResourceManager.GetString("ParagraphDivider", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to HTML successfully generated in {0}&quot;{1}&quot; directory.
        /// </summary>
        public static string SuccessfullyWroteReport {
            get {
                return ResourceManager.GetString("SuccessfullyWroteReport", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Runs all unit tests of the project under test. YoScore for this check depends on how many tests are written and how many pass.
        ///Broken tests refer to purposely included tests without the &quot;Theory&quot; tag at the top in the Original project.
        /// </summary>
        public static string TestCountCheck {
            get {
                return ResourceManager.GetString("TestCountCheck", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Uses Selenium to find the text box for the web app, tries to input certain values and checks if the issues deliberately left in the test project have been fixed. YoScore for this check is calculated by taking the percentage of issues fixed..
        /// </summary>
        public static string UIBadInputCheck {
            get {
                return ResourceManager.GetString("UIBadInputCheck", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Checks .cshtml files for keywords that would suggest that the user interface was implemented. YoScore for this check will be 100% if it finds any evidence and 0% if it doesn’t (also multiplied by its weighting)..
        /// </summary>
        public static string UICodeCheck {
            get {
                return ResourceManager.GetString("UICodeCheck", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Uses Selenium to look for places to input values and tries to input numbers, then tries to find buttons with keywords and click on it, checks if the output is correct. YoScore for this check will be 100% if all the values got expected results and 0% if they aren’t (also multiplied by its weighting)..
        /// </summary>
        public static string UIConversionCheck {
            get {
                return ResourceManager.GetString("UIConversionCheck", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Looks at certain HTML elements of the web app and looks for evidence (keywords) that would suggest that the user interface was implemented. YoScore for this check will be 100% if it finds any evidence and 0% if it doesn’t (also multiplied by its weighting)..
        /// </summary>
        public static string UIFeatureImplemented {
            get {
                return ResourceManager.GetString("UIFeatureImplemented", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Inputs certain values directly to the web app through the backend and checks that those values will have the expected results. YoScore for this check is calculated by taking the percentage of inputs that had the expected values..
        /// </summary>
        public static string UnitConverterCheck {
            get {
                return ResourceManager.GetString("UnitConverterCheck", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Welcome to YoCode!
        ///
        ///.
        /// </summary>
        public static string Welcome {
            get {
                return ResourceManager.GetString("Welcome", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to HTML report could not be written to &quot;{0}&quot; directory.{1}If you would like to specify different directory,{2}please append &quot;--output=&lt;path-where-to-write&gt;&quot; command.
        /// </summary>
        public static string WrongWritePermission {
            get {
                return ResourceManager.GetString("WrongWritePermission", resourceCulture);
            }
        }
    }
}
