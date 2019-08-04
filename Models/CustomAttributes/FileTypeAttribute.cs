using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Web;



namespace Models.CustomAttributes
{
    public class FileTypeAttribute : ValidationAttribute
    {
        private List<string> _types;
        private bool _isRequired;

        public FileTypeAttribute(string[] fileExtentions, bool isFileRequired)
        {
            _isRequired = isFileRequired;
            _types = new List<string>();
            foreach (string item in fileExtentions)
            {
                if (!String.IsNullOrWhiteSpace(item))
                {
                    if (item[0] != '.')
                        _types.Add('.' + item);
                    else
                        _types.Add(item);
                }
            }
        }

        public override bool IsValid(object value)
        {
            HttpPostedFileBase input = value as HttpPostedFileBase;
            if (input != null)
            {
                foreach (string fileType in _types)
                {
                    if (input.FileName.EndsWith(fileType, StringComparison.OrdinalIgnoreCase))
                        return true;
                }
                return false;
            }
            else return !_isRequired;
        }

        public override string FormatErrorMessage(string name)
        {
            if(!String.IsNullOrEmpty(name))
                return base.FormatErrorMessage(name);
            else
            {
                StringBuilder message = new StringBuilder();
                message.Append("Please use ");
                for (int i = 0; i < _types.Count; i++)
                {
                    message.Append($"{_types[i]} ");
                    if (i + 1 < _types.Count)
                        message.Append("or ");
                    else
                        message.Append("files only");
                }
                return message.ToString();
            }
        }
    }
}
