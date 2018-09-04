using System.Collections.Generic;

namespace YoCode
{
    internal static class UIEnumErrFormat
    {
        public static List<string> ConvertEnum(List<UICheckErrEnum> errs)
        {
            List<string> formatedErrs = new List<string>();

            foreach(var err in errs)
            {
                switch (err)
                {
                    case UICheckErrEnum.noForm:
                        formatedErrs.Add("No form found to input test data");
                        break;
                    case UICheckErrEnum.noOptionInDoubleDropdownMenu:
                        formatedErrs.Add("Could not select conversion types in dropdown menues");
                        break;
                    case UICheckErrEnum.noOptionInSingleDropdownMenu:
                        formatedErrs.Add("Could not select conversion type in dropdown menu");
                        break;
                    case UICheckErrEnum.noProperSelectTags:
                        formatedErrs.Add("Too many select elements found");
                        break;
                    default:
                        break;
                }
            }
            return formatedErrs;
        }
    }
}
