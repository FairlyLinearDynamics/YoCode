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
        ///   Looks up a localized string similar to ==========================================================.
        /// </summary>
        public static string Devider {
            get {
                return ResourceManager.GetString("Devider", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to 
        ///To run this application you will have to install Command Line Tools by Jetbrains
        ///Direct download link here: https://www.jetbrains.com/resharper/download/download-thanks.htmlplatform=windows&amp;code=RSCLT
        ///After you downloaded it please specify its location in appsetting.json file, which lives in the root directory of this  project.
        /// </summary>
        public static string DupFinderHelp {
            get {
                return ResourceManager.GetString("DupFinderHelp", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to ___________________________
        ///   _|__ _________________ __|__
        ///  _|___||               ||_|__
        ///  ___|_||       )  &apos;    ||___|_
        ///   _|__||    ( ()\(     ||_|___
        ///  ___|_||  ( ,|,(X)&apos;    ||___#_
        ///  _|___|| /,)/|`\``\\\  |||__/\
        ///       &apos;&apos;---------------&apos;&apos;  /  `--#
        ///       . - ------------ . #/      |
        ///     (( (((  (( ))) ))))  )\      |
        ///       `  -   ----  __ -/\  `.__.-#
        ///                  C(__)`\ \____
        ///                      /_`\/___/.
        /// </summary>
        public static string Fireplace {
            get {
                return ResourceManager.GetString("Fireplace", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Application takes 2 parameters: path to original test directory and path to modified test directory
        ///Possible commands: --{0}; --{1}; --{2}
        ///Example use: --{0}=&lt;path-to-original-test&gt; --{1}=&lt;path-to-modified-test&gt;
        ///.
        /// </summary>
        public static string HelpMessage {
            get {
                return ResourceManager.GetString("HelpMessage", resourceCulture);
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
    }
}
