using System;

namespace BLL
{
    public class ParseSyntaxError
    {
        public string Line         { get; }
        public uint   LineNumber   { get; }
        public uint   ColumnNumber { get; }
        public string Message      { get; }

        public ParseSyntaxError(
            string line,
            uint lineNumber,
            uint columnNumber,
            string message
        ) {
            if (line    == null) throw new ArgumentNullException(nameof(line));
            if (message == null) throw new ArgumentNullException(nameof(message));

            Line         = line;
            LineNumber   = lineNumber;
            ColumnNumber = columnNumber;
            Message      = message;
        }

        public override bool Equals(object obj)
        {
            var val = obj as ParseSyntaxError;

            if (val == null)
                return false;

            return
                Line         == val.Line         &&
                LineNumber   == val.LineNumber   &&
                ColumnNumber == val.ColumnNumber &&
                Message      == val.Message;
        }

        public override int GetHashCode()
        {
            return new
            {
                Line,
                LineNumber,
                ColumnNumber,
                Message
            }.GetHashCode();
        }

        public override string ToString()
        {
            return $"({LineNumber}, {ColumnNumber}): {Message}: '{Line}'";
        }
    }
}

