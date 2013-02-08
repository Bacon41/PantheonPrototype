using System;
using Microsoft.Xna.Framework.Content.Pipeline;
using System.Xml.Linq;
using System.IO;

namespace FuncWorks.XNA.XTiled {
    [ContentImporter(".tmx", DisplayName = "TMX Map Importer", DefaultProcessor = "TMXContentProcessor")]
    public class TMXContentImporter : ContentImporter<XDocument> {
        public override XDocument Import(string filename, ContentImporterContext context) {
            XDocument doc = XDocument.Load(filename);
            doc.Document.Root.Add(new XElement("File", 
                new XAttribute("name", Path.GetFileName(filename)),
                new XAttribute("path", Path.GetDirectoryName(filename))));

            return doc;
        }
    }
}
