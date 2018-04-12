﻿using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace MiroBozik.StringParser
{
    public class Template
    {
        private readonly string _templateStr;
        private string _pattern;

        public Template(string templateStr)
        {
            _templateStr = templateStr;
        }

        public void Compile()
        {            
            var tmpl = Regex.Replace(_templateStr, @"[\\\^\$\.\|\?\*\+\(\)]", m => "\\" + m.Value);
            
            _pattern = "^" + Regex.Replace(tmpl, @"\{([a-zA-Z_]+)\}", "(?<$1>[a-zA-Z_]+)") + "$";
        }        

        public bool IsMatch(string input)
        {
            return Regex.IsMatch(input, _pattern);
        }

        public IDictionary<string, string> Parse(string input)
        {
            var data = new Dictionary<string, string>();
            var regex = new Regex(_pattern);
            var match = regex.Match(input);
            var names = regex.GetGroupNames();
            foreach (var name in names.Where(n=>n!="0"))
            {
                data.Add(name, match.Groups[name].Value);
            }
            return data;
        }
    }
}
